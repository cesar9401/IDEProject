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
        //Alfabeto
        private List<String> alfabeto = new List<String>();

        public Automata(int numeroEstados, List<int> estadosA, List<String> alfabeto)
        {
            this.estadoInicial = 0;
            this.numeroEstados = numeroEstados;
            this.estadosA = estadosA;
            this.alfabeto = alfabeto;
            this.setEstados();
        }

        private void setEstados()
        {
            for (int i = 0; i < numeroEstados; i++) 
            {
                estados.Add(i);
            }
        }

        private void setEstadosAceptacion(int q)
        {
            if(estados.Contains(q))
            {
                estadosA.Add(q);
            }
        }

        public void setTransiciones(int x, int y, int q)
        {
            if(estados.Contains(q))
            {
                transiciones[x].Insert(y, q);
            }
        }
        
        private int getEstado(int x, int y) 
        {
            return transiciones[x][y];
        }

        public Boolean verificarCadena(String cadena)
        {
            int q = 0;
            for(int i=0; i<cadena.Length; i++)
            {
                int index = alfabeto.IndexOf(cadena.Substring(i));
                q = getEstado(q, index);
                if(q == -1)
                {
                    break;
                }
            }

            return estadosA.Contains(q);
        }
    }
}
