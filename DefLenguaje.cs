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
                return "N";
            if (IsLetter())
                return "L";
            if (isDoubleQuotes())
                return "C";
            if(IsAsterisk())
                return "A";
            if (isDiagonal())
                return "D";
            if (IsPoint())
                return "P";
            if (IsSymbol())
                return "S";
            if (isUnderscore())
                return "U";
            if (IsSpace())
                return "E";
            if (isEnterKey())
                return "R";
            if (isFinalLine())
                return "F";
           
            return "Ex";
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
            return (code>32 && code <=126 && !IsNumber() && !IsLetter()) && !IsPoint() && !IsAsterisk() && !isDiagonal() && !isDoubleQuotes() && !isUnderscore() || code == 191;
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

        public Boolean IsSpace()
        {
            return code == 32;
        }
    }
}
