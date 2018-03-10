using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Progetto_2._0
{
    public partial class Form2 : Form
    {
        private bool cancel;
        private object cancelLocker;
        public delegate void setPercentageDelegate(int i);
        public delegate void setTimeDelegate(string s);
        public setPercentageDelegate percentageDelegate;
        public setTimeDelegate timeDelegate;

        public Form2()
        {
            InitializeComponent();
            this.Cancel = false;

            percentageDelegate = new setPercentageDelegate(this.Setpercentage);
            timeDelegate = new setTimeDelegate(SetTime);

            //text in the title bar
            //text of the filna Name
            //Download Progression

        }

        private void Setpercentage(int i)
        {
            this.progressBar1.Value = i;
        }

        private void SetTime(string s)
        {
            //set label to s
        }

        private void Annulla_Click(object sender, EventArgs e)
        {
            Console.WriteLine("bottone premuto");
            Cancel = true;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public bool Cancel
        {
            get
            {
                lock (cancelLocker)
                {
                    return cancel;
                }
            }
            set
            {
                lock (cancelLocker)
                {
                    cancel = value;
                }
            }
        }
    }
}
