using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto_2._0
{
    class RibbitException : Exception
    {
        public RibbitException(String param) {
            Parameter = param;
        }

        public String Parameter{
            get;
            set;
        }
    }
}
