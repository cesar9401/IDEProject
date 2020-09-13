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
            consoleText.Document.Blocks.Clear();
            path = null; 
            this.Title = "NoteC";
            /**
            String content = StringFromRichTextBox();
            String str = "queso";
            paintText(content, str, consoleText.Document.ContentStart);
            */
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

        private void paintText(String content, String str, TextPointer pos0)
        {
            int i = 0;
            TextPointer posF = consoleText.Document.ContentEnd;
            TextRange range = new TextRange(pos0, posF);
            String text = range.Text;

            String aux = content;
            //String str = "queso";
            int index = 0;
            if(aux.Contains(str))
            {
                index = aux.IndexOf(str);
                aux = content.Substring(0, index);
                MessageBox.Show("aux: " + aux);
                if(text.Length != 0)
                {
                    range.Text = aux;
                }
                else
                {
                    paint(aux, Brushes.White, pos0);
                }
                pos0 = consoleText.Document.ContentEnd;
                paint(str, Brushes.DarkOrange, pos0);
                paint("", Brushes.White, pos0);

                i = index + str.Length;
                aux = content.Substring(i, content.Length - i);
                MessageBox.Show("aux: " + aux);
                if (!aux.Contains(str))
                {
                    //paint(aux.Substring(0, aux.IndexOf(str)), Brushes.White);

                    paint(aux, Brushes.White, pos0);
                    paint("", Brushes.White, pos0);
                    consoleText.CaretPosition = pos0;
                }
                else
                {
                    paintText(aux, str, pos0);
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

        private void paint(string texto, SolidColorBrush color, TextPointer pos0)
        {
            TextRange rangeOfText1 = new TextRange(pos0, pos0);
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

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            getLines();
        }

        private void Compilar_Click(object sender, RoutedEventArgs e)
        {
            //analizarCadena();
            String cadena = StringFromRichTextBox();
            Automata aut = new Automata();
            aut.cadena = cadena;
            Boolean estado = aut.verificarCadena();
            labelCadena.Content = estado;
        }
    }
}
