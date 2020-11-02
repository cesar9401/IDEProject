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
        private Stack<TreeNode> pila;
        private Queue<Token> tokens;
        private List<String> terminales;
        private List<String> changes;
        private Stack<String>[,] values;
        public List<String> reports;
        private Boolean acept;
        private DefGramatica gramatica;
        private int actualNode;
        private List<TreeNode> tree;

        //Constructor, construir automata
        public AutomataPila()
        {
            this.tree = new List<TreeNode>();
            this.actualNode = 0;

            gramatica = new DefGramatica();
            reports = new List<String>();
            this.terminales = gramatica.GetTerminales();
            this.changes = gramatica.GetChanges();
            this.values = gramatica.GetValues();

            pila = new Stack<TreeNode>();
            pila.Push(new TreeNode("$", -1));
            pila.Push(new TreeNode("S", actualNode));
            tree.Add(new TreeNode(null, "S", actualNode));
            actualNode++;
            this.acept = true;
        }

        //Analizar cadena
        public void StartAnalisis()
        {
            ShowPila();
            while (pila.Count > 0 && tokens.Count > 0)
            {
                //Acciones Shift
                if (terminales.Contains(pila.Peek().data))
                {
                    Console.WriteLine("Shift " + pila.Peek().data);
                    Shift(pila.Peek().data, tokens.Peek());
                    ShowPila();
                }
                else
                {
                    //Acciones Reduce E
                    if (pila.Peek().data.Equals("E"))
                    {
                        Console.WriteLine("Reduce E");
                        pila.Pop();
                        ShowPila();
                    }
                    else
                    {
                        // Acciones para Reduce
                        string str = GetString(tokens.Peek());

                        if (str.Equals(pila.Peek().data))
                        {
                            //Reduce
                            Console.WriteLine("Reduce " + pila.Peek().data);
                            tokens.Dequeue();
                            pila.Pop();
                            ShowPila();
                        }
                        else
                        {
                            Console.WriteLine("Se esperaba: " + pila.Peek().data);
                            reports.Add("Error Sintactico: Fila: " + tokens.Peek().row + ", Columna: " + tokens.Peek().col + ", se ha encontrado: " + tokens.Peek().type + "(" + tokens.Peek().cadena + "), se esperaba: " + pila.Peek().data);
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
            foreach(TreeNode s in pila)
            {
                Console.Write(s.data);
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
                    TreeNode father = pila.Pop();
                    Stack<TreeNode> tmp = new Stack<TreeNode>();

                    foreach (String t in values[indexT, indexC])
                    {
                        pila.Push(new TreeNode(t, actualNode));
                        tmp.Push(new TreeNode(father, t, actualNode));
                        actualNode++;
                    }

                    foreach(TreeNode t in tmp)
                    {
                        if (!t.data.Equals("E"))
                        {
                            tree.Add(t);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("indexT: " + indexT);
                    Console.WriteLine("indexC: " + indexC);
                    Console.WriteLine("Pop en pila: " + pila.Peek().data);
                    reports.Add("Error Sintactico: Fila: " + tokens.Peek().row + ", Columna: " + tokens.Peek().col + ", se ha encontrado: " + tokens.Peek().type + "(" + tokens.Peek().cadena + "), no es posible reconocer el token.");
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

        //Metodo para escoger el tipo de cadena a retornar
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

        //Obtener string para generar archivo .dot
        public String getInstructions()
        {
            //Construir archivo
            String instructions = "digraph arbolSintactico{\n";
            foreach (TreeNode t in tree)
            {
                if (t.data.Equals("id"))
                {
                    t.data += getCondition(t);
                }
                instructions += "\tNodo" + t.id + "[label=\"" + t.data + "\"];\n";
            }
            foreach (TreeNode t in tree)
            {
                if (t.father != null)
                {
                    instructions += "\tNodo" + t.father.id + " -> Nodo" + t.id + "[arrowhead = none];\n";
                }
            }
            instructions += "}";
            Console.WriteLine(instructions);
            return instructions;
        }

        //Vericar si existe o se establecera el id en la tabla de simbolos
        public String getCondition(TreeNode t)
        {
            List<String> genId = new List<String>()
            {
                "B", "EN'", "DEC'", "CAD'", "BOL'", "CAR'", "A'", "B'"
            };

            List<String> isId = new List<String>()
            {
                "N", "W", "R", "B'", "K"
            };

            if (t.father != null)
            {
                if (genId.Contains(t.father.data) && isId.Contains(t.father.data))
                {
                    return "(V&A -> ts)";
                }
                else if (genId.Contains(t.father.data))
                {
                    return "(A -> ts)";
                }
                else if (isId.Contains(t.father.data))
                {
                    return "(V -> ts)";
                }
            }
            return "";
        }

        //Metodo get del arbol
        public List<TreeNode> GetTreeNode()
        {
            return this.tree;
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

        //Metodo para verificar si el codigo fuente es aceptado
        public Boolean IsAcept()
        {
            return this.acept;
        }
    }
}
