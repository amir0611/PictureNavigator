using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureNavigator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<string> myAllImageInSelectedDirectory;
        private Command myNextCommand;
        private Command myPreviousCommand; 
        private Command myBrowseCommand; 
        private int currentIndex = -1;
        private ImageSource myImageSource;
        private string mySourceDirectoryPath;

        public MainWindowViewModel()
        {
            myAllImageInSelectedDirectory = Enumerable.Empty<string>();
        }

        public ImageSource ImageSource
        {
            get { return myImageSource; }
            set
            {
                myImageSource = value;
                NotifyPropertyChanged(nameof(ImageSource));
            }
        }

        public string SourceDirectoryPath
        {
            get { return mySourceDirectoryPath; }
            set
            {
                mySourceDirectoryPath = value;
                NotifyPropertyChanged("SourceDirectory");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand NextCommand
        {
            get
            {
                return myNextCommand ??
                       (myNextCommand =
                           new Command(MoveNext, CanMoveNext));
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return myPreviousCommand ??
                       (myPreviousCommand =
                           new Command(MovePrevious, CanMovePrevious));
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                return myBrowseCommand ??
                       (myBrowseCommand =
                           new Command(Browse));
            }
        }

        private void MovePrevious()
        {
            var navigated = NavigateImage(currentIndex - 1);
            if (navigated)
            {
                currentIndex--;
                myNextCommand.RaiseCanExecuteChanged();
                myPreviousCommand.RaiseCanExecuteChanged();
            }
        }       

        private void MoveNext()
        {
            var navigated = NavigateImage(currentIndex + 1);
            if (navigated)
            {
                currentIndex++;
                myNextCommand.RaiseCanExecuteChanged();
                myPreviousCommand.RaiseCanExecuteChanged();
            }
        }

        public void Browse()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (dlg.ShowDialog().Value)
            {
                var selectedFileName = dlg.FileName;
                var directoryPath = Path.GetDirectoryName(selectedFileName);

                myAllImageInSelectedDirectory = Directory.EnumerateFiles(directoryPath).Where(file => Path.GetExtension(file).ToLower().Equals(".jpg") || Path.GetExtension(file).ToLower().Equals(".png"));
                mySourceDirectoryPath = directoryPath;

                SetImageSource(selectedFileName);
                currentIndex++;

                myNextCommand.RaiseCanExecuteChanged();
                myPreviousCommand.RaiseCanExecuteChanged();
            }
        }

        private bool NavigateImage(int index)
        {
            bool navigated = false;
            var allFiles = myAllImageInSelectedDirectory.ToArray();

            if (!allFiles.Any())
            {
                navigated = false;
            }

            var selectedFileName = allFiles[index];
            if (!string.IsNullOrEmpty(selectedFileName))
            {
                SetImageSource(selectedFileName);
                navigated = true;
            }

            return navigated;
        }

        private void SetImageSource(string fileName)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileName);
            bitmap.EndInit();
            ImageSource = bitmap;
        }

        private bool CanMoveNext()
        {
            return myAllImageInSelectedDirectory.Any() && currentIndex >= 0 && currentIndex + 1 != myAllImageInSelectedDirectory.Count();
        }

        private bool CanMovePrevious()
        {
            return myAllImageInSelectedDirectory.Any() && currentIndex > 0 && currentIndex < myAllImageInSelectedDirectory.Count();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
