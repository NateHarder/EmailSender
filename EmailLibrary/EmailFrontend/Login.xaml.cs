using System;
using System.Windows;
using EmailLibrary;

namespace EmailFrontend
{
    /// <summary>  
    /// Interaction logic for MainWindow.xaml  
    /// </summary>   
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            int result = EmailSender.Login(textBoxEmail.Text, passwordBox1.Password);
            if (result == 1)
            {
                EmailFrontend.EmailLogPage LogWindow = new EmailFrontend.EmailLogPage(textBoxEmail.Text, passwordBox1.Password);
                LogWindow.Show();
                this.Close();
            }
            else
            {
                textBoxEmail.Text = string.Empty;
                passwordBox1.Password = string.Empty;
                MessageBoxResult error = MessageBox.Show("Email or Password Invalid.");
            }
        }
    }
}