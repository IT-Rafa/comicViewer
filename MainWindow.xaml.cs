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
        private List<string> filePaths = new();
        private int imageIndex;
        /**
         * Open new image
         * 
         */
        public MainWindow()
        {
            InitializeComponent();
        }
        /**
         * Open new image
         * 
         */
        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Please select an image file."
            };
            if (dialog.ShowDialog() == true)
            {
                filePaths.Add(dialog.FileName);
                imageIndex = 0;

                Uri fileUri = new(filePaths[imageIndex]);
                imagePicture.Source = new BitmapImage(fileUri);

            }
        }
        /**
         * Open comic (cbr, cbz)
         * 
         */
        private void AddComic_Click(object sender, RoutedEventArgs e)
        {
            //Path.GetTempFileName();
            String extractPath = Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop) + "\\extract";

            OpenFileDialog dialog = new()
            {
                Filter = "Comic Files(*.cbr; *.cbz;)|*.cbr; *.cbz",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Please select an comic file."
            };

            if (dialog.ShowDialog() == true)
            {
                Boolean fileOk = false;
                try
                {
                    ZipFile.ExtractToDirectory(dialog.FileName, extractPath, true);

                    filePaths.Clear();
                    filePaths = Directory.GetFiles(extractPath).ToList<String>();
                    imageIndex = 0;
                    fileOk = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "A handled exception just occurred: "
                        + ex.Message, "Exception Sample");
                }

                if (fileOk)
                {
                    
                    Uri fileUri = new(filePaths[imageIndex]);

                    imagePicture.Source = new BitmapImage(fileUri);

                }


            }
        }

        private void ImagePicture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (filePaths.Count > imageIndex)
            {
                Uri fileUri = new(filePaths[++imageIndex]);
                imagePicture.Source = new BitmapImage(fileUri);
                imagePicture.StretchDirection = StretchDirection.UpOnly;
            }

        }
    }
}
