using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

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
        private SettingsForm settingsForm;
        Boolean finalClose = false;
        private ServerTCP() { }

        public ServerTCP(int portTCP, string pathDest, bool automaticAnswer, SettingsForm settingsForm)
        {
            this.portTCP = portTCP;
            this.pathDest = pathDest;
            this.automaticAnswer = automaticAnswer;
            this.closeServerTCP = false;
            this.settingsForm = settingsForm;
        }
        public void Execute() {

            try
            {
                //listen local IP:portTCP
                serverTCP = new TcpListener(Dns.GetHostAddresses(Dns.GetHostName()).Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray()[0], PortTCP);
                serverTCP.Start();
                
                //create a list of Threads
                List<Thread> threadList = new List<Thread>();

                //wait for connection or for closeServerTCP
                while (true)
                {
                    while (!serverTCP.Pending() && !closeServerTCP) {
                        
                        //check if there are finished thread and remove them
                        for (int i = 0; i < threadList.Count; i++)
                        {
                            if (threadList[i].Join(0))
                            {
                                threadList.RemoveAt(i);
                                i--;
                            }
                        }

                        //wait for 0,1 seconds
                        Thread.Sleep(100);
                    }

                    if (!closeServerTCP && serverTCP.Pending())
                    {
                        //received connection request
                        TcpClient connectedSocket = serverTCP.AcceptTcpClient();

                        //create thread to receive file
                        Receiver r = new Receiver(connectedSocket, AutomaticAnswer, PathDest, settingsForm);
                        Thread t = new Thread(r.Execute);
                        t.Start();
                        threadList.Add(t);
                    }
                    else if (closeServerTCP)
                    {
                        //if closeServerTCP is true exit
                        break;
                    }
                }
                
                //stop listening for new client (finally)
                serverTCP.Stop();
                //wait all threads
                threadList.ForEach(x => { x.Join(); });
                
                //say to form that you finished (finally)
                if (!finalClose) {
                    settingsForm.BeginInvoke(settingsForm.CloseThreadDelegate, new object[] { Thread.CurrentThread, Utilities.ServerTCP });
                }
                
            }
            catch(Exception e)
            {
                //InvalidOperationException
                //ArgumentNullException
                //ThreadStateException
                //ThreadInterruptedException
                //SocketException
                //ArgumentOutOfRangeException
                //OutOfMemoryException
                //AggregateException
                //ObjectDisposedException
               
            }
        }

        public void CloseThread (Boolean finalclose){
            closeServerTCP = true;
            this.finalClose = finalclose;
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
