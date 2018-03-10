using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Progetto_2._0
{
    class ClientUDP
    {
        private UdpClient udpClient;
        private volatile bool closeClientUDP;
        private string name;
        private int portTCP;
        private Form1 form;
        private object portTCPlocker = new object();
        private ClientUDP()
        {
        }
        public ClientUDP(string name, int portTCP, Form1 form)
        {
            this.name = name;
            this.portTCP = portTCP;
            this.form = form;
            this.closeClientUDP = false;
        }
        public void Execute() {
            try
            { 
                //initialize UdpClient
                udpClient = new UdpClient();

                //set end point to multicast 239.168.100.2:64537
                IPEndPoint endPointUDP = new IPEndPoint(Utilities.multicastEndPoint.Address, Utilities.multicastEndPoint.Port);

                while (closeClientUDP == false) //must use mutex 
                {
                    //create payload: 4 byte lenght payload 4 byte port n byte name
                    int nameSize = Name.Length;
                    int payloadSize = nameSize + 8;
                    byte[] payload = new byte[payloadSize];
                    Buffer.BlockCopy(BitConverter.GetBytes(payloadSize),0, payload, 0, 4); //big/little endian               
                    Buffer.BlockCopy(BitConverter.GetBytes(PortTCP), 0, payload, 4, 4);
                    Buffer.BlockCopy(Encoding.UTF8.GetBytes(Name), 0, payload, 8, nameSize);
              
                    //send messagge
                    udpClient.Send(payload, payloadSize, endPointUDP);

                    //sleep for 2 seconds
                    Thread.Sleep(2000);
                }

                //close udpclient (finally)
                udpClient.Close();

                //say to form that you finished (finally)
                form.BeginInvoke(form.CloseThreadDelegate, new object[] { Thread.CurrentThread });
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CloseThread()
        {
            closeClientUDP = true;
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