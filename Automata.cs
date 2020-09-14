using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IDEProject
{
    class Automata
    {
        public String cadena { get; set; }
        private int estadoInicial { get; set; }
        private int numeroEstados { get; set; }
        //estados
        private List<int> estados = new List<int>();
        //estados de aceptacion
        private List<int> estadosA = new List<int>();
        //Transiciones
        private List<List<int>> transiciones = new List<List<int>>();
        private int[,] trans;
        //Alfabeto
        private List<String> alfabeto = new List<String>();
        //Palabras reservadas
        private List<String> reservadas;

        public Automata()
        {
            setAutomata();
        }

        public void buildAutomata(int numeroEstados, List<int> estadosA, List<String> alfabeto)
        {
            this.estadoInicial = 0;
            this.numeroEstados = numeroEstados;
            this.estadosA = estadosA;
            this.alfabeto = alfabeto;
            this.setEstados();
            this.trans = new int[this.estados.Count, this.alfabeto.Count];
        }

        private void setReservadas()
        {
            reservadas = new List<String>()
            {
                "entero",
                "decimal",
                "cadena",
                "booleano",
                "caracter",
                "SI",
                "SINO",
                "SINO_SI",
                "MIENTRAS",
                "HACER",
                "DESDE",
                "HASTA",
                "INCREMENTO"
            };
        }

        private void setEstados()
        {
            for (int i = 0; i < numeroEstados; i++) 
            {
                estados.Add(i);
            }
        }

        public void setTransiciones(int x, int y, int q)
        {
            if(estados.Contains(q))
            {
                //MessageBox.Show("valor q: " + q);
                //transiciones[x].Insert(y, q);
                trans[x, y] = q;
            }
        }
        
        private int getEstado(int x, int y) 
        {
            //return transiciones[x][y];
            return trans[x, y];
        }

        public String verificarCadena()
        {
            DefLenguaje leng = new DefLenguaje();
            int q = 0;
            for(int i=0; i<cadena.Length-2; i++)
            {
                int code = (int)Convert.ToChar(cadena.Substring(i, 1));
                String whatIs = leng.WhatIs(code);
                int index = alfabeto.IndexOf(whatIs);
                try
                {
                    q = trans[q, index];
                }
                catch (Exception)
                {
                    MessageBox.Show("Error: " + whatIs);
                    return "SIGMA";
                }

                if(q == -1)
                {
                    break;
                }
            }

            if(q == -1)
            {
                return "ERROR";
            }

            if (estadosA.Contains(q))
            {
                switch (q)
                {
                    case 1:
                        return "PALABRA";
                    case 2:
                        return "NUMERO";
                    case 4:
                        return "DECIMAL";
                }
            }

            return "NO VALIDO";
        }

        public void setAutomata()
        {
            int estados = 5;
            List<int> acep = new List<int>();
            acep.Add(1);
            acep.Add(2);
            acep.Add(4);
            List<String> alf = new List<String>();
            alf.Add("L");
            alf.Add("N");
            alf.Add(".");
            this.buildAutomata(estados, acep, alf);
            setEmpties();
            setTransiciones(0, 0, 1);
            setTransiciones(0, 1, 2);
            setTransiciones(1, 0, 1);
            setTransiciones(2, 1, 2);
            setTransiciones(2, 2, 3);
            setTransiciones(3, 1, 4);
            setTransiciones(4, 1, 4);
        }

        private void setEmpties()
        {
            for(int i=0; i<trans.GetLength(0); i++)
            {
                for(int j=0; j<trans.GetLength(1); j++)
                {
                    trans[i, j] = -1;
                }
            }
        }
    }
}
