using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Progetto_2._0
{
    class Receiver
    {
        private const int dimentionsLenght = 17;
        private const int buffer = 4096;
        private TcpClient connectedSocket;
        private bool automaticAnswer;
        private string pathDest;
        private bool isFolder;
        private SettingsForm settingsForm;
        private Receiver() { }
        public Receiver(TcpClient connectedSocket, bool automaticAnswer, string pathDest, SettingsForm settingsForm)
        {
            this.connectedSocket = connectedSocket;
            this.automaticAnswer = automaticAnswer;
            this.pathDest = pathDest;
            this.settingsForm = settingsForm;
        }

        private void ReceiveFile(NetworkStream stream, long fileSize, string fileName)
        {
            try
            {
                //set filepath
                string filepath = string.Concat(pathDest, "\\");
                filepath = string.Concat(filepath, fileName);

                if (File.Exists(filepath))
                {
                    //ask if the user want to replace the file or create another or cancel
                    String message = "Do you want to overwrite file?";
                    String caption = "File already exist";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    DialogResult result = MessageBox.Show(message, caption, buttons);

                    if (result == DialogResult.No)
                    {
                        //rename file
                        int i = 1;
                        String extension = Path.GetExtension(filepath);
                        String filename = Path.GetFileNameWithoutExtension(filepath);
                        String tempFilePath = filename + "(" + i + ")";
                        while (File.Exists(pathDest + "\\" + tempFilePath + extension))
                        {
                            i++;
                            tempFilePath = filename + "(" + i + ")";
                        }

                        filepath = pathDest + "\\" + tempFilePath + extension;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                //create file
                FileStream file = File.Create(filepath);

                //set array to receive
                byte[] data = new byte[buffer];
                int read;
                long totRead = 0;
                Stopwatch timer;
                int percentage = 0;
                String remainingSeconds = "";

                //set the timer
                timer = new Stopwatch();
                timer.Start();
                int counter = -1;

                //create condition vatiable and mutex
                Object locker = new Object();
                Flag isCreated = new Flag(false);
                Flag cancel = new Flag(false);

                //create form
                ProgressBar progressBarForm = new ProgressBar(isCreated, locker, cancel, "Receiving " + fileName);
                Task.Run(() => { progressBarForm.ShowDialog(); });

                //wait until form is created
                lock (locker)
                {
                    while (isCreated.value() == false)
                    {
                        Monitor.Wait(locker);
                    }
                }

                Boolean sendingCompleted = true;

                //send file
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
                        data = new byte[fileSize - totRead];
                    }

                    //read data from stream
                    read = stream.Read(data, 0, data.Length);

                    //check if read is 0 (nothing to read or closed connection)
                    if (read == 0)
                    {
                        //delete file and close connection
                        file.Flush();
                        file.Close();
                        File.Delete(filepath);
                        progressBarForm.BeginInvoke(progressBarForm.closeFormDelegate);
                        settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { "Connection interrupted by the other user", true });
                        return;
                    }

                    //write data to file
                    file.Write(data, 0, read);
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

                //close progressbar, send notification, flush and close fileStream (finally)
                progressBarForm.BeginInvoke(progressBarForm.closeFormDelegate);
                if (sendingCompleted)
                {
                    settingsForm.BeginInvoke(settingsForm.DownloadStateDelegate, new object[] { "File received correctly", false });
                }
                file.Flush();
                file.Close();

            }catch(Exception ex)
            {
                //IOException
                //ObjectDisposedException
                //InvalidOperationException
                //NotSupportedException
                //ArgumentException
                //ArgumentNullException
                //ArgumentOutOfRangeException
                //DirectoryNotFoundException
                //PathTooLongException
                //UnauthorizedAccessException
                //SynchronizationLockException
                //ThreadInterruptedException
                //InvalidEnumArgumentException
            }
        }

        public bool Answer(NetworkStream stream, string fileName, string userName) {
            try
            {
                //set answer
                byte[] answer = new byte[1];
                if (automaticAnswer)
                {
                    answer[0] = 1;
                }
                else
                {
                    string file_folder = (isFolder) ? "la cartella" : "il file";

                    //ask to user
                    if (MessageBox.Show("Richiesta di ricezione", "Vuoi ricevere " + file_folder + fileName + " da " + userName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        answer[0] = 1;
                    }
                    else
                    {
                        answer[0] = 0;
                    }
                }

                //send answer
                stream.Write(answer, 0, 1);
                if (answer[0] == 0) { return false; }
                else { return true; }

            }
            catch(Exception ex)
            {
                return false;
                //ArgumentNullException
                //ArgumentOutOfRangeException
                //IOException
                //ObjectDisposedException
                //InvalidEnumArgumentException
                //InvalidOperationException

            }
            
        }
        public void Execute()
        {
            try
            {
                //create the stream
                NetworkStream stream = connectedSocket.GetStream();

                //receive size of file, username and filename
                byte[] readData = new byte[dimentionsLenght];

                //non so se è giusto. Capire se read è bloccante
                stream.ReadTimeout = 10000;

                if (stream.Read(readData, 0, readData.Length)!= dimentionsLenght) {      
                    //error must be 17 byte
                }

                //set isFolder byte, filesize, filenamesize and usernamesize
                isFolder = (readData[0]==1) ? true : false;
                long fileSize = BitConverter.ToInt64(readData, 1);
                int filenameSize = BitConverter.ToInt32(readData, 9);
                int usernameSize = BitConverter.ToInt32(readData, 13);

                //receive file and user name
                readData = new byte[filenameSize + usernameSize];
                if (stream.Read(readData, 0, readData.Length) != filenameSize + usernameSize)
                {
                    //error must be filenameSize + usenameSize byte
                }

                //get file name and user name
                byte[] bufferFileName = new byte[filenameSize];
                byte[] bufferUserName = new byte[usernameSize];
                Buffer.BlockCopy(readData, 0, bufferFileName, 0, filenameSize);
                Buffer.BlockCopy(readData, filenameSize, bufferUserName, 0, usernameSize);
                string fileName = Encoding.UTF8.GetString(bufferFileName);
                string userName = Encoding.UTF8.GetString(bufferUserName);

                //send answer
                if (Answer(stream, fileName, userName))
                {
                    //receive file
                    ReceiveFile(stream, fileSize, fileName);
                }
                
                //close socket (finally)
                connectedSocket.Close();
            }
            catch (Exception e)
            {
                //ArgumentNullException
                //ArgumentOutOfRangeException
                //IOException
                //ObjectDisposedException
                //ArgumentException
                //InvalidOperationException
                //DecoderFallbackException
                Console.WriteLine(e.ToString());
            }
        }
    }
}       
