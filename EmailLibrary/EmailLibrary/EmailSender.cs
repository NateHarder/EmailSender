using System;
using System.Data.SqlClient;
using MimeKit;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace EmailLibrary
{
    public class EmailSender
    {


        public static void Send(String recipient, String subject, String body) {

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();

            var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            var message = new MimeMessage();
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(config["Smtp:Username"], config["Smtp:Email"]));
            mailMessage.To.Add(new MailboxAddress("", recipient));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = body
            };

           
            smtpClient.Connect(config["Smtp:Host"], Int32.Parse(config["Smtp:Port"]), true);
            smtpClient.Authenticate(config["Smtp:Email"], config["Smtp:Password"]);

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


            using (SqlConnection connection = new SqlConnection(config["ConnectionStrings:EmailConnection"]))
            {
                string query = "INSERT INTO dbo.Emails (Sender,Recipient,Subject,Body,Date,Sent)"+
                    " VALUES ('"+ config["Smtp:Email"] + "','"+ recipient + "','"+ mailMessage.Subject +"','"+ body +"','"+ DateTime.Now +"',"+sent+")";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }
    }
}
