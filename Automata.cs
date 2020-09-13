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

        public Automata(int numeroEstados, List<int> estadosA, List<String> alfabeto)
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

        public Boolean verificarCadena(String text)
        {
            int q = 0;
            String cadena = text;
            MessageBox.Show(cadena.Length + " length");
            for(int i=0; i<text.Length-2; i++)
            {
                int index = alfabeto.IndexOf(cadena.Substring(i, 1));
                q = trans[q, index];
                MessageBox.Show("q: " + q);
                if(q == -1)
                {
                    break;
                }
            }

            return estadosA.Contains(q);
        }
    }
}
