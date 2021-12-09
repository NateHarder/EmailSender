
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace EmailFrontend
{
    /// <summary>
    /// Interaction logic for EmailLogPage.xaml
    /// </summary>
    public partial class EmailLogPage : Window
    {
        public string email
        { get; set; }
        public string password
        { get; set; }

        public EmailLogPage(string email, string password)
        {
            InitializeComponent();
            this.email = email;
            this.password = password;
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = System.IO.Path.GetDirectoryName(executable)[0..^38];
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            config["ConnectionStrings:EmailConnection"] = config["ConnectionStrings:EmailConnection"].Insert(52, path);
            using (SqlConnection connection = new SqlConnection(config["ConnectionStrings:EmailConnection"]))
            {
                string query = "SELECT Sender, Recipient, Subject, Body, Date, Sent FROM dbo.Emails WHERE Sender = '" + email + "'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader result = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(result);
                    dataGrid1.DataContext = table.DefaultView;

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(this, email, password);
            mainWindow.Show();
            this.Close();
        }

    }
}
