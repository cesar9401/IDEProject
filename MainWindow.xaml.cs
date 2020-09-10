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
        }

        private void newFile_Click(object sender, RoutedEventArgs e)
        {

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
                        String text = StringFromRichTextBox(consoleText);
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

        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }
    }
}
