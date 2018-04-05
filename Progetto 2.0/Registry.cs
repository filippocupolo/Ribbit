using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto_2._0
{
    public class KeyReg
    {
        private RegistryKey startkeyFile;
        private RegistryKey startkeyFolder;
        private string Path;
       

        public KeyReg()
        {
            try { 
            startkeyFolder = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Directory\\shell\\Ribbit\\command",true);
            startkeyFile = Registry.CurrentUser.OpenSubKey("Software\\Classes\\*\\shell\\Ribbit\\command",true);
            Path = System.Reflection.Assembly.GetEntryAssembly().Location;
            Path = Path + " \"%1\"";
            if (startkeyFile == null || startkeyFolder == null)
            {
                if (startkeyFolder == null)
                {
                    RegistryKey keyFolder = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Directory\\shell\\Ribbit\\command");
                    keyFolder.SetValue("", Path);
                }

                if (startkeyFile == null)
                {
                    RegistryKey keyFile = Registry.CurrentUser.CreateSubKey("Software\\Classes\\*\\shell\\Ribbit\\command");
                    keyFile.SetValue("", Path);
                }
            }
            else
            {
                string checkPathFile=(string)startkeyFile.GetValue("");
                string checkPathFolder=(string)startkeyFile.GetValue("");
                if (Path != checkPathFile) {
                    startkeyFile.SetValue("",Path);

                }
                if (Path != checkPathFolder) {
                    startkeyFolder.SetValue("",Path);

                }
            }
            }
            catch (Exception ex) {
                //ArgumentNullException
                //ArgumentException
                //ObjectDisposedException
                //UnauthorizedAccessException
                //SecurityException
                //IOException
                //NotSupportedException

            }
        }
            
}
}
