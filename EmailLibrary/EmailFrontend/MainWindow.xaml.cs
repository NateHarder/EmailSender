
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
        public EmailLogPage emailLog;
        public MainWindow(EmailLogPage log, string email, string password)
        {
            InitializeComponent();
            emailLog = log;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmailSender.Send(emailLog.email, emailLog.password, Recipient.Text, Subject.Text, Body.Text);
            EmailLogPage logWindow = new EmailLogPage(emailLog.email, emailLog.password);
            logWindow.Show();
            this.Close();
        }
    }
}
