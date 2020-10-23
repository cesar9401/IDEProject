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

        //Constructor, construir automata
        public AutomataPila()
        {
            SetTerminales();
            SetChanges();
            this.values = new Stack<String>[this.terminales.Count, this.changes.Count];
            BuildAutomata();
            pila = new Stack<String>();
            pila.Push("$");
            //pila.Push("S");
            foreach(String t in values[0, 0])
            {
                pila.Push(t);
            }
        }

        //Terminales en tabla de analisis
        private void SetTerminales()
        {
            this.terminales = new List<String>()
            {
                "S", "A", "B", "C", "T"
            };
        }

        //Cambios en tabla de analisis
        private void SetChanges()
        {
            this.changes = new List<String>()
            {
                "principal", "(", ")", "{", "}", "id", ",", ";", "entero", "decimal", "cadena", "booleano", "caracter", "$"
            };
        }

        //Analizar cadena
        public void StartAnalisis()
        {
            Boolean acept = true;
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
                        string str;
                        if (tokens.Peek().type.Equals("id"))
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
                            Console.WriteLine("Reduce " + pila.Peek());
                            tokens.Dequeue();
                            pila.Pop();
                            ShowPila();
                        }
                        else
                        {
                            Console.WriteLine("Se esperaba: " + pila.Peek());
                            Console.WriteLine("No es posible hacer reduce");
                            ShowNextToken();
                            //Pasar al siguiente token
                            tokens.Dequeue();
                            pila.Pop();
                            ShowPila();
                            ShowNextToken();
                            acept = false;
                        }
                    }
                }
            }

            if(pila.Count == 0 && tokens.Count == 0)
            {
                if(acept)
                {
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
            String str;
            if (tkn.type.Equals("id"))
            {
                str = "id";
            }
            else
            {
                str = tkn.cadena;
            }
            int indexC = changes.IndexOf(str);


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
                    Console.WriteLine("indexT: " + indexT);
                    Console.WriteLine("indexC: " + indexC);
                    
                    Console.WriteLine("Pop en pila: " + pila.Peek());
                    pila.Pop();
                }
            }
            else
            {
                Console.WriteLine("indexT: " + indexT);
                Console.WriteLine("indexC: " + indexC);

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
                values[2, i].Push("id");
                values[2, i].Push("C");
                values[2, i].Push(";");
            }

            //Valores para C
            values[3, 4] = new Stack<string>();
            values[3, 4].Push("E");
            values[3, 6] = new Stack<string>();
            values[3, 6].Push(",");
            values[3, 6].Push("id");
            values[3, 6].Push("C");
            for(int i = 7 ; i < 13; i++)
            {
                values[3, i] = new Stack<string>();
                values[3, i].Push("E");
            }

            //Valores para T
            for (int i = 8; i<13; i++)
            {
                values[4, i] = new Stack<string>();
            }
            values[4, 8].Push("entero");
            values[4, 9].Push("decimal");
            values[4, 10].Push("cadena");
            values[4, 11].Push("booleano");
            values[4, 12].Push("caracter");
        }

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
