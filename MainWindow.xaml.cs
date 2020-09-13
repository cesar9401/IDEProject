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
        }

        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            //probandoAutomata();
            analizarCadena();
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
                //SaveFileDialog save = new SaveFileDialog();
                if (path != null)
                {
                    if (path.Length != 0)
                    {
                        StreamWriter save = File.CreateText(path);
                        String text = StringFromRichTextBox();
                        save.Write(text);
                        save.Flush();
                        save.Close();
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
            TextRange range = new TextRange(consoleText.Document.ContentStart, consoleText.Document.ContentEnd);
            String words = range.Text;
            if (words.Contains(";"))
            {
                
                int i = words.LastIndexOf(";");
                String aux = words;
                String newString = aux.Remove(i, words.Length - i);
                consoleText.Document.Blocks.Clear();
                TextRange rangeOfText1 = new TextRange(consoleText.Document.ContentStart, consoleText.Document.ContentEnd);
                rangeOfText1.Text = newString;
                paint(";");
                
                //Mover cursor
                TextPointer pos = consoleText.CaretPosition;
                pos = pos.DocumentEnd;
                consoleText.CaretPosition = pos;
            }
  
            //var startPointer = consoleText.Document.ContentStart.GetPositionAtOffset(0);
            //var endPointer = consoleText.Document.ContentEnd.GetPositionAtOffset(-10);
            //consoleText.Selection.Select(startPointer, endPointer);
        }

        private void getLines()
        {
            TextPointer pos0 = consoleText.Document.ContentStart;
            TextPointer pos1 = consoleText.Document.ContentEnd;
            TextRange rangeOfText1 = new TextRange(pos0, pos1);
            int len = rangeOfText1.Text.Length;

            int column = pos0.GetOffsetToPosition(pos1);
            bool inicio = pos1.IsAtLineStartPosition;

            labelInfo.Content = "Len: " + len + ", posRel: " + column;


        }

        private void paint(string texto)
        {
            TextRange rangeOfText1 = new TextRange(consoleText.Document.ContentEnd, consoleText.Document.ContentEnd);
            rangeOfText1.Text = texto;
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.DarkOrange);
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
            //paintText();
            //cursorPosition();
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            //paintText();
            //cursorPosition();
            //String keyD = e.Key.ToString();
            //MessageBox.Show(keyD);
            /**
            String key = e.Key.ToString();
            if (key.Equals("Return") || key.Equals("Up") || key.Equals("Down") || key.Equals("Left") || key.Equals("Right"))
            {
                getLines();
            }
            */
            getLines();
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
    }
}
