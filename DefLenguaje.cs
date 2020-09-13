using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEProject
{
    class DefLenguaje
    {
       
        private int code { get; set; }

        public String WhatIs(int code)
        {
            this.code = code;
            if (IsNumber())
            {
                return "N";
            }
            else if (IsLowerLetter())
            {
                return "l";
            }
            else if (IsUpperLetter())
            {
                return "L";
            }else if (IsSpace())
            {
                return "S";
            }

            return "E";
        }

        public Boolean IsNumber() 
        {
            return code >= 48 && code <= 57;
        }

        public Boolean IsLowerLetter()
        {
            return code >= 97 && code <= 122;
        }

        public Boolean IsUpperLetter()
        {
            return code >= 65 && code <= 90;
        }

        public Boolean IsSpace()
        {
            return code == 32;
        }
    }
}
