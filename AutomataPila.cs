﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IDEProject
{
    class AutomataPila
    {
        private Stack<String> pila;
        private Queue<Token> tokens;
        private List<String> terminales;
        private List<String> changes;
        private Stack<String>[,] values;

        //Constructor, construir automata
        public AutomataPila()
        {
            SetTerminales();
            SetChanges();
            this.values = new Stack<String>[this.terminales.Count, this.changes.Count];
            BuildAutomata();
            pila = new Stack<String>();
            pila.Push("$");
            pila.Push("S");
        }

        //Terminales en tabla de analisis
        private void SetTerminales()
        {
            this.terminales = new List<String>()
            {
                "S", "A", "B", "T"
            };
        }

        //Cambios en tabla de analisis
        private void SetChanges()
        {
            this.changes = new List<String>()
            {
                "principal", "(", ")", "{", "}", " ","id", ";", "entero", "decimal", "cadena", "booleano", "caracter", "$"
            };
        }

        //Analizar cadena
        public void StartAnalisis()
        {
            while(pila.Count > 0 && tokens.Count > 0)
            {
                //Acciones Shift
                if (terminales.Contains(pila.Peek()))
                {
                    ShowPila();
                    Console.WriteLine("Shift");
                    Shift(pila.Peek(), tokens.Peek());
                    ShowPila();
                }

                //Acciones Reduce
                if (pila.Peek().Equals("E"))
                {
                    ShowPila();
                    Console.WriteLine("Reduce E");
                    pila.Pop();
                    ShowPila();
                }

                string str;
                if (tokens.Peek().type.Equals("ESPACIO"))
                {
                    str = "ESPACIO";
                }
                else if (tokens.Peek().type.Equals("id"))
                {
                    str = "id";
                }
                else
                {
                    str = tokens.Peek().cadena;
                }

                if (str.Equals(pila.Peek()))
                {
                    //Reduce
                    Console.WriteLine("Reduce");
                    ShowPila();
                    tokens.Dequeue();
                    pila.Pop();
                    ShowPila();
                }
                else
                {
                    Console.WriteLine("No se encontro en la pila");
                    Console.WriteLine("Hacer break...");
                }

                //Aceptacion
                if (pila.Peek().Equals("$"))
                {
                    pila.Pop();
                    ShowPila();
                }
            }

            if(pila.Count == 0 && tokens.Count == 0)
            {
                Console.WriteLine("Pila Vacia -> Cadena Aceptada");
            }
            else
            {
                Console.WriteLine("Cadena no aceptada");
                Console.WriteLine("Pila -> " + pila.Count);
                Console.WriteLine("Tokens -> " + tokens.Count);
            }
        }

        //Mostrar elementos en la pila
        private void ShowPila()
        {
            foreach(String s in pila)
            {
                Console.Write(s);
            }
            Console.WriteLine("");
        }

        //Acciones shift
        private void Shift(String terminal, Token tkn)
        {
            int indexT = terminales.IndexOf(terminal);
            String str;
            if(tkn.type.Equals("ESPACIO"))
            {
                str = "ESPACIO";
            } else if (tkn.type.Equals("id"))
            {
                str = "id";
            }
            else
            {
                str = tkn.cadena;
            }
            int indexC = changes.IndexOf(str);

            Console.WriteLine("indexT: " + indexT);
            Console.WriteLine("indexC: " + indexC);
            if (indexT != -1 && indexC != -1)
            {
                if(values[indexT, indexC] != null)
                {
                    pila.Pop();
                    foreach (String t in values[indexT, indexC])
                    {
                        pila.Push(t);
                    }
                }
                else
                {
                    Console.WriteLine("Lenguaje no reconocido");
                    Console.WriteLine("Eliminar token: " + tokens.Peek().cadena + " tipo: " + tokens.Peek().type);
                    tokens.Dequeue();
                }

            }
            else
            {
                Console.WriteLine("Alfabeto no reconocido");
                Console.WriteLine("Eliminar token: " + tokens.Peek().cadena + " tipo: " + tokens.Peek().type);
                tokens.Dequeue();
            }
        }

        //Construccion de automata
        private void BuildAutomata()
        {
            //Valores para S
            values[0, 0] = new Stack<String>();
            values[0, 0].Push("principal");
            values[0, 0].Push("(");
            values[0, 0].Push(")");
            values[0, 0].Push("{");
            values[0, 0].Push("A");
            values[0, 0].Push("}");

            //Valores para A
            values[1, 4] = new Stack<string>();
            values[1, 4].Push("E");
            for (int i = 8; i < 13; i++) 
            {
                values[1, i] = new Stack<string>();
                values[1, i].Push("B");
                values[1, i].Push("A");
            }

            //Valores para B
            for (int i = 8; i < 13; i++)
            {
                values[2, i] = new Stack<string>();
                values[2, i].Push("T");
                values[2, i].Push("ESPACIO");
                values[2, i].Push("id");
                values[2, i].Push(";");
            }

            //Valores para T
            for(int i = 8; i<13; i++)
            {
                values[3, i] = new Stack<string>();
            }
            values[3, 8].Push("entero");
            values[3, 9].Push("decimal");
            values[3, 10].Push("cadena");
            values[3, 11].Push("booleano");
            values[3, 12].Push("caracter");
        }

        //Se inicializan los tokens
        public void SetTokens(List<Token> tokens)
        {
            this.tokens = new Queue<Token>();
            foreach(Token t in tokens)
            {
                this.tokens.Enqueue(t);
            }
            this.tokens.Enqueue(new Token("$", "$", 0));
        }
    }
}