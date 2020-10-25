using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace IDEProject
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String path;
        AutomataPila autP;

        public MainWindow()
        {
            InitializeComponent();
            consoleText.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            consoleText.Document.PageWidth = 1920;
            this.Title = "NoteC";
        }

        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            AllClear();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = "Abrir";
                open.Filter = "gt files (*.gt)|*.gt";
                open.ShowDialog();

                if (File.Exists(open.FileName))
                {
                    path = open.FileName;
                    TextReader read = new StreamReader(path);
                    consoleText.Document.Blocks.Clear();
                    TextRange range = new TextRange(consoleText.Document.ContentStart, consoleText.Document.ContentEnd);
                    range.Text = read.ReadToEnd();
                    read.Close();

                    this.Title = path + " - NoteC";

                    if (!isOpen(path))
                    {
                        treeViewDirectory.Items.Add(path);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al abrir");
            }
        }

        private Boolean isOpen(String path)
        {
            Boolean isOpen = false;
            for(int i=0; i<treeViewDirectory.Items.Count; i++)
            {
                if (treeViewDirectory.Items[i].ToString().Equals(path))
                {
                    isOpen = true;
                    break;
                }
            }

            return isOpen;
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog forSave = new SaveFileDialog();
                forSave.Title = "Guardar";
                forSave.Filter = "gt files (*.gt)|*.gt";
                if (path != null)
                {
                    if (path.Length != 0)
                    {
                        StreamWriter save = File.CreateText(path);
                        String text = StringFromRichTextBox();
                        save.Write(text);
                        save.Flush();
                        save.Close();

                        this.Title = path + " - NoteC";
                    }
                }
                else
                {
                    if(forSave.ShowDialog() == true)
                    {
                        if (forSave.FileName != null)
                        {
                            String pathF = forSave.FileName;
                            StreamWriter save = File.CreateText(pathF);
                            String text = StringFromRichTextBox();
                            save.Write(text);
                            save.Flush();
                            save.Close();
                            path = pathF;

                            this.Title = path + " - NoteC";
                            treeViewDirectory.Items.Add(path);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se ha podido guardar el archivo");
            }
        }

        private void getLines()
        {
            TextPointer pos0 = consoleText.Document.ContentStart;
            TextPointer pos1 = consoleText.CaretPosition;
            TextPointer posI = consoleText.CaretPosition.GetLineStartPosition(0);

            TextRange rangeOfText1 = new TextRange(pos0, pos1);
            TextRange rangeOfTextL = new TextRange(posI, pos1);
            
            int length = rangeOfText1.Text.Length;
            int dif = pos0.GetOffsetToPosition(pos1);

            int row = (dif - length) / 2;
            int column = rangeOfTextL.Text.Length;

            labelRow.Content = "Row -> " + row;
            labelColumn.Content = "Column -> " + column;
        }

        private void paint(string texto, SolidColorBrush color, TextPointer pos0)
        {
            TextRange rangeOfText1 = new TextRange(pos0, pos0);
            rangeOfText1.Text = texto;
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }

        string StringFromRichTextBox()
        {
            TextRange textRange = new TextRange(consoleText.Document.ContentStart, consoleText.Document.ContentEnd);
            return textRange.Text;
        }

        private void cambiosTexto(object sender, TextChangedEventArgs e)
        {
            //paint();
        }

        private void selection(object sender, MouseEventArgs e)
        {
            getLines();
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            getLines();
            TextRange range = new TextRange(consoleText.CaretPosition, consoleText.CaretPosition);
            range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
            //paintText();
        }

        private void cambio(object sender, DependencyPropertyChangedEventArgs e)
        {
            //getLines();
        }

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            getLines();
        }

        private void Compilar_Click(object sender, RoutedEventArgs e)
        {
            String cadena = StringFromRichTextBox();

            Automata aut = new Automata();
            aut.str = cadena;
            List<Token> tokens = aut.verificarCadena();
            labelCadena.Content = "Tokens: " + contarTokens(tokens);

            //Automata de Pila
            autP = new AutomataPila();
            autP.SetTokens(tokens);

            consoleText.Document.Blocks.Clear();

            for (int i=0; i<tokens.Count; i++)
            {
                String estado = tokens[i].type;
                String word = tokens[i].cadena;
                SolidColorBrush color = Brushes.GreenYellow;
                //MessageBox.Show(estado);
                switch (estado)
                {
                    case "COMENTARIO":
                        color = Brushes.Red;
                        break;
                    case "CADENA":
                        color = Brushes.Gray;
                        break;
                    case "cadena":
                        color = Brushes.Gray;
                        break;
                    case "ENTERO":
                        color = Brushes.Purple;
                        break;
                    case "entero":
                        color = Brushes.Purple;
                        break;
                    case "DECIMAL":
                        color = Brushes.LightBlue;
                        break;
                    case "decimal":
                        color = Brushes.LightBlue;
                        break;
                    case "BOOLEANO":
                        color = Brushes.DarkOrange;
                        break;
                    case "booleano":
                        color = Brushes.DarkOrange;
                        break;
                    case "CARACTER":
                        color = Brushes.Brown;
                        break;
                    case "caracter":
                        color = Brushes.Brown;
                        break;
                    case "RESERVADO":
                        color = Brushes.Green;
                        break;
                    case "OPERADORES":
                        color = Brushes.DarkBlue;
                        break;
                    case "OPERADORES_FS":
                        color = Brushes.Pink;
                        break;
                    case "NO VALIDO":
                        color = Brushes.Black;
                        break;
                }
                paint(word, color, consoleText.Document.ContentEnd);
            }
        }

        private int contarTokens(List<Token> tokens)
        {
            int count = 0;
            int errores = 0;
            String errors = "";
            for (int i = 0; i < tokens.Count; i++)
            {
                if (!tokens[i].type.Equals("FIN"))
                {
                    if(tokens[i].type.Equals("NO VALIDO"))
                    {
                        errores++;
                        errors += "Error fila: " + tokens[i].row + ", columna: " + tokens[i].col + ", cadena: " + tokens[i].cadena + "\n";
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            reportText.Document.Blocks.Clear();
            reportText.AppendText(path);
            reportText.AppendText("Cantidad de errores: " + errores);
            reportText.AppendText("\nCantidad de tokens: " + count);
            reportText.AppendText("\n" + errors);

            return count;
        }

        //Acciones guardar reporte
        private void saveReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog forSave = new SaveFileDialog();
            forSave.Title = "Save Report - NoteC";
            forSave.Filter = "gtE files (*.gtE)|*.gtE";
            TextRange range = new TextRange(reportText.Document.ContentStart, reportText.Document.ContentEnd);
            if (forSave.ShowDialog() == true)
            {
                if (forSave.FileName != null)
                {
                    String pathR = forSave.FileName;
                    StreamWriter save = File.CreateText(pathR);
                    String text = range.Text;
                    save.Write(text);
                    save.Flush();
                    save.Close();
                }
            }
        }

        //Acciones boton eliminar
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var obj = treeViewDirectory.SelectedItem;
            if (obj != null)
            {
                String name = obj.ToString();
                treeViewDirectory.Items.Remove(obj);
                if (name.Equals(path))
                {
                    AllClear();
                }
                try
                {
                    File.Delete(name);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al eliminar archivo");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un archivo");
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            var obj = treeViewDirectory.SelectedItem;
            if(obj != null)
            {
                String name = obj.ToString();
                treeViewDirectory.Items.Remove(obj);
                if (name.Equals(path))
                {
                    AllClear();
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un archivo");
            }
        }

        private void closeFile_Click(object sender, RoutedEventArgs e)
        {
            AllClear();
        }

        private void AllClear()
        {
            consoleText.Document.Blocks.Clear();
            reportText.Document.Blocks.Clear();
            path = null;
            this.Title = "NoteC";
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            var obj = treeViewDirectory.SelectedItem;
            if (obj != null)
            {
                String name = obj.ToString();
                AllClear();
                path = name;
                TextReader read = new StreamReader(path);
                consoleText.Document.Blocks.Clear();
                TextRange range = new TextRange(consoleText.Document.ContentStart, consoleText.Document.ContentEnd);
                range.Text = read.ReadToEnd();
                read.Close();

                this.Title = path + " - NoteC";
            }
            else
            {
                MessageBox.Show("Debe seleccionar un archivo");
            }
        }

        private void analizer_Click(object sender, RoutedEventArgs e)
        {
            //Acciones analizar
            autP.StartAnalisis();
            List<String> reports = autP.reports;
            reportText.AppendText("Errores de Compilacion:");
            foreach (String t in reports)
            {
                reportText.AppendText("\n" + t);
            }
        }
    }
}
