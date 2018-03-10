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

namespace Progetto_2._0
{
    class ClientTCP
    {
        private TcpClient tcpClient;
        private string pathFile;
        private string userName;
        private IPEndPoint endPoint;
        private const int buffer = 4096;
        private Form1 form;
        private bool IsFolder;

        private byte[] CreateRequestMessage(ref int messageSize)
        {
            //get file info
            FileInfo fileInfo = new FileInfo(pathFile);

            //create request message: 1 byte isFolder, 8 byte dimension file, 4 byte dimension file name, 4 byte dimension user name, n bit file name, n bit user name
            int userNameSize = userName.Length;
            int fileNameSize = fileInfo.Name.Length;
            long fileSize = fileInfo.Length;
            messageSize = 17 + userNameSize + fileNameSize;
            byte[] request = new byte[messageSize];
            
            request[0] = (IsFolder) ? (byte)1 : (byte)0;
            Buffer.BlockCopy(BitConverter.GetBytes(fileSize), 0, request, 1, 8); //big/little endian 
            Buffer.BlockCopy(BitConverter.GetBytes(fileNameSize), 0, request, 9, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(userNameSize), 0, request, 13, 4);
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(fileInfo.Name), 0, request, 17, fileNameSize);
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(userName), 0, request, 17 + fileNameSize, userNameSize);
            
            return request;
        }
        private ClientTCP()
        {
        }
        public ClientTCP(string pathFile, bool IsFolder, string userName, IPEndPoint endPoint, Form1 form)
        {
            this.pathFile = pathFile;
            this.userName = userName;
            this.endPoint = endPoint;
            this.form = form;
            this.IsFolder = IsFolder;
        }
        public void Execute()
        {
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

                //say to form that you finished (finally)
                form.BeginInvoke(form.CloseThreadDelegate, new object[] { Thread.CurrentThread });
            }
            catch (SocketException se) {
                Console.WriteLine("se: " + se.ErrorCode);
            }
            catch (Exception e)
            {
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
                string remainingTime = "";

                //create progress bar


                //set the timer
                timer = new Stopwatch();
                timer.Start();
                int counter = -1;

                while (fileSize > totRead)
                {
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
                        //do something
                    }

                    //write to networkstream
                    stream.Write(data, 0, read); //write scrive tutti i byte di read sicuro??
                    totRead = totRead + read;

                    //set remaining time and pecentage
                    Utilities.SetPercentage(ref percentage, fileSize, totRead);
                    if (counter < timer.Elapsed.Seconds)
                    {
                        counter = timer.Elapsed.Seconds;
                        Utilities.SetRemainingTime(ref remainingTime, timer.Elapsed, fileSize, totRead);
                    }
                    Console.WriteLine(percentage + "% remaining " + remainingTime + " seconds");
                }
                
                timer.Stop();

                if (fileSize != totRead)
                {
                    //errore
                }

                //flush and close fileStream (finally)
                file.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //check if file exists 
            }
        }

    }
}
