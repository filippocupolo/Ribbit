using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto_2._0
{
    public class Options
    {
        private string destPath;
        private string name;
        private bool ricMode;
        private bool privateMode;
        private Object locker = new Object();

        public Options()
        {
            try
            {
                //get settings from file
                if (Properties.Settings.Default.DestPath == "NULL")
                {
                    Properties.Settings.Default.DestPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
            }catch(Exception ex)
            {
                //ArgumentException
                //PlatformNotSupportedException
            }
            
            this.name = Properties.Settings.Default.Name;
            this.destPath = Properties.Settings.Default.DestPath;
            this.privateMode = Properties.Settings.Default.PrivateMode;
            this.ricMode = Properties.Settings.Default.RicMode;
        }

        public string DestPath
        {
            get
            {
                lock (locker) { 
                    return this.destPath;
                }
            }

            set
            {
                lock (locker)
                {
                    Properties.Settings.Default.DestPath = value;
                    Properties.Settings.Default.Save();
                    this.destPath = value;
                }
            }
        }

        public string Name
        {
            get
            {
                lock (locker)
                {
                    return this.name;
                }
            }

            set
            {
                lock (locker)
                {
                    Properties.Settings.Default.Name = value;
                    Properties.Settings.Default.Save();

                    this.name = value;
                }
            }
        }

        public bool RicMode
        {
            get
            {
                lock (locker)
                {
                    return this.ricMode;
                }
            }

            set
            {
                    lock (locker)
                    {
                        Properties.Settings.Default.RicMode = value;
                        Properties.Settings.Default.Save();
                        this.ricMode = value;
                    }
                }
        }

        public bool PrivateMode
        {
            get
            {
                lock (locker)
                {
                    return this.privateMode;
                }
            }

            set
            {
                    lock (locker)
                    {
                        Properties.Settings.Default.PrivateMode = value;
                        Properties.Settings.Default.Save();
                        this.privateMode = value;
                    }
            }
        }
    }
}
