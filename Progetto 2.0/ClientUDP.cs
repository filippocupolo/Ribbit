using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace Progetto_2._0
{
    class ClientUDP
    {
        private UdpClient udpClient;
        private volatile bool closeClientUDP;
        private string name;
        private int portTCP;
        private SettingsForm form;
        private object portTCPlocker = new object();
        Boolean finalClose = false;
        private ClientUDP()
        {
        }
        public ClientUDP(string name, int portTCP, SettingsForm form)
        {
            this.name = name;
            this.portTCP = portTCP;
            this.form = form;
            this.closeClientUDP = false;
        }
        public void Execute() {

            int c = 0;
            Boolean repeat = true;
            while (repeat)
            {
                repeat = false;
                c++;

                try
                {
                    //initialize UdpClient
                    udpClient = new UdpClient();
                   
                    //set end point to multicast 239.168.100.2:64537
                    IPEndPoint endPointUDP = new IPEndPoint(Utilities.multicastEndPoint.Address, Utilities.multicastEndPoint.Port);

                    while (closeClientUDP == false) //must use mutex 
                    {
                        //create payload: 4 byte lenght payload 4 byte port n byte name
                        byte[] nameBytes = Encoding.UTF8.GetBytes(Name);
                        int nameSize = nameBytes.Length;
                        int payloadSize = nameSize + 8;
                        byte[] payload = new byte[payloadSize];
                        Buffer.BlockCopy(BitConverter.GetBytes(payloadSize), 0, payload, 0, 4); //big/little endian               
                        Buffer.BlockCopy(BitConverter.GetBytes(PortTCP), 0, payload, 4, 4);
                        Buffer.BlockCopy(nameBytes, 0, payload, 8, nameSize);
                        
                        if (NetworkInterface.GetIsNetworkAvailable()) {
                            
                            //send messagge
                            udpClient.Send(payload, payloadSize, endPointUDP);
                        }

                        //sleep for 2 seconds
                        Thread.Sleep(2000);
                    }

                    //close udpclient
                    udpClient.Close();

                    //say to form that you finished 
                    if (!finalClose)
                    {
                        form.BeginInvoke(form.CloseThreadDelegate, new object[] { Thread.CurrentThread, Utilities.CientUDP });
                    }
                }
                catch (SocketException e)
                {
                    //Console.WriteLine(e.ErrorCode);
                    Console.WriteLine(e.ToString());
                    if (c < 2)
                    {
                        if (udpClient != null && udpClient.Client != null) {
                            udpClient.Close();
                        }
                        repeat = true;
                    }
                    else {
                        
                        //say to form that you finished
                        if (!finalClose)
                        {
                            form.BeginInvoke(form.CloseThreadDelegate, new object[] { Thread.CurrentThread, Utilities.CientUDP });
                        }

                        if (udpClient != null && udpClient.Client != null)
                        {
                            udpClient.Close();
                        }
                    }
                }
            }
        }

        public void CloseThread(Boolean finalClose)
        {
            closeClientUDP = true;
            this.finalClose = finalClose;
        }

        public string Name {

            get
            {
                lock (this.name)
                {
                    return this.name;
                }
            }

            set
            {
                lock (this.name)
                {
                    this.name = value;
                }
            }

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

    }
}

//questions: big/little endian, ipv6, mutex, try catch, ip change during loop, portUDP, 
//how to close the thread at the end of the program, close in finally