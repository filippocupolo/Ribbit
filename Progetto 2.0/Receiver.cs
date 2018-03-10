using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Receiver() { }
        public Receiver(TcpClient connectedSocket, bool automaticAnswer, string pathDest)
        {
            this.connectedSocket = connectedSocket;
            this.automaticAnswer = automaticAnswer;
            this.pathDest = pathDest;
        }

        private void ReceiveFile(NetworkStream stream, long fileSize, string fileName)
        {
            //set filepath
            string filepath = string.Concat(pathDest, "\\" );
            filepath = string.Concat(filepath, fileName);

            if (File.Exists(filepath))
            {
                //file already exist do something
            }

            //create file
            FileStream file = File.Create(filepath);

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

            while (fileSize > totRead) {

                //if byte to be receved are less than buffer size reset the buffer
                if (fileSize < buffer)
                {
                    data = new byte[fileSize];
                }
               
                //read data from stream
                read = stream.Read(data,0,data.Length);

                //check if read is 0 (nothing to read or closed connection)
                if (read == 0)
                {
                    //do something
                }
                
                //write data to file
                file.Write(data, 0, read);
                totRead = totRead + read;

                //set remaining time and pecentage
                
                Utilities.SetPercentage(ref percentage, fileSize, totRead);
                if (counter < timer.Elapsed.Seconds)
                {
                    counter = timer.Elapsed.Seconds;
                    Utilities.SetRemainingTime(ref remainingSeconds, timer.Elapsed, fileSize, totRead);
                }
                Console.WriteLine(percentage + "%       remaining " + remainingSeconds + " seconds");
            }

            timer.Stop();

            if (fileSize != totRead)
            {
                //errore
                Console.WriteLine("ERROR - Receiver.receivefile");
            }

            //flush and close fileStream (finally)
            file.Flush();
            file.Close();
        }

        public bool Answer(NetworkStream stream, string fileName, string userName) {

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
                if (MessageBox.Show("Richiesta di ricezione", "Vuoi ricevere " + file_folder + fileName + " da " +userName, MessageBoxButtons.YesNo) == DialogResult.Yes)
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

            if(answer[0] == 0) { return false; }
            else { return true; }
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
                Console.WriteLine(e.ToString());
            }
        }
    }
}       
