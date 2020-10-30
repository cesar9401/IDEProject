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


        //Constructor, construir automata
        public AutomataPila()
        {
            reports = new List<String>();
            SetTerminales();
            SetChanges();
            this.values = new Stack<String>[this.terminales.Count, this.changes.Count];
            InitValues();
            BuildAutomata();
            pila = new Stack<String>();
            pila.Push("$");
            //pila.Push("S");
            foreach(String t in values[0, 0])
            {
                pila.Push(t);
            }
            this.acept = true;
        }

        //Iniciarlizar las casillas de la matriz
        private void InitValues()
        {
            for(int i=0; i<values.GetLength(0); i++)
            {
                for(int j=0; j<values.GetLongLength(1); j++)
                {
                    values[i, j] = new Stack<string>();
                }
            }
        }


        //Terminales en tabla de analisis
        private void SetTerminales()
        {
            this.terminales = new List<String>()
            {
                "S", "A", "B", "EN", "EN'", "DEC", "DEC'", "CAD", "CAD'", "BOL", "BOL'", "CAR", "CAR'", "R", "W", "K", "K'",
                "B'", "AS", "X", "X'", "T", "T'", "P", "P'", "U", "N", "I", "I'", "C", "C'", "LOG", "COM", "OC", "OL", "M", "H", "F", "A'"
            };
        }

        //Cambios en tabla de analisis
        private void SetChanges()
        {
            this.changes = new List<String>()
            {
                "principal", "(", ")", "{", "}", "=", ",", ";", "+", "-", "*", "/", "^", "++", "--", "&&", "||", ">=", "<=", ">", "<", "==", "!=", "!", "+=", "-=", "*=", "/=", "id", 
                "entero", "decimal", "cadena", "booleano", "caracter", "leer", "imprimir", "ENTERO", "DECIMAL", "CADENA", "BOOLEANO", "CARACTER", "SI", "SINO_SI", "SINO", "MIENTRAS", "HACER", "DESDE", "INCREMENTO", "$"
            };
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
        private void BuildAutomata()
        {
            //Valores para S
            //values[0, 0] = new Stack<String>();
            values[0, 0].Push("principal");
            values[0, 0].Push("(");
            values[0, 0].Push(")");
            values[0, 0].Push("{");
            values[0, 0].Push("A");
            values[0, 0].Push("}");

            //Valores para A
            //values[1, 4] = new Stack<string>();
            values[1, 4].Push("E");
            values[1, 28].Push("B'");
            values[1, 28].Push("A");
            values[1, 29].Push("B");
            values[1, 29].Push("A");
            values[1, 30].Push("B");
            values[1, 30].Push("A");
            values[1, 31].Push("B");
            values[1, 31].Push("A");
            values[1, 32].Push("B");
            values[1, 32].Push("A");
            values[1, 33].Push("B");
            values[1, 33].Push("A");
            values[1, 34].Push("R");
            values[1, 34].Push("A");
            values[1, 35].Push("W");
            values[1, 35].Push("A");
            values[1, 41].Push("I");
            values[1, 41].Push("A");
            values[1, 44].Push("M");
            values[1, 44].Push("A");
            values[1, 45].Push("H");
            values[1, 45].Push("A");
            values[1, 46].Push("F");
            values[1, 46 ].Push("A");

            //Valores para B
            values[2, 29].Push("entero");
            values[2, 30].Push("decimal");
            values[2, 31].Push("cadena");
            values[2, 32].Push("booleano");
            values[2, 33].Push("caracter");
            for (int i = 29; i < 34; i++)
            {
                values[2, i].Push("id");
            }

            values[2, 29].Push("EN");
            values[2, 30].Push("DEC");
            values[2, 31].Push("CAD");
            values[2, 32].Push("BOL");
            values[2, 33].Push("CAR");

            for (int i = 29; i < 34; i++)
            {
                values[2, i].Push(";");
            }

            //Valores para COL -> 5
            for (int i = 3; i < 12; i+=2)
            {
                //values[i, 5] = new Stack<string>();
                values[i, 5].Push("=");
            }
            values[3, 5].Push("X");
            values[3, 5].Push("EN'");
            values[5, 5].Push("X");
            values[5, 5].Push("DEC'");
            values[7, 5].Push("CADENA");
            values[7, 5].Push("CAD'");
            values[9, 5].Push("BOOLEANO");
            values[9, 5].Push("BOL'");
            values[11, 5].Push("CARACTER");
            values[11, 5].Push("CAR'");

            //Valores para COL --> 6
            for (int i = 3; i < 13; i++)
            {
                values[i, 6] = new Stack<string>();
                if (i % 2 == 0)
                {
                    values[i, 6].Push(",");
                    values[i, 6].Push("id");
                }
            }
            values[3, 6].Push("EN'");
            values[4, 6].Push("EN");

            values[5, 6].Push("DEC'");
            values[6, 6].Push("DEC");

            values[7, 6].Push("CAD'");
            values[8, 6].Push("CAD");

            values[9, 6].Push("BOL'");
            values[10, 6].Push("BOL");

            values[11, 6].Push("CAR'");
            values[12, 6].Push("CAR");

            //Valores para COL --> 7
            for(int i=3; i<13; i++)
            {
                //values[i, 7] = new Stack<string>();
                values[i, 7].Push("E");
            }

            //Valores para R
            //values[13, 15] = new Stack<string>();
            values[13, 34].Push("leer");
            values[13, 34].Push("(");
            values[13, 34].Push("id");
            values[13, 34].Push(")");
            values[13, 34].Push(";");

            //Valores para W
            values[14, 35] = new Stack<string>();
            values[14, 35].Push("imprimir");
            values[14, 35].Push("(");
            values[14, 35].Push("K");
            values[14, 35].Push(")");
            values[14, 35].Push(";");

            //Valores para K
            values[15, 28].Push("id");
            values[15, 28].Push("K'");

            values[15, 36].Push("ENTERO");
            values[15, 37].Push("DECIMAL");
            values[15, 38].Push("CADENA");
            values[15, 39].Push("BOOLEANO");
            values[15, 40].Push("CARACTER");

            for (int i = 36; i < 41; i++)
            {
                values[15, i].Push("K'");
            }

            //Valores para K'
            values[16, 2].Push("E");
            values[16, 8].Push("+");
            values[16, 8].Push("K");

            //Valores para B'
            values[17, 28].Push("id");
            values[17, 28].Push("AS");
            values[17, 28].Push(";");

            //Valores para AS
            values[18, 5].Push("=");
            values[18, 5].Push("X");
            values[18, 13].Push("++");
            values[18, 14].Push("--");

            values[18, 24].Push("+=");
            values[18, 24].Push("X");
            values[18, 25].Push("-=");
            values[18, 25].Push("X");
            values[18, 26].Push("*=");
            values[18, 26].Push("X");
            values[18, 27].Push("/=");
            values[18, 27].Push("X");

            //Valores para X
            values[19, 1].Push("T");
            values[19, 1].Push("X'");
            values[19, 9].Push("T");
            values[19, 9].Push("X'");
            values[19, 28].Push("T");
            values[19, 28].Push("X'");
            values[19, 36].Push("T");
            values[19, 36].Push("X'");
            values[19, 37].Push("T");
            values[19, 37].Push("X'");

            //Valores para X'
            values[20, 2].Push("E");
            values[20, 6].Push("E");
            values[20, 7].Push("E");
            values[20, 8].Push("+");
            values[20, 8].Push("T");
            values[20, 9].Push("-");
            values[20, 9].Push("T");
            for(int i=15; i<23; i++)
            {
                values[20, i].Push("E");
            }

            values[20, 47].Push("E");

            //Valores para T
            values[21, 1].Push("P");
            values[21, 1].Push("T'");
            values[21, 9].Push("P");
            values[21, 9].Push("T'");
            values[21, 28].Push("P");
            values[21, 28].Push("T'");
            values[21, 36].Push("P");
            values[21, 36].Push("T'");
            values[21, 37].Push("P");
            values[21, 37].Push("T'");

            //Valores para T'
            values[22, 2].Push("E");
            for (int i = 6; i < 10; i++)
            {
                values[22, i].Push("E");
            }
            values[22, 10].Push("*");
            values[22, 10].Push("P");
            values[22, 11].Push("/");
            values[22, 11].Push("P");
            for (int i = 15; i < 23; i++)
            {
                values[22, i].Push("E");
            }

            values[22, 47].Push("E");

            //Valores para P
            values[23, 1].Push("U");
            values[23, 1].Push("P'");
            values[23, 9].Push("U");
            values[23, 9].Push("P'");
            values[23, 28].Push("U");
            values[23, 28].Push("P'");
            values[23, 36].Push("U");
            values[23, 36].Push("P'");
            values[23, 37].Push("U");
            values[23, 37].Push("P'");

            //Valores para P'
            values[24, 2].Push("E");
            for (int i=6; i<12; i++)
            {
                values[24, i].Push("E");
            }
            values[24, 12].Push("^");
            values[24, 12].Push("P");
            for (int i = 15; i < 23; i++)
            {
                values[24, i].Push("E");
            }

            values[24, 47].Push("E");


            //Valores para U
            values[25, 1].Push("N");
            values[25, 9].Push("-");
            values[25, 9].Push("N");
            values[25, 28].Push("N");
            values[25, 36].Push("N");
            values[25, 37].Push("N");

            //Valores para N
            values[26, 1].Push("(");
            values[26, 1].Push("X");
            values[26, 1].Push(")");
            values[26, 28].Push("id");
            values[26, 36].Push("ENTERO");
            values[26, 37].Push("DECIMAL");

            //Valores para I
            values[27, 41].Push("SI");
            values[27, 41].Push("(");
            values[27, 41].Push("C");
            values[27, 41].Push(")");
            values[27, 41].Push("{");
            values[27, 41].Push("A");
            values[27, 41].Push("}");
            values[27, 41].Push("I'");

            //Valores para I'
            values[28, 4].Push("E");
            for(int i=28; i<36; i++)
            {
                values[28, i].Push("E");
            }
            values[28, 41].Push("E");

            values[28, 42].Push("SINO_SI");
            values[28, 42].Push("(");
            values[28, 42].Push("C");
            values[28, 42].Push(")");
            values[28, 42].Push("{");
            values[28, 42].Push("A");
            values[28, 42].Push("}");
            values[28, 42].Push("I'");

            values[28, 43].Push("SINO");
            values[28, 43].Push("{");
            values[28, 43].Push("A");
            values[28, 43].Push("}");

            //Valores para C
            values[29, 1].Push("LOG");
            values[29, 1].Push("OL");
            values[29, 2].Push("E");
            values[29, 9].Push("LOG");
            values[29, 9].Push("OL");

            values[29, 23].Push("C'");
            values[29, 23].Push("BOOLEANO");
            values[29, 23].Push("OL");

            values[29, 28].Push("LOG");
            values[29, 28].Push("OL");
            values[29, 36].Push("LOG");
            values[29, 36].Push("OL");
            values[29, 37].Push("LOG");
            values[29, 37].Push("OL");

            values[29, 39].Push("C'");
            values[29, 39].Push("BOOLEANO");
            values[29, 39].Push("OL");

            values[29, 47].Push("E");

            //Valores para C'
            values[30, 23].Push("!");
            values[30, 23].Push("C'");
            values[30, 39].Push("E");

            //Valores para LOG
            values[31, 1].Push("X");
            values[31, 1].Push("COM");
            values[31, 9].Push("X");
            values[31, 9].Push("COM");
            values[31, 28].Push("X");
            values[31, 28].Push("COM");
            values[31, 36].Push("X");
            values[31, 36].Push("COM");
            values[31, 37].Push("X");
            values[31, 37].Push("COM");

            values[31, 47].Push("E");

            //Valores para COM
            for(int i=17; i<23; i++)
            {
                values[32, i].Push("OC");
                values[32, i].Push("X");
            }

            //Valores para OC
            values[33, 17].Push(">=");
            values[33, 18].Push("<=");
            values[33, 19].Push(">");
            values[33, 20].Push("<");
            values[33, 21].Push("==");
            values[33, 22].Push("!=");

            //Valores para OL
            values[34, 2].Push("E");
            values[34, 15].Push("&&");
            values[34, 15].Push("C");
            values[34, 16].Push("||");
            values[34, 16].Push("C");

            values[34, 47].Push("E");

            //Valores para M
            values[35, 44].Push("MIENTRAS");
            values[35, 44].Push("(");
            values[35, 44].Push("C");
            values[35, 44].Push(")");
            values[35, 44].Push("{");
            values[35, 44].Push("A");
            values[35, 44].Push("}");

            //Valores para H
            values[36, 45].Push("HACER");
            values[36, 45].Push("{");
            values[36, 45].Push("A");
            values[36, 45].Push("}");
            values[36, 45].Push("MIENTRAS");
            values[36, 45].Push("(");
            values[36, 45].Push("C");
            values[36, 45].Push(")");
            values[36, 45].Push(";");

            //Valores para F
            values[37, 46].Push("DESDE");
            values[37, 46].Push("A'");
            values[37, 46].Push("HASTA");
            values[37, 46].Push("C");
            values[37, 46].Push("INCREMENTO");
            values[37, 46].Push("ENTERO");
            values[37, 46].Push("{");
            values[37, 46].Push("A");
            values[37, 46].Push("}");

            //Valores para A'
            values[38, 28].Push("id");
            values[38, 28].Push("=");
            values[38, 28].Push("ENTERO");

            values[38, 29].Push("entero");
            values[38, 29].Push("id");
            values[38, 29].Push("=");
            values[38, 29].Push("ENTERO");
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
