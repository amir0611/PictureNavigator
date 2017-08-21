using System;
using System.Windows;

namespace PictureNavigator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _myMainWindowViewModel = new MainWindowViewModel();
        private const string browseTextBoxMessage = "Select the photo by clicking Browse button";
        private const string destinationPathTextBoxMessage = "Select the detination folder to copy your favourite pics";
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _myMainWindowViewModel;

            myBrowseTextBox.Text = browseTextBoxMessage;
            myBrowseTextBox.LostFocus += AddBrowseText;
            myBrowseTextBox.GotFocus += RemoveBrowseText;

            myDestinationPathTextBox.Text = destinationPathTextBoxMessage;
            myDestinationPathTextBox.LostFocus += AddDestinationText;
            myDestinationPathTextBox.GotFocus += RemoveDestinationText;           
        }

        public void AddBrowseText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(myBrowseTextBox.Text))
            {
                myBrowseTextBox.Text = browseTextBoxMessage;
            }
        }
        public void RemoveBrowseText(object sender, EventArgs e)
        {
            if (myBrowseTextBox.Text.Equals(browseTextBoxMessage))
            {
                myBrowseTextBox.Text = string.Empty;
            }
        }

        public void AddDestinationText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(myDestinationPathTextBox.Text))
            {
                myDestinationPathTextBox.Text = destinationPathTextBoxMessage;
            }
        }
        public void RemoveDestinationText(object sender, EventArgs e)
        {
            if (myDestinationPathTextBox.Text.Equals(destinationPathTextBoxMessage))
            {
                myDestinationPathTextBox.Text = string.Empty;
            }
        }
    }
}
