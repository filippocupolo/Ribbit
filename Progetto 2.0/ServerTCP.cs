﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Progetto_2._0
{
    class ServerTCP
    {
        private TcpListener serverTCP;
        private int portTCP;
        private string pathDest;
        private bool automaticAnswer;
        private object portTCPlocker = new object();
        private object automaticAnswerlocker = new object();
        private volatile bool closeServerTCP;
        private Form1 form;
        private ServerTCP() { }

        public ServerTCP(int portTCP, string pathDest, bool automaticAnswer, Form1 form)
        {
            this.portTCP = portTCP;
            this.pathDest = pathDest;
            this.automaticAnswer = automaticAnswer;
            this.closeServerTCP = false;
            this.form = form;
        }
        public void Execute() {

            try
            {
                //listen local IP:portTCP
                serverTCP = new TcpListener(Dns.GetHostAddresses(Dns.GetHostName()).Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray()[0], PortTCP);
                serverTCP.Start();
                
                //create a list of Threads
                List<Thread> threadList = new List<Thread>();

                //wait for connections
                Task<TcpClient> task = serverTCP.AcceptTcpClientAsync();
                
                while (closeServerTCP == false)
                {   
                    if (task.Wait(10000)) {

                        //received connection request
                        TcpClient connectedSocket = task.Result;

                        //create thread to receive file
                        Receiver r = new Receiver(connectedSocket, AutomaticAnswer, PathDest);
                        Thread t = new Thread(r.Execute);
                        t.Start();

                        //check if there are finished thread and remove them
                        for (int i = 0; i < threadList.Count; i++)
                        {
                            if (threadList[i].Join(0))
                            {
                                threadList.RemoveAt(i);
                                i--;
                            }
                        }

                        threadList.Add(t);
                    }

                    //if task is completed create another
                    if (task.IsCompleted)
                    {
                        task = serverTCP.AcceptTcpClientAsync();
                    }
                }

                //stop listening for new client (finally)
                serverTCP.Stop();

                //wait all threads
                threadList.ForEach(x => { x.Join();});

                //say to form that you finished (finally)
                form.BeginInvoke(form.CloseThreadDelegate, new object[] { Thread.CurrentThread });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CloseThread (){
            closeServerTCP = true;
        }

        public int PortTCP
        {
            get
            {
                lock (this.portTCPlocker)
                {
                    return this.portTCP;
                }
            }
            set
            {
                lock (this.portTCPlocker)
                {
                    this.portTCP = value;
                }
            }
        }
        public string PathDest
        {
            get
            {
                lock (this.pathDest)
                {
                    return this.pathDest;
                }
            }
            set
            {
                lock (this.pathDest)
                {
                    this.pathDest = value;
                }
            }
        }
        public bool AutomaticAnswer
        {
            get
            {
                lock (this.automaticAnswerlocker)
                {
                    return this.automaticAnswer;
                }
            }
            set
            {
                lock (this.automaticAnswerlocker)
                {
                    this.automaticAnswer = value;
                }
            }
        }
    }
}