
using System.Windows;
using EmailLibrary;
using System.Threading;

namespace EmailFrontend
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmailSender.Send(Recipient.Text, Subject.Text, Body.Text);
            Recipient.Text = string.Empty;
            Subject.Text = string.Empty;
            Body.Text = string.Empty;
        }
    }
}
