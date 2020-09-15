using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            else if (IsLetter())
            {
                return "L";
            } 
            else if (isDoubleQuotes())
            {
                return "C";
            }
            else if(IsAsterisk())
            {
                return "A";
            }
            else if (isDiagonal())
            {
                return "D";
            }
            else if (IsPoint()){
                return "P";
            }
            else if (IsSymbol())
            {
                return "S";
            }
            else if (isUnderscore())
            {
                return "U";
            }
           
            return "E";
        }

        public Boolean IsNumber() 
        {
            return code >= 48 && code <= 57;
        }

        public Boolean IsLetter()
        {
            return code>=65 && code <=90 || code >= 97 && code <= 122 || code == 209 || code ==241;
        }

        public Boolean IsSymbol()
        {
            return (code>=32 && code <=126 && !IsNumber() && !IsLetter()) && !IsPoint() && !IsAsterisk() && !isDiagonal() && !isDoubleQuotes() && !isUnderscore() || code==191;
        }

        public Boolean IsPoint()
        {
            return code == 46;
        }

        public Boolean IsAsterisk()
        {
            return code == 42;
        }

        public Boolean isDiagonal()
        {
            return code == 47;
        }

        public Boolean isDoubleQuotes()
        {
            return code == 34;
        }

        public Boolean isUnderscore()
        {
            return code == 95;
        }

        public Boolean isFinalLine()
        {
            return code == 10;
        }

        public Boolean isEnterKey()
        {
            return code == 13;
        }
    }
}
