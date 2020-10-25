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
                "S", "A", "B", "EN", "EN'", "DEC", "DEC'", "CAD", "CAD'", "BOL", "BOL'", "CAR", "CAR'"
            };
        }

        //Cambios en tabla de analisis
        private void SetChanges()
        {
            this.changes = new List<String>()
            {
                "principal", "(", ")", "{", "}", "id", "=", ",", ";", "entero", "decimal", "cadena", "booleano", "caracter", "ENTERO", "DECIMAL", "CADENA", "BOOLEANO", "CARACTER", "$"
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
            int indexC = changes.IndexOf(GetString(tkn));

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

        private String GetString(Token tkn)
        {
            if(tkn.type.Equals("OPERADORES") || tkn.type.Equals("OPERADORES_FS"))
            {
                return tkn.cadena;
            }
            else
            {
                if (tkn.cadena.Equals("principal"))
                    return "principal";
                return tkn.type;
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
            for (int i = 9; i < 14; i++) 
            {
                values[1, i] = new Stack<string>();
                values[1, i].Push("B");
                values[1, i].Push("A");
            }

            //Valores para B
            for (int i = 9; i < 14; i++)
            {
                values[2, i] = new Stack<string>();
            }
            values[2, 9].Push("entero");
            values[2, 10].Push("decimal");
            values[2, 11].Push("cadena");
            values[2, 12].Push("booleano");
            values[2, 13].Push("caracter");
            for (int i = 9; i < 14; i++)
            {
                values[2, i].Push("id");
            }

            values[2, 9].Push("EN");
            values[2, 10].Push("DEC");
            values[2, 11].Push("CAD");
            values[2, 12].Push("BOL");
            values[2, 13].Push("CAR");

            for (int i = 9; i < 14; i++)
            {
                values[2, i].Push(";");
            }

            //Valores para COL -> 6
            for (int i = 3; i < 12; i+=2)
            {
                values[i, 6] = new Stack<string>();
                values[i, 6].Push("=");
            }
            values[3, 6].Push("ENTERO");
            values[3, 6].Push("EN'");
            values[5, 6].Push("DECIMAL");
            values[5, 6].Push("DEC'");
            values[7, 6].Push("CADENA");
            values[7, 6].Push("CAD'");
            values[9, 6].Push("BOOLEANO");
            values[9, 6].Push("BOL'");
            values[11, 6].Push("CARACTER");
            values[11, 6].Push("CAR'");

            //Valores para COL --> 7
            for (int i = 3; i < 13; i++)
            {
                values[i, 7] = new Stack<string>();
                if (i % 2 == 0)
                {
                    values[i, 7].Push(",");
                    values[i, 7].Push("id");
                }
            }
            values[3, 7].Push("EN'");
            values[4, 7].Push("EN");

            values[5, 7].Push("DEC'");
            values[6, 7].Push("DEC");

            values[7, 7].Push("CAD'");
            values[8, 7].Push("CAD");

            values[9, 7].Push("BOL'");
            values[10, 7].Push("BOL");

            values[11, 7].Push("CAR'");
            values[12, 7].Push("CAR");

            //Valores para COL --> 8
            for(int i=3; i<13; i++)
            {
                values[i, 8] = new Stack<string>();
                values[i, 8].Push("E");
            }
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
