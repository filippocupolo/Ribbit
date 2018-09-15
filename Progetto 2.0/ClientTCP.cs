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
using System.Windows.Forms;
using System.Security;

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
            //if is a folder zip it and send it
            if (IsFolder)
            {
                try
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
                    ZipFile.CreateFromDirectory(pathFile, pathFile + "\\..\\" + zipName, CompressionLevel.Optimal, true);
                    pathFile = pathFile + "\\..\\" + zipName;
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("You do not have the permission to open the folder", "Folder permission error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Folder do not exist anymore", "Folder not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                    return;
                }
            }

            bool aswerDeclined = false;

            try
            {

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
                if (stream.Read(answer, 0, 1) == 0) {

                    //if no byte is read connenction is closed
                    settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { "Unable to send or receive message with the server", true });
                    tcpClient.Close();
                    return;
                }

                //check the answer
                if (answer[0] == 1)
                {
                    //receiver does want the file
                    //send the file
                    SendFile(stream);
                }
                else
                {
                    aswerDeclined = true;
                }
            }
            catch (Exception e) {

                String mx;

                if (e is SocketException)
                {
                    mx = "Unable to connect to the server";
                }
                else if (e is IOException)
                {
                    mx = "Unable to send or receive message with the server";
                }
                else if (e is SecurityException || e is UnauthorizedAccessException)
                {
                    mx = "Unable to open the file";
                }
                else if (e is RibbitException)
                {
                    RibbitException ribbitException = (RibbitException) e;
                    if (ribbitException.Parameter.Equals("File"))
                    {
                        mx = "Unable to open the file";
                    }
                    else if (ribbitException.Parameter.Equals("Socket"))
                    {
                        mx = "Network problems, unable to send the file";
                    }
                    else 
                    {
                        mx = "Unable to send the file";
                    }
                }
                else
                {
                    mx = "Unable to send the file";
                }

                settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { mx, true });
                tcpClient.Close();
                Console.WriteLine(e.ToString());
                return;
            }

            //close connection
            tcpClient.Close();
            
            //if is a folder delete the zip File
            if (IsFolder)
            {
                try {
                    File.Delete(pathFile);
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            //send notifications
            if (sendingCompleted) {
                String msg;
                if (aswerDeclined)
                {
                    msg = "Receiver declined your file/folder";
                }
                else
                {
                    msg = "File sent correctly";
                }
                settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { msg, false });
            }        
        }

        private void SendFile(NetworkStream stream)
        {
            Flag isCreated = new Flag(false);
            ProgressBar progressBarForm = null;
            FileStream file = null;

            try
            {
                //create file
                file = File.OpenRead(pathFile);
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
                Flag cancel = new Flag(false);

                //create form
                progressBarForm = new ProgressBar(isCreated, locker, cancel, "Sending " + fileName + "...");
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

                    try
                    {
                        //write to networkstream
                        stream.Write(data, 0, read);
                        totRead = totRead + read;
                    }
                    catch (IOException)
                    {
                        throw new RibbitException("Socket");
                    }

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

                //flush fileStream
                file.Flush();
            }
            catch (RibbitException e) {
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                RibbitException ribbitException;
                if (e is UnauthorizedAccessException || e is SecurityException || e is IOException)
                {
                    ribbitException = new RibbitException("File");
                }
                else
                {
                    ribbitException = new RibbitException("Generic");
                }

                throw ribbitException;
            }
            finally
            {
                if (progressBarForm != null && isCreated.value())
                {
                    progressBarForm.BeginInvoke(progressBarForm.closeFormDelegate);
                }
                if (file != null)
                {
                    file.Close();
                }
            }
        }

    }
}
