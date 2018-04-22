using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace Progetto_2._0
{
    class ServerUDP
    {
        private UdpClient udpServer;
        private SettingsForm settingsForm;
        private volatile bool closeServerUDP;
        private List<User> userList;
        private ConcurrentQueue<Tuple<byte[], IPEndPoint>> queue;
        private AutoResetEvent newData;
        private ManualResetEvent closeThread;
        private ConcurrentDictionary<SharingForm,String> sharingFormList;

        public ServerUDP(List<User> userList, ConcurrentDictionary<SharingForm, String> sharingFormList, SettingsForm settingsForm) {

            this.settingsForm = settingsForm;
            this.closeServerUDP = false;
            this.sharingFormList = sharingFormList;
            this.userList = userList;
            this.queue = new ConcurrentQueue<Tuple<byte[], IPEndPoint>>();
            this.newData = new AutoResetEvent(false);
            this.closeThread = new ManualResetEvent(false);
        }
        public void Execute() {

            Thread listManager = null;

            try
            {
                //join muticast and listen at 64537 port
                udpServer = new UdpClient(Utilities.multicastEndPoint.Port);
                udpServer.JoinMulticastGroup(Utilities.multicastEndPoint.Address);

                //create thread to manage userlist
                listManager = new Thread(this.ListManager);
                listManager.Start();

                //wait for data or for closeServerUDP
                while (true)
                {
                    while (udpServer.Available==0 && !closeServerUDP)
                    {
                        //wait for 0,1 seconds
                        Thread.Sleep(100);
                    }
                    if (!closeServerUDP && udpServer.Available > 0)
                    {
                        //Datagram received and put it into temp list
                        IPEndPoint RemoteIpEndPoint = new IPEndPoint(0, 0);
                        byte[] data = udpServer.Receive(ref RemoteIpEndPoint);
                        queue.Enqueue(new Tuple<byte[], IPEndPoint>(data, RemoteIpEndPoint));
                        newData.Set();
                    }
                    else if(closeServerUDP)
                    {
                        //if closeServerUDP is true exit
                        break;
                    }
                }
                
                //leave multicast group and close udpclient (finally)
                udpServer.DropMulticastGroup(Utilities.multicastEndPoint.Address);
                udpServer.Close();

                //close ListSetter
                listManager.Join();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                //close socket
                if (udpServer != null && udpServer.Client != null)
                {
                    udpServer.Close();
                }

                //close listManager
                CloseThread();
                if (listManager != null) { listManager.Join(); }

                //say to settingsform that you closed
                settingsForm.BeginInvoke(settingsForm.CloseThreadDelegate, new object[] { Thread.CurrentThread, Utilities.ServerUDP });
            }
        }
        public void CloseThread()
        {
            closeServerUDP = true;
            closeThread.Set();
        }
        private void ScanUserList(User u = null)
        {
            bool exist = false;
            for (int i = 0; i < userList.Count; i++)
            {
                if (u != null)
                {
                    //check if user already exist and reset time
                    if (userList[i].IP.Address.Equals(u.IP.Address))
                    {
                        userList[i].SetTime();

                        //if the name is changed change it
                        if (!userList[i].Name.Equals(u.Name))
                        {
                            userList[i].Name = u.Name;
                            if (sharingFormList.Any())
                            {
                                foreach (KeyValuePair<SharingForm, String> sf in sharingFormList)
                                {
                                    sf.Key.BeginInvoke(sf.Key.changeItemDelegate, new object[] { u });
                                }
                            }
                        }

                        //if the port is changed change it
                        if (!userList[i].IP.Port.Equals(u.IP.Port))
                        {
                            userList[i].IP.Port = u.IP.Port;
                            if (sharingFormList.Any())
                            {
                                foreach (KeyValuePair<SharingForm, String> sf in sharingFormList)
                                {
                                    sf.Key.BeginInvoke(sf.Key.changeItemDelegate, new object[] { u });
                                }
                            }
                        }

                        exist = true;
                    }
                }

                //delete element exipired
                TimeSpan span = DateTime.Now.Subtract(userList[i].GetTime());
                if (span.TotalSeconds > 5)
                {
                    if (sharingFormList.Any())
                    {
                        foreach (KeyValuePair<SharingForm, String> sf in sharingFormList)
                        {
                            sf.Key.BeginInvoke(sf.Key.removeItemDelegate, new object[] { userList[i] });
                        }
                    }

                    userList.RemoveAt(i);
                }
            }

            //add element if it is new
            if (exist == false && u != null)
            {
                userList.Add(u);
                if (sharingFormList.Any())
                {
                    foreach (KeyValuePair<SharingForm, String> sf in sharingFormList)
                    {
                        sf.Key.BeginInvoke(sf.Key.addItemDelegate, new object[] { u });
                    }
                }
            }
        }

        private void GetElement() {

            while (queue.Any())
            {
                //get first element
                Tuple<byte[], IPEndPoint> tuple;
                if (!queue.TryDequeue(out tuple)) break;

                //get data from first element
                byte[] data = tuple.Item1;
                IPAddress address = tuple.Item2.Address;
                //Console.WriteLine("ServerUDP.GetElement: received;" + address);

                try
                {
                    //check if the adress is mine
                    if (!address.Equals(Dns.GetHostAddresses(Dns.GetHostName()).Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray()[0]))
                    {
                        //get payloadsize and check if corrispond to receivedbyte
                        int payloadSize = BitConverter.ToInt32(data, 0);
                        if (data.Length == payloadSize)
                        {
                            //create the new user
                            int port = BitConverter.ToInt32(data, 4);
                            byte[] byteName = new byte[payloadSize - 8];
                            Buffer.BlockCopy(data, 8, byteName, 0, payloadSize - 8);
                            String name = Encoding.UTF8.GetString(byteName);
                            User u = new User(name, new IPEndPoint(address, port));

                            //scan the userlist
                            ScanUserList(u);
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }
        private void ListManager() {

            WaitHandle[] waithandles = new WaitHandle[] { this.closeThread, this.newData };
            while (closeServerUDP == false)
            {

                if (!userList.Any())
                {
                    //wait for multiple object (newData and closeThread)
                    WaitHandle.WaitAny(waithandles);

                    //get the new element from temp list and put in userlist
                    GetElement();
                }
                else
                {
                    //wait for new data or timeout (need to scan the list every 100 ms)
                    if (!newData.WaitOne(100))
                    {
                        //scan the userlist
                        ScanUserList();
                    }
                    else
                    {
                        //get the new element from temp list and put in userlist
                        GetElement();
                    }
                }
            }
        }
    }
}

