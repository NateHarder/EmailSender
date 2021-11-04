
using System.Windows;
using EmailLibrary;

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
        }
    }
}
