using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PictureNavigator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _myMainWindowViewModel = new MainWindowViewModel();       
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _myMainWindowViewModel;
        }        
    }
}
