using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Progetto_2._0
{
    class ServerUDP
    {
        private UdpClient udpServer;
        private volatile bool closeServerUDP;
        private List<User> userList;
        private ConcurrentQueue<Tuple<byte[], IPEndPoint>> queue;
        private AutoResetEvent newData;
        private ManualResetEvent closeThread;
        private Form1 form;
        public ServerUDP(Form1 form) {
            this.form = form;
            this.closeServerUDP = false;
            this.userList = new List<User>();
            this.queue = new ConcurrentQueue<Tuple<byte[], IPEndPoint>>();
            this.newData = new AutoResetEvent(false);
            this.closeThread = new ManualResetEvent(false);
        }
        public void Execute() {
            try
            {
                //join muticast and listen at 64537 port
                udpServer = new UdpClient(Utilities.multicastEndPoint.Port);
                udpServer.JoinMulticastGroup(Utilities.multicastEndPoint.Address);

                //create thread to manage userlist
                Thread listManager = new Thread(this.ListManager);
                listManager.Start();

                //receive datagram
                Task<UdpReceiveResult> task = udpServer.ReceiveAsync();

                //receive data
                while (closeServerUDP == false)
                {
                    //wait to receive new byte or closeServerUDP
                    if (task.Wait(10000)) {

                        //Datagram received and put it into temp list
                        byte[] data = task.Result.Buffer;
                        IPEndPoint RemoteIpEndPoint = task.Result.RemoteEndPoint;
                        
                        queue.Enqueue(new Tuple<byte[], IPEndPoint>(data, RemoteIpEndPoint));
                        newData.Set();
                    }

                    //if task is completed create another
                    if (task.IsCompleted)
                    {
                        task = udpServer.ReceiveAsync();
                    }
                }

                //leave multicast group and close udpclient (finally)
                udpServer.DropMulticastGroup(Utilities.multicastEndPoint.Address);
                udpServer.Close();

                //close ListSetter
                listManager.Join();
                form.BeginInvoke(form.CloseThreadDelegate, new object[] { Thread.CurrentThread });
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                if(u != null)
                {
                    //check if user already exist and reset time
                    if (userList[i].IP.Address.Equals(u.IP.Address))
                    {
                        userList[i].SetTime();

                        //if the name is changed change it
                        if (!userList[i].Name.Equals(u.Name))
                        {
                            userList[i].Name = u.Name;
                            form.Invoke(form.changeItemDelegate,new object[] { u });
                        }

                        //if the port is changed change it
                        if (!userList[i].IP.Port.Equals(u.IP.Port))
                        {
                            userList[i].IP.Port = u.IP.Port;
                            form.Invoke(form.changeItemDelegate, new object[] { u });
                        }

                        exist = true;
                    }
                }

                //delete element exipired
                TimeSpan span = DateTime.Now.Subtract(userList[i].GetTime());
                if (span.Seconds > 5)
                {
                    form.Invoke(form.removeItemDelegate,new object []{ userList[i] });
                    userList.RemoveAt(i);
                }
            }

            //add element if it is new
            if (exist == false && u != null)
            {
                userList.Add(u);
                form.Invoke(form.addItemDelegate,new object[] { u });
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
        }
        private void ListManager() {
            try 
            {
                WaitHandle[] waithandles = new WaitHandle[] { this.closeThread, this.newData };
                while (closeServerUDP == false) {

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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

//questions: big/little endian, ipv6, mutex, try catch, receive data con un altro thread
//how to close the thread at the end of the program,
