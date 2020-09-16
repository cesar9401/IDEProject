using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEProject
{
    class Token
    {
        public String type { get; set; }
        public String cadena { get; set; }

        public Token(String type, String cadena)
        {
            this.type = type;
            this.cadena = cadena;
        }
    }
}
