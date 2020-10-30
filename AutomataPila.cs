using System;
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
        public List<String> reports;
        private Boolean acept;
        private DefGramatica gramatica;

        //Constructor, construir automata
        public AutomataPila()
        {
            gramatica = new DefGramatica();
            reports = new List<String>();
            this.terminales = gramatica.GetTerminales();
            this.changes = gramatica.GetChanges();
            this.values = gramatica.GetValues();

            pila = new Stack<String>();
            pila.Push("$");
            foreach(String t in values[0, 0])
            {
                pila.Push(t);
            }
            this.acept = true;
        }

        //Analizar cadena
        public void StartAnalisis()
        {
            ShowPila();
            while (pila.Count > 0 && tokens.Count > 0)
            {
                //Acciones Shift
                if (terminales.Contains(pila.Peek()))
                {
                    Console.WriteLine("Shift " + pila.Peek());
                    Shift(pila.Peek(), tokens.Peek());
                    ShowPila();
                }
                else
                {
                    //Acciones Reduce E
                    if (pila.Peek().Equals("E"))
                    {
                        Console.WriteLine("Reduce E");
                        pila.Pop();
                        ShowPila();
                    }
                    else
                    {
                        // Acciones para Reduce
                        string str = GetString(tokens.Peek());

                        if (str.Equals(pila.Peek()))
                        {
                            //Reduce
                            Console.WriteLine("Reduce " + pila.Peek());
                            tokens.Dequeue();
                            pila.Pop();
                            ShowPila();
                        }
                        else
                        {
                            Console.WriteLine("Se esperaba: " + pila.Peek());
                            reports.Add("Error Sintactico: Fila: " + tokens.Peek().row + ", Columna: " + tokens.Peek().col + ", se ha encontrado: " + tokens.Peek().type + "(" + tokens.Peek().cadena + "), se esperaba: " + pila.Peek());
                            Console.WriteLine("No es posible hacer reduce");
                            ShowNextToken();
                            //Pasar al siguiente token
                            tokens.Dequeue();
                            pila.Pop();
                            ShowPila();
                            ShowNextToken();
                            this.acept = false;
                        }
                    }
                }
            }

            if(pila.Count == 0 && tokens.Count == 0)
            {
                if(acept)
                {
                    reports.Add("No se encontro ningun error sintactico, codigo fuente aceptado.");
                    Console.WriteLine("Pila Vacia -> Cadena Aceptada");
                }
                else
                {
                    Console.WriteLine("Error");
                }
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

        //Mostrar el siguiente token
        private void ShowNextToken()
        {
            if(tokens.Count > 0)
            {
                Console.WriteLine("Siguiente token -> Tipo: " + tokens.Peek().type + ", cadena: " + tokens.Peek().cadena + ", row: " + tokens.Peek().row + ", col: " + tokens.Peek().col);
            }
        }

        //Acciones shift
        private void Shift(String terminal, Token tkn)
        {
            int indexT = terminales.IndexOf(terminal);
            int indexC = changes.IndexOf(GetString(tkn));

            if (indexT != -1 && indexC != -1)
            {
                //values[indexT, indexC] != null
                if(values[indexT, indexC].Count > 0)
                {
                    pila.Pop();
                    foreach (String t in values[indexT, indexC])
                    {
                        pila.Push(t);
                    }
                }
                else
                {
                    Console.WriteLine("indexT: " + indexT);
                    Console.WriteLine("indexC: " + indexC);
                    
                    Console.WriteLine("Pop en pila: " + pila.Peek());
                    pila.Pop();
                    this.acept = false;
                }
            }
            else
            {
                Console.WriteLine("indexT: " + indexT);
                Console.WriteLine("indexC: " + indexC);
                Console.WriteLine("Eliminar token: " + tokens.Peek().cadena + " tipo: " + tokens.Peek().type);
                reports.Add("Error Sintactico: Fila: " + tokens.Peek().row + ", Columna: " + tokens.Peek().col + ", se ha encontrado: " + tokens.Peek().type + "(" + tokens.Peek().cadena + "), no es posible reconocer el token.");
                tokens.Dequeue();
                this.acept = false;
            }
        }

        private String GetString(Token tkn)
        {
            if(tkn.type.Equals("OPERADORES") || tkn.type.Equals("OPERADORES_FS") || tkn.type.Equals("RESERVADO"))
            {
                return tkn.cadena;
            }
            else
            {
                return tkn.type;
            }
        }

        //Construccion de automata


        //Se inicializan los tokens
        public void SetTokens(List<Token> tokens)
        {
            this.tokens = new Queue<Token>();
            foreach(Token t in tokens)
            {
                if (!t.type.Equals("FIN") && !t.type.Equals("ESPACIO") && !t.type.Equals("COMENTARIO"))
                {
                    this.tokens.Enqueue(t);
                }
            }
            this.tokens.Enqueue(new Token("$", "$", 0, 0));
        }
    }
}
