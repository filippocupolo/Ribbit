using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Progetto_2._0
{
    public class User
    {
        private User()
        {
        }
        private DateTime time;
        public string Name { set; get;}
        public IPEndPoint IP { set; get;}
        public void SetTime()
        {
            time = DateTime.Now;
        }
        public DateTime GetTime()
        {
            return time;
        }
        public User(string name, IPEndPoint ip) {
            Name = name;
            IP = ip;
            time = DateTime.Now;
        }
    }
}
