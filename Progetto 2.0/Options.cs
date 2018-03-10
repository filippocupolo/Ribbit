using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto_2._0
{
    class Options
    {
        private string destPath;
        private string name;
        private bool ricMode;
        private bool privateMode;

        public Options()
        {
            //get settings from file
            if (Properties.Settings.Default.DestPath == "NULL")
            {
                Properties.Settings.Default.DestPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
                return this.destPath;
            }

            set
            {
                Properties.Settings.Default.DestPath = value;
                this.destPath = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                Properties.Settings.Default.Name = value;
                this.name = value;
            }
        }

        public bool RicMode
        {
            get
            {
                return this.ricMode;
            }

            set
            {
                Properties.Settings.Default.RicMode = value;
                this.ricMode = value;
            }
        }

        public bool PrivateMode
        {
            get
            {
                return this.privateMode;
            }

            set
            {
                Properties.Settings.Default.PrivateMode = value;
                this.privateMode = value;
            }
        }
    }
}
