using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace Progetto_2._0
{
    class Utilities
    {

        //time to erase a element from the list
        static public int time_to_delete = 5;

        //time to resend udp information
        static public int time_to_send = 2;

        //multicast end point value
        static public IPEndPoint multicastEndPoint = new IPEndPoint(IPAddress.Parse("239.168.100.2"), 64537);

        static public void SetPercentage(ref int percentage, long fileSize, long sentTotalByte)
        {
            //set percentage
            percentage = (int)Math.Round((double)(100 * sentTotalByte) / fileSize);
        }

        static public void SetRemainingTime(ref string remainingTime, TimeSpan time, long fileSize, long sentTotalByte) {
           
            //set remaining seconds
            long remainingBytes = fileSize - sentTotalByte;
            long tmp = (int)time.TotalSeconds * remainingBytes;
            int seconds = (int)(tmp / sentTotalByte);

            if (seconds > 60)
            {
                if (seconds < 120)
                {
                    remainingTime = "1 minute";
                }
                else
                {
                    int minutes = (int)seconds / 60;
                    remainingTime = minutes.ToString() + " minutes";
                }
            }
            else
            {
                remainingTime = seconds.ToString() + " seconds";
            }
        }

        public static bool FindPort(ref int port)
        {
            bool found = false;
            for (port = 2000; port <= IPEndPoint.MaxPort; port++)
            {
                if (CheckAvailableServerPort(port))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
        private static bool CheckAvailableServerPort(int port)
        {
            try
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
                IPEndPoint[] udpConnInfoArray = ipGlobalProperties.GetActiveUdpListeners();

                foreach (IPEndPoint endpoint in tcpConnInfoArray)
                {
                    if (endpoint.Port == port)
                    {
                        return false;
                    }
                }

                foreach (IPEndPoint endpoint in udpConnInfoArray)
                {
                    if (endpoint.Port == port)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
