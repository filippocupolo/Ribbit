using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto_2._0
{
    public class Flag
    {
        private Boolean flag;
        private Flag() {}
        public Flag(Boolean flag) {
            this.flag = flag;
        }
        public void setTrue() {
            flag = true;
        }
        public void setFalse()
        {
            flag = false;
        }
        public Boolean value()
        {
            return flag;
        }

    }
}
