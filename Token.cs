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
        public int row { get; set; }
        public int col { get; set; }

        public Token(String type, String cadena, int row, int col)
        {
            this.type = type;
            this.cadena = cadena;
            this.row = row;
            this.col = col;
        }
    }
}
