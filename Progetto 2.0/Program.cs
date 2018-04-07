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
            //if the mutex is owned enter and launch the program
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                try
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
                }
                catch (Exception e)
                { Console.WriteLine(e.ToString()); }
                finally
                {
                    //release mutex
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                if (args.Length > 0)
                {
                    Boolean repeat = true;
                    int i = 0;
                    while (repeat)
                    {
                        NamedPipeClientStream pipe = new NamedPipeClientStream(".", "RibbitPipe", PipeAccessRights.FullControl,
                            PipeOptions.WriteThrough, System.Security.Principal.TokenImpersonationLevel.None, System.IO.HandleInheritability.None);
                        repeat = false;
                        i++;
                        try
                        {

                            pipe.Connect(3000);
                            byte[] bytes = Encoding.UTF8.GetBytes(args[0]);
                            pipe.Write(bytes, 0, bytes.Length);

                        }
                        catch (System.IO.IOException e)
                        {
                            if (i < 2) { repeat = true; }
                            Console.WriteLine(e.ToString());
                        }
                        catch (TimeoutException e)
                        {
                            if (i < 2) { repeat = true; }
                            Console.WriteLine(e.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                        finally
                        {
                            pipe.Flush();
                            pipe.Close();
                        }
                    }
                }
                else
                {
                    //program already open
                    MessageBox.Show("Program is already running!!");
                }               
            }
        }   
    }  
}
