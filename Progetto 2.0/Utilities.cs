using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

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
            long tmp = time.Seconds * remainingBytes;
            int seconds = (int)(tmp / sentTotalByte);
            if (seconds > 60)
            {
                if (seconds < 120)
                {
                    remainingTime = "un minuto";
                }
                else
                {
                    int minutes = (int)seconds / 60;
                    remainingTime = minutes.ToString() + "minuti";
                }
            }
            else
            {
                remainingTime = seconds.ToString() + "secondi";
            }
        }
    }
}
