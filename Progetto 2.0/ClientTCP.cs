using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.IO.Compression;

namespace Progetto_2._0
{
    class ClientTCP
    {
        private TcpClient tcpClient;
        private string pathFile;
        private string userName;
        private IPEndPoint endPoint;
        private const int buffer = 4096;
        private bool IsFolder;
        private SettingsForm settingsForm;
        private Boolean sendingCompleted = true;

        private byte[] CreateRequestMessage(ref int messageSize)
        {
            try
            {
                //get file info
                FileInfo fileInfo = new FileInfo(pathFile);

                //create request message: 1 byte isFolder, 8 byte dimension file, 4 byte dimension file name, 4 byte dimension user name, n bit file name, n bit user name
                byte[] fileNameByte = Encoding.UTF8.GetBytes(fileInfo.Name);
                byte[] userNameByte = Encoding.UTF8.GetBytes(userName);
                int userNameSize = userNameByte.Length;
                int fileNameSize = fileNameByte.Length;
                long fileSize = fileInfo.Length;
                messageSize = 17 + userNameSize + fileNameSize;
                byte[] request = new byte[messageSize];

                request[0] = (IsFolder) ? (byte)1 : (byte)0;
                Buffer.BlockCopy(BitConverter.GetBytes(fileSize), 0, request, 1, 8); //big/little endian 
                Buffer.BlockCopy(BitConverter.GetBytes(fileNameSize), 0, request, 9, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(userNameSize), 0, request, 13, 4);
                Buffer.BlockCopy(fileNameByte, 0, request, 17, fileNameSize);
                Buffer.BlockCopy(userNameByte, 0, request, 17 + fileNameSize, userNameSize);

                return request;
            }
            catch (Exception ex) {
                //ArgumentException
                //ArgumentNullException
                //ArgumentOutOfRangeException
                //EncoderFallbackException
                //OverflowException
                //FileNotFOundException
                //IOException
                //SecurityException
                //PathTooLongException
                //UnauthorizedAccessException
                //NotSupportedException
                return null;
            }
        }
        private ClientTCP()
        {
        }
        public ClientTCP(string pathFile, bool IsFolder, string userName, IPEndPoint endPoint, SettingsForm settingsForm)
        {
            this.pathFile = pathFile;
            this.userName = userName;
            this.endPoint = endPoint;
            this.IsFolder = IsFolder;
            this.settingsForm = settingsForm;
        }
        public void Execute()
        {
            try
            {
                //if is a folder zip it and send it
                if (IsFolder)
                {
                    //give a not existing name
                    int c = 0;
                    String zipName = Path.GetFileName(pathFile) + ".zip";
                    while (File.Exists(pathFile + "\\..\\" + zipName))
                    {
                        c++;
                        zipName = Path.GetFileName(pathFile) + "(" + c + ").zip";
                    }

                    //create zip and change the pathFile
                    ZipFile.CreateFromDirectory(pathFile, pathFile + "\\..\\" + zipName, CompressionLevel.Optimal,true);
                    pathFile = pathFile + "\\..\\" + zipName;
                }

                //initialize socket
                tcpClient = new TcpClient();

                //connect to server
                tcpClient.Connect(endPoint.Address, endPoint.Port);

                //create request message
                int messageSize = 0;
                byte[] request = CreateRequestMessage(ref messageSize);

                //get network stream
                NetworkStream stream = tcpClient.GetStream();

                //send request message
                stream.Write(request, 0, messageSize);
                
                byte[] answer = new byte[1];

                // Read can return anything from 0 to numBytesToRead. 
                stream.Read(answer, 0, 1);
                
                //check the answer
                if (answer[0] == 1)
                {
                    //receiver does want the file
                    //send the file
                    SendFile(stream);
                }
                
                //close connection (finally)
                tcpClient.Close();

                //if is a folder delete the zip File
                if (IsFolder)
                {
                    File.Delete(pathFile);
                }

                //send notifications
                if (sendingCompleted) { 
                    settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { "File sent correctly", false });
                }
            }
            
            catch (Exception e)
            {
                //InvalidOperationException
                //ArgumentNullException
                //ArgumentException
                //ArgumentOutOfRangeException
                //IOException
                //ObjectDisposedException
                //DirectoryNotFoundException
                //NotSupportedException
                //PathTooLongException
                //UnauthorizedAccessException
                //SocketException
                Console.WriteLine(e.ToString());
            }
        }

        private void SendFile(NetworkStream stream)
        {
            try
            {
                //create file
                FileStream file = File.OpenRead(pathFile);
                long fileSize = new FileInfo(pathFile).Length;

                //set array to receive
                byte[] data = new byte[buffer];
                int read;
                long totRead = 0;
                Stopwatch timer;
                int percentage = 0;
                string remainingSeconds = "";

                //set the timer
                timer = new Stopwatch();
                timer.Start();
                int counter = -1;

                //create condition vatiable and mutex
                string fileName = Path.GetFileName(pathFile);
                Object locker = new Object();
                Flag isCreated = new Flag(false);
                Flag cancel = new Flag(false);

                //create form
                ProgressBar progressBarForm = new ProgressBar(isCreated, locker, cancel, "Sending " + fileName + "...");
                Task.Run(() => { progressBarForm.ShowDialog(); });

                //wait until form is created
                lock (locker)
                {
                    while (isCreated.value() == false)
                    {
                        Monitor.Wait(locker);
                    }
                }

                while (fileSize > totRead)
                {
                    lock (locker)
                    {
                        //if the cancel button is pressed stop receiving
                        if (cancel.value() == true)
                        {
                            sendingCompleted = false;
                            break;
                        }
                    }

                    //if byte to be receved are less than buffer size reset the buffer
                    if (fileSize - totRead < buffer)
                    {
                        data = new byte[fileSize];
                    }

                    //read data from file
                    read = file.Read(data, 0, data.Length);
                   
                    //check if read is 0 (nothing to read or closed connection)
                    if (read == 0)
                    {
                        //close file and close connection
                        file.Flush();
                        file.Close();
                        progressBarForm.BeginInvoke(progressBarForm.closeFormDelegate);
                        settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { "Cannot read the file", true });
                        return;
                    }

                    //write to networkstream
                    stream.Write(data, 0, read); //write scrive tutti i byte di read sicuro??
                    totRead = totRead + read;
                    
                    //set remaining time and pecentage once at second
                    if (counter < (int)timer.Elapsed.TotalSeconds)
                    {
                        counter = (int)timer.Elapsed.TotalSeconds;
                        Utilities.SetPercentage(ref percentage, fileSize, totRead);
                        Utilities.SetRemainingTime(ref remainingSeconds, timer.Elapsed, fileSize, totRead);

                        progressBarForm.BeginInvoke(progressBarForm.percentageDelegate, new object[] { percentage });
                        progressBarForm.BeginInvoke(progressBarForm.timeDelegate, new object[] { new String(remainingSeconds.ToCharArray()) });
                    }
                }
                
                timer.Stop();

                if (fileSize != totRead && cancel.value() == false)
                {
                    //errore
                    Console.WriteLine("ERROR - Receiver.receivefile");
                }

                //flush and close fileStream (finally)
                progressBarForm.BeginInvoke(progressBarForm.closeFormDelegate);
                file.Flush();
                file.Close();
                
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e.ToString());
                //IOException
                //ObjectDisposedException
                //InvalidOperationException
                //ArgumentOutOfRangeException
                //ArgumentNullException
                //ArgumentException
                //NotSupportedException
                //PathTooLongException
                //UnauthorizedAccessException
                //SecurityException
                //SynchronizationLockException
                //ThreadInterruptedException

                //check if file exists 
            }
        }

    }
}
