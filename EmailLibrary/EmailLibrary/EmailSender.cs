using System;
using System.Data.SqlClient;
using MimeKit;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace EmailLibrary
{
    public static class EmailSender
    {
        public static MailKit.Net.Smtp.SmtpClient smtpClient;

        public static int Login(string username, string password) 
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            smtpClient = new MailKit.Net.Smtp.SmtpClient();
            smtpClient.Connect(config["Smtp:Host"], Int32.Parse(config["Smtp:Port"]), true);
            try
            {
                smtpClient.Authenticate(username, password);
            }
            catch 
            {
                return 0;
            }
            return 1;
            
        }

        public static void Send( string email, string password, string recipient, string subject, string body) 
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = System.IO.Path.GetDirectoryName(executable);
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            config["ConnectionStrings:EmailConnection"] = config["ConnectionStrings:EmailConnection"].Insert(52, path.Substring(0, path.Length - 38));
            var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            var message = new MimeMessage();
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(email, password));
            mailMessage.To.Add(new MailboxAddress("", recipient));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = body
            };

           
            smtpClient.Connect(config["Smtp:Host"], Int32.Parse(config["Smtp:Port"]), true);
            smtpClient.Authenticate(email, password);

            int sent = 1;
            int attempts = 3;
            while (sent == 1)
            {
                try
                {
                    smtpClient.Send(mailMessage);
                    break;
                }
                catch
                {
                    if (--attempts == 0) 
                    {
                        sent = 0;
                        throw; 
                    }
                    else Thread.Sleep(10000);
                }
            }

            smtpClient.Disconnect(true);

            string dir = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", dir);
            using (SqlConnection connection = new SqlConnection(config["ConnectionStrings:EmailConnection"]))
            {
                string query = "INSERT INTO dbo.Emails (Sender,Recipient,Subject,Body,Date,Sent)"+
                    " VALUES ('"+ email + "','"+ recipient + "','"+ mailMessage.Subject +"','"+ body +"','"+ DateTime.Now +"',"+sent+")";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }
    }
}
