﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureNavigator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<string> myAllImageInSelectedDirectory;
        private DelegateCommand myNextCommand;
        private DelegateCommand myPreviousCommand; 
        private DelegateCommand myBrowseCommand;
        private DelegateCommand mySelectFolderCommand;
        private DelegateCommand myCopyCommand;
        private int currentIndex = -1;
        private ImageSource myImageSource;
        private string mySourceDirectoryPath = "Select the photo by clicking Browse button";
        private string myDestinationPath = "Select the detination folder to copy your favourite pics";
    

        public MainWindowViewModel()
        {
            myAllImageInSelectedDirectory = Enumerable.Empty<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                NotifyPropertyChanged(nameof(SourceDirectoryPath));
            }
        }

        public string DestinationPath
        {
            get { return myDestinationPath; }
            set
            {
                myDestinationPath = value;
                NotifyPropertyChanged(nameof(DestinationPath));
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                return myBrowseCommand ??
                       (myBrowseCommand =
                           new DelegateCommand(Browse));
            }
        }

        public ICommand SelectFolderCommand
        {
            get
            {
                return mySelectFolderCommand ??
                       (mySelectFolderCommand =
                           new DelegateCommand(SelectDestinationFolder));
            }
        }

        public ICommand NextCommand
        {
            get
            {
                return myNextCommand ??
                       (myNextCommand =
                           new DelegateCommand(MoveNext, CanMoveNext));
            }
        }

        public ICommand PreviousCommand
        {
            get
            {
                return myPreviousCommand ??
                       (myPreviousCommand =
                           new DelegateCommand(MovePrevious, CanMovePrevious));
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                return myCopyCommand ??
                       (myCopyCommand =
                           new DelegateCommand(Copy, CanCopy));
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
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
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
                SourceDirectoryPath = directoryPath;

                SetImageSource(selectedFileName);
                currentIndex++;

                myNextCommand.RaiseCanExecuteChanged();
                myPreviousCommand.RaiseCanExecuteChanged();
                myCopyCommand.RaiseCanExecuteChanged();
            }
        }

        private void Copy()
        {
            var allFiles = myAllImageInSelectedDirectory.ToArray();

            if (!allFiles.Any())
            {
                return;
            }

            var selectedFileFullPath = allFiles[currentIndex];
            var selectedFile = Path.GetFileName(selectedFileFullPath);

            File.Copy(selectedFileFullPath, DestinationPath + @"\" + selectedFile,true);

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

        private void SelectDestinationFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select the directory that you want to dump to selected pics.",
                ShowNewFolderButton = true
                //RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DestinationPath = folderBrowserDialog.SelectedPath;

                myCopyCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanMoveNext()
        {
            return myAllImageInSelectedDirectory.Any() && currentIndex >= 0 && currentIndex + 1 != myAllImageInSelectedDirectory.Count();
        }

        private bool CanMovePrevious()
        {
            return myAllImageInSelectedDirectory.Any() && currentIndex > 0 && currentIndex < myAllImageInSelectedDirectory.Count();
        }

        private bool CanCopy()
        {
            return myAllImageInSelectedDirectory.Any() && !string.IsNullOrEmpty(DestinationPath) && Directory.Exists(DestinationPath);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
