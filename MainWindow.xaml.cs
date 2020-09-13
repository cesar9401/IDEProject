using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data.Common;

namespace IDEProject
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String path;

        public MainWindow()
        {
            InitializeComponent();
            consoleText.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            consoleText.Document.PageWidth = 1920;
            this.Title = "NoteC";
        }

        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            //probandoAutomata();
            //consoleText.Document.Blocks.Clear();
            //path = null; 
            //this.Title = "NoteC";
            //analizarCadena();
            paintText();
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
                open.Filter = "css files (*.css)|*.css";
                open.ShowDialog();

                if (File.Exists(open.FileName))
                {
                    path = open.FileName;
                    TextReader read = new StreamReader(path);
                    consoleText.Document.Blocks.Clear();
                    consoleText.Document.Blocks.Add(new Paragraph(new Run(read.ReadToEnd())));
                    read.Close();
                    paintText();

                    this.Title = path + " - NoteC";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al abrir");
            }
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog forSave = new SaveFileDialog();
                forSave.Title = "Guardar";
                forSave.Filter = "css files (*.css)|*.css";
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
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se ha podido guardar el archivo");
            }
        }

        private void paintText()
        {
            int i = 0;
            TextPointer pos0 = consoleText.Document.ContentStart;
            TextPointer posF = consoleText.Document.ContentEnd;
            TextRange range = new TextRange(pos0, posF);
            String content = range.Text;

            String aux = content;
            String str = "queso";
            int index = 0;
            while (aux.Contains(str))
            {
                index = aux.IndexOf(str);
                aux = aux.Substring(0, index);
                if (i == 0)
                {
                    range.Text = aux;
                }
                else
                {
                    paint(aux, Brushes.White);
                }

                consoleText.CaretPosition = consoleText.Document.ContentEnd;
                paint(str, Brushes.DarkOrange);
                i = index + str.Length;
                aux = content.Substring(i, content.Length - i);
                MessageBox.Show(aux);
                if (!aux.Contains(str))
                {
                    //paint(aux.Substring(0, aux.IndexOf(str)), Brushes.White);
                    paint(aux, Brushes.White);
                }
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

        private void paint(string texto, SolidColorBrush color)
        {
            TextRange rangeOfText1 = new TextRange(consoleText.Document.ContentEnd, consoleText.Document.ContentEnd);
            rangeOfText1.Text = texto;
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }

        private void cursorPosition()
        {
            TextPointer pos = consoleText.CaretPosition;
            pos = consoleText.CaretPosition.GetPositionAtOffset(20);
            pos = pos.DocumentEnd;
            consoleText.CaretPosition = pos;
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
            //paintText();
        }

        private void cambio(object sender, DependencyPropertyChangedEventArgs e)
        {
            //getLines();
        }

        private void analizarCadena() 
        {
            String cadena = StringFromRichTextBox();
            for(int i=0; i<cadena.Length; i++)
            {
                String ch = cadena.Substring(i, 1);
                char c = Convert.ToChar(ch);
                int index = (int)c;
                MessageBox.Show("index: " + index);
            }
        }

        private void probandoAutomata()
        {
            int estados = 3;
            List<int> aceptacion = new List<int>();
            aceptacion.Add(2);
            List<String> alfabeto = new List<String>();
            alfabeto.Add("0");
            alfabeto.Add("1");
            Automata afd = new Automata(estados, aceptacion, alfabeto);
            afd.setTransiciones(0, 0, 1);
            afd.setTransiciones(0 ,1 ,2);
            afd.setTransiciones(1, 0, 1);
            afd.setTransiciones(1, 1, 2);
            afd.setTransiciones(2, 0, 1);
            afd.setTransiciones(2, 1, 2);

            String texto = StringFromRichTextBox();
            Boolean estado = afd.verificarCadena(texto);
            labelCadena.Content = estado;
        }

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            getLines();
        }
    }
}
