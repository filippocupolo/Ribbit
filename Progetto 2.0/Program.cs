using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Text;

namespace Progetto_2._0
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        private static SettingsForm settingsForm;
        private static Mutex mutex = new Mutex(true, "Filippo&Marco.Ribbit");

        [STAThread]
        static void Main(String[] args)
        {
            

           
            try
            {
             

                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.EnableVisualStyles();

                    Options options = new Options();
                    KeyReg keyReg = new KeyReg();

                    //check if user is already logged
                    if (options.Name.Equals("DefaultName"))
                    {
                        FirstRegistrationForm registrationForm = new FirstRegistrationForm(options);
                        Task task = new Task(() => { registrationForm.ShowDialog(); });
                        task.RunSynchronously();
                    }

                    if (args.Length > 0)
                    {
                        //create SettingsForm passing args
                        settingsForm = new SettingsForm(args[0]);
                    }
                    else
                    {
                        settingsForm = new SettingsForm(null);
                    }
                    Application.Run(settingsForm);
                    
                    mutex.ReleaseMutex();
                }
                else
                {
                    if (args.Length > 0)
                    {
                        NamedPipeClientStream pipe = new NamedPipeClientStream(".", "RibbitPipe", PipeAccessRights.FullControl,
                            PipeOptions.WriteThrough, System.Security.Principal.TokenImpersonationLevel.None, System.IO.HandleInheritability.None);
                        pipe.Connect(3000);
                        byte[] bytes = Encoding.UTF8.GetBytes(args[0]);
                        pipe.Write(bytes, 0, bytes.Length);
                        pipe.Flush();
                        pipe.Close();
                    }
                    else
                    {
                        //program already open
                        MessageBox.Show("Program is already running!!");
                    }

                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                //ObjectDisposedException
                //InvalidOperationException
                //NotSupportedException
                //System.IO.Exception
                //ArgumentOutOfRangeExpeption
                //ArgumentNullException
                //ArgumentException
                //EncoderFallBackException
                //TimeOutException
                //OverflowException
                //ApplicationException
                //AbandonedMutexException
                //UnhautorizedAccessException
                //WaitHAndleCannotBeOpenedException
            }
          
        }
    
    }
   
}
