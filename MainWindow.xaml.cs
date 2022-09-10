using Microsoft.Win32;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace ComicViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<string>  filePaths = new();
        private readonly List<double> vertical = new();
        private int imageIndex = 0;
        private string comicActual = "";
        private string lastFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /**
         * Open new image
         * 
         */
        public MainWindow()
        {
            InitializeComponent();

            vertical.Add(0);
            this.Title = "Comic Viewer: [Default]";

        }

        /**
         * Open new image
         * 
         */
        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Multiselect = true,
                Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp",
                InitialDirectory = lastFolder,
                Title = "Please select an image file."
            };

            if (dialog.ShowDialog() == true)
            {
                imageIndex = 0;

                filePaths.Clear();
                vertical.Clear();

                filePaths.AddRange(dialog.FileNames);

                lastFolder = dialog.FileNames[0];

                for (int x= 0; x < filePaths.Count; x++)
                {
                    vertical.Add(0);
                }

                Uri fileUri = new(filePaths[imageIndex]);
                this.Title = "Comic Viewer: Folder [" + Path.GetDirectoryName(filePaths[imageIndex]) +
                    "] [" + Path.GetFileName(filePaths[imageIndex]) + "]" + vertical[imageIndex];
                imagePicture.Source = new BitmapImage(fileUri);
            }
        }
        /**
         * Open comic (cbr, cbz)
         * 
         */
        private void AddComic_Click(object sender, RoutedEventArgs e) { 
        
            string extractPath = "D:\\source\\Windows_IDE\\ComicViewer\\resources\\extract";

            OpenFileDialog dialog = new()
            {
                Filter = "Comic Files(*.cbr; *.cbz;)|*.cbr; *.cbz",
                InitialDirectory = lastFolder,
                Title = "Please select an comic file."
            };

            if (dialog.ShowDialog() == true)
            {
                Boolean fileOk = false;
                comicActual = dialog.FileName;
                try
                {
                    imageIndex = 0;

                    filePaths.Clear();
                    vertical.Clear();

                    System.IO.DirectoryInfo di = new(extractPath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }


                    if (dialog.FileName.EndsWith(".cbr")){
                        
                        using (var archive = RarArchive.Open(dialog.FileName))
                        {
                            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                            {
                                entry.WriteToDirectory(extractPath, new ExtractionOptions()
                                {

                                });
                            }
                        }
                        fileOk = true;

                    }

                    else if (dialog.FileName.EndsWith(".cbz")){
                        ZipFile.ExtractToDirectory(dialog.FileName, extractPath, true);
                        fileOk = true;

                    }

                    if(fileOk)
                    {
                        filePaths.AddRange(Directory.GetFiles(extractPath));
                        lastFolder = dialog.FileNames[0];
                        for (int x = 0; x < filePaths.Count; x++)
                        {
                            vertical.Add(0);
                        }

                        for (int x = 0; x < filePaths.Count; x++)
                        {
                            vertical.Add(0);
                        }
                    }

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

                    this.Title = "Comic Viewer: Comic [" + Path.GetFileName(comicActual) +
                    "] [" + Path.GetFileName(filePaths[imageIndex]) + "]";

                    BitmapImage image = new();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(filePaths[imageIndex]);
                    image.EndInit();
                    imagePicture.Source = image;

                    imageContainer.ScrollToVerticalOffset(vertical[imageIndex]);

                }
            }
        }

        private void ImagePicture_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveToNextImage();
        }
        private void MoveToNextImage()
        {
            vertical[imageIndex] = imageContainer.VerticalOffset;

            if (imageIndex >=0 && imageIndex < (filePaths.Count - 1))
            {
                ++imageIndex;

                this.Title = "Comic Viewer: Comic [" + Path.GetFileName(comicActual) +
                    "] [" + Path.GetFileName(filePaths[imageIndex]) + "]";

                BitmapImage image = new();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(filePaths[imageIndex]);
                image.EndInit();
                imagePicture.Source = image;

                imageContainer.ScrollToVerticalOffset(vertical[imageIndex]);

            }

        }

        private void ImagePicture_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveToPreviousImage();
        }

        private void MoveToPreviousImage()
        {
            vertical[imageIndex] = imageContainer.VerticalOffset;

            if (imageIndex > 0 && imageIndex < (filePaths.Count))
            {
                --imageIndex;

                this.Title = "Comic Viewer: Comic [" + Path.GetFileName(comicActual) +
                    "] [" + Path.GetFileName(filePaths[imageIndex]) + "]";
                
                BitmapImage image = new();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(filePaths[imageIndex]);
                image.EndInit();

                imagePicture.Source = image; imageContainer.ScrollToVerticalOffset(vertical[imageIndex]);

            }

        }

        private void ImageContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }
    }
}
