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
using System.IO.Compression;
using System.Reflection;
using System.IO;

namespace ComicViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /**
         * Open new image
         * 
         */
        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.Title = "Please select an image file.";
            if (dialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(dialog.FileName);
                imagePicture.Source = new BitmapImage(fileUri);
            }
        }
        /**
         * Open comic (cbr, cbz)
         * 
         */
        private void addComic_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Comic Files(*.cbr; *.cbz;)|*.cbr; *.cbz";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.Title = "Please select an comic file.";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    ZipFile.ExtractToDirectory(dialog.FileName, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "A handled exception just occurred: "
                        + ex.Message, "Exception Sample");
                }

            }
        }
    }
}
