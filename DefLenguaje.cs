using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEProject
{
    class DefLenguaje
    {
        public Boolean isNumber(char c) 
        {
            int code = (int)c;
            return code >= 48 && code <= 57;
        }

        public Boolean isGeneral(char c)
        {
            int code = (int)c;
            return code >= 32 && code <= 126;
        }
    }
}
