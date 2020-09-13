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

        public Boolean verificarCadena()
        {
            DefLenguaje leng = new DefLenguaje();
            int q = 0;
            MessageBox.Show("length: " + cadena.Length);
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
                    return false;
                }
                MessageBox.Show("Estado: " + q);
                if(q == -1)
                {
                    break;
                }
            }

            return estadosA.Contains(q);
        }

        public void setAutomata()
        {
            int estados = 2;
            List<int> acep = new List<int>();
            acep.Add(1);
            List<String> alf = new List<String>();
            alf.Add("L");
            this.buildAutomata(estados, acep, alf);
            this.setTransiciones(0, 0, 1);
            this.setTransiciones(1, 0, 1);

            /*
            int estados = 3;
            List<int> aceptacion = new List<int>();
            aceptacion.Add(2);
            List<String> alfabeto = new List<String>();
            alfabeto.Add("0");
            alfabeto.Add("1");
            Automata afd = new Automata(estados, aceptacion, alfabeto);
            this.setTransiciones(0, 0, 1);
            this.setTransiciones(0, 1, 2);
            this.setTransiciones(1, 0, 1);
            this.setTransiciones(1, 1, 2);
            this.setTransiciones(2, 0, 1);
            this.setTransiciones(2, 1, 2);
            */
        }
    }
}
