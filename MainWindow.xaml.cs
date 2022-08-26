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
using System.IO.Compression;

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

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg files (*.jpg)|*.jpg";
            dialog.InitialDirectory = @"%USERPROFILE%\OneDrive\Escritorio\";
            dialog.Title = "Please select an image file.";
            if (dialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(dialog.FileName);
                imagePicture.Source = new BitmapImage(fileUri);
            }
        }

        private void addImageList_Click(object sender, RoutedEventArgs e)
        {
            String extractPath = "C:/Users/it-ra/OneDrive/Escritorio/extract";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "cb* files (*.cbr)|*.cbr";
            dialog.InitialDirectory = @"%USERPROFILE%\OneDrive\Escritorio\";
            dialog.Title = "Please select an comic file.";
            if (dialog.ShowDialog() == true)
            {
                Uri cbFile = new Uri(dialog.FileName);
                Uri extractFile = new Uri(extractPath);
                MessageBox.Show(cbFile.AbsolutePath);
                try
                {
                    MessageBox.Show(extractFile.AbsolutePath);

                    ZipFile.ExtractToDirectory(cbFile.AbsolutePath, extractFile.AbsolutePath, true);
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
