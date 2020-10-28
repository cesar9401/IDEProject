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
        public String str { get; set; }
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
        private List<String> operadores;
        //Tokens
        public List<Token> tokens = new List<Token>();
        private int index = 0;

        public Automata()
        {
            setAutomata();
            setReservadas();
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
                "entero", "decimal", "cadena", "booleano", "caracter", "principal", "imprimir", "leer",
                "SI",
                "SINO",
                "SINO_SI",
                "MIENTRAS",
                "HACER",
                "DESDE",
                "HASTA",
                "INCREMENTO"
            };

            operadores = new List<String>()
            {
                "+", "-", "*", "/", "++", "--", ">", "<", ">=", "<=", "==", "!=", "||", "&&", "!", "(", ")", "=", ";", "{", "}", ",",
                "+=", "-=", "*=", "/="
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
                trans[x, y] = q;
            }
        }
        
        private int getEstado(int x, int y) 
        {
            return trans[x, y];
        }

        public List<Token> verificarCadena()
        {
            String cadena = str;
            int row = 1;
            int col = 0;

            while(cadena.Length > 0)
            {
                String type = CheckSubString(cadena);
                String content = cadena.Substring(0, index);
                
                cadena = cadena.Substring(index);

                tokens.Add(new Token(type, content, row, col));
                
                col += content.Length;
                if (type.Equals("FIN"))
                {
                    row++;
                    col = 0;
                }
            }

            foreach (Token t in tokens)
            {
                Console.WriteLine("Tipo: " + t.type + ", cadena: " + t.cadena + ", row: " + t.row + ", col: " + t.col);
            }

            return tokens;
        }

        public String CheckSubString(String cadena)
        {
            DefLenguaje leng = new DefLenguaje();
            int q = 0;
            int q0 = 0;
            int i;
            for(i=0; i<cadena.Length; i++)
            {
                int code = (int)Convert.ToChar(cadena.Substring(i, 1));
                String whatIs = leng.WhatIs(code);
                int j = alfabeto.IndexOf(whatIs);
                try
                {
                    q0 = q;
                    q = trans[q, j];
                }
                catch (Exception)
                {
                    MessageBox.Show("Error: " + whatIs);
                    return "SIGMA";
                }
                
                if (q == 9)
                {
                    if (i == 1 && cadena.Length > 2)
                    {
                        String st = cadena.Substring(0, i+1);
                        if (!operadores.Contains(st))
                        {
                            break;
                        }
                        else
                        {
                            i++;
                            break;
                        }
                    }
                }

                if (q == -1)
                {
                    q = q0;
                    break;
                }
            }

            index = i;
            if(q == -1)
            {
                return "ERROR";
            }

            if (estadosA.Contains(q))
            {
                switch (q)
                {
                    case 2:
                        if (cadena.Substring(0, index).Length == 3)
                            return "CARACTER";
                        return "CADENA";
                    case 3:
                        return "ENTERO";
                    case 5:
                        return "DECIMAL";
                    case 7:
                        String str1 = cadena.Substring(0, index);
                        if (reservadas.Contains(str1))
                        {
                            if (str1.Equals("entero"))
                                return "entero";
                            if (str1.Equals("decimal"))
                                return "decimal";
                            if (str1.Equals("cadena"))
                                return "cadena";
                            if (str1.Equals("booleano"))
                                return "booleano";
                            if (str1.Equals("caracter"))
                                return "caracter";

                            return "RESERVADO";
                        }
                        else{
                            if (str1.Equals("verdadero"))
                                return "BOOLEANO";
                            if (str1.Equals("falso"))
                                return "BOOLEANO";
                            if (str1.Length == 1)
                                return "CHAR";
                        }
                        if(str1.StartsWith("_"))
                            return "id";

                        return "TEXTO";

                    case 10:
                        return "COMENTARIO";
                    case 13:
                        return "COMENTARIO";
                    case 15:
                        return "FIN";
                    case 16:
                        return "ESPACIO";
                }

                if(q == 8 || q == 9)
                {
                    String str1 = cadena.Substring(0, index);
                    if (str1.Equals("=") || str1.Equals(";") || str1.Equals("+=") || str1.Equals("-=") || str1.Equals("*=") || str1.Equals("/="))
                        return "OPERADORES_FS";
                    if (operadores.Contains(str1))
                        return "OPERADORES";


                    return "NO VALIDO";
                }
            }

            return "NO VALIDO";
        }

        public void setAutomata()
        {
            int estados = 17;
            List<int> acep = new List<int>();
            acep.Add(2);
            acep.Add(3);
            acep.Add(5);
            acep.Add(7);
            acep.Add(8);
            acep.Add(9);
            acep.Add(10);
            acep.Add(13);
            acep.Add(15);
            acep.Add(16);

            List<String> alf = new List<String>();
            alf.Add("L");
            alf.Add("N");
            alf.Add("S");
            alf.Add("C");
            alf.Add("A");
            alf.Add("D");
            alf.Add("P");
            alf.Add("U");
            alf.Add("E");
            alf.Add("R");
            alf.Add("F");

            this.buildAutomata(estados, acep, alf);
            setEmpties();
            //String
            setTransiciones(0, 3, 1);
            setTransiciones(1, 0, 1);
            setTransiciones(1, 1, 1);
            setTransiciones(1, 2, 1);
            setTransiciones(1, 3, 2);
            setTransiciones(1, 4, 1);
            setTransiciones(1, 5, 1);
            setTransiciones(1, 6, 1);
            setTransiciones(1, 7, 1);
            setTransiciones(1, 8, 1);

            //Numeros
            setTransiciones(0, 1, 3);
            setTransiciones(3, 1, 3);
            setTransiciones(3, 6, 4);
            setTransiciones(4, 1, 5);
            setTransiciones(5, 1, 5);

            //Cadenas - identificadores
            setTransiciones(0, 0, 7);
            setTransiciones(0, 7, 6);
            setTransiciones(6, 0, 7);
            setTransiciones(7, 0, 7);
            setTransiciones(7, 1, 7);
            setTransiciones(7, 7, 7);

            //Operadores
            setTransiciones(0, 2, 9);
            setTransiciones(0, 4, 9);
            setTransiciones(0, 5, 8);
            setTransiciones(8, 2, 9);
            setTransiciones(9, 2, 9);

            //Comentarios
            setTransiciones(8, 4, 11);
            setTransiciones(8, 5, 10);
            for(int i=0; i<alf.Count-2; i++)
            {
                setTransiciones(10, i, 10);
                setTransiciones(11, i, 11);
                setTransiciones(12, i, 11);
            }
            //Comentario entre lineas
            setTransiciones(11, 9, 11);
            setTransiciones(11, 10, 11);

            setTransiciones(11, 4, 12);
            setTransiciones(12, 5, 13);

            //Espacio
            setTransiciones(0, 8, 16);

            //Salto Linea
            setTransiciones(0, 9, 14);
            setTransiciones(14, 10, 15);
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
