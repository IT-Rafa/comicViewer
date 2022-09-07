using Microsoft.Win32;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

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
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Please select an image file."
            };

            if (dialog.ShowDialog() == true)
            {
                imageIndex = 0;

                filePaths.Clear();
                vertical.Clear();

                filePaths.AddRange(dialog.FileNames);
                for(int x= 0; x < filePaths.Count; x++)
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
        private void AddComic_Click(object sender, RoutedEventArgs e)
        {


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
                    //
                    using (var archive = RarArchive.Open(dialog.FileName))
                    {
                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                        {
                            entry.WriteToDirectory(extractPath, new ExtractionOptions()
                            {

                            });
                        }
                    }




                    ZipFile.ExtractToDirectory(dialog.FileName, extractPath, true);

                    imageIndex = 0;

                    filePaths.Clear();
                    vertical.Clear();

                    filePaths.AddRange(Directory.GetFiles(extractPath));
                    for (int x = 0; x < filePaths.Count; x++)
                    {
                        vertical.Add(0);
                    }
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

                    this.Title = "Comic Viewer: Folder [" + Path.GetDirectoryName(filePaths[imageIndex]) +
                        "] [" + Path.GetFileName(filePaths[imageIndex]) + "]" + vertical[imageIndex];

                    imagePicture.Source = new BitmapImage(fileUri);

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
                Uri fileUri = new(filePaths[++imageIndex]);

                this.Title = "Comic Viewer: Folder [" + Path.GetDirectoryName(filePaths[imageIndex]) +
                    "] [" + Path.GetFileName(filePaths[imageIndex]) + "]" + vertical[imageIndex];
                imagePicture.Source = new BitmapImage(fileUri);
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
                Uri fileUri = new(filePaths[--imageIndex]);

                this.Title = "Comic Viewer: Folder [" + Path.GetDirectoryName(filePaths[imageIndex]) +
                    "] [" + Path.GetFileName(filePaths[imageIndex]) + "]" + vertical[imageIndex];
                imagePicture.Source = new BitmapImage(fileUri);
                imageContainer.ScrollToVerticalOffset(vertical[imageIndex]);

            }

        }
    }
}
