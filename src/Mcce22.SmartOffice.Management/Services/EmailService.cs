using Mcce22.SmartOffice.Management.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Mcce22.SmartOffice.Management.Services
{
    public interface IEmailService
    {
        Task SendMail(User user, Workspace workspace);
    }

    public class EmailService : IEmailService
    {
        private const string HOST = "smtp.gmail.com";
        private const int PORT = 587;        
        private const string USERNAME = "";
        private const string PASSWORD = "";
        private const string SENDERNAME = "MCCE22 Smart Office";
        private const string SENDERADDRESS = "mcce22smartoffice@gmail.com";
        private const string TITLE = "[MCCE22-Smart-Office] Workspace ready for activation!";

        public async Task SendMail(User user, Workspace workspace)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(SENDERNAME, SENDERADDRESS));
                message.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email));
                message.Subject = TITLE;

                message.Body = new TextPart("plain") { Text = "Your workspace '' is ready for activation!" };

                using var client = new SmtpClient();

                await client.ConnectAsync(HOST, PORT, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(USERNAME, PASSWORD);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
