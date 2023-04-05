using MailKit.Net.Smtp;
using MailKit.Security;
using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Management.Entities;
using MimeKit;
using Serilog;

namespace Mcce22.SmartOffice.Management.Services
{
    public interface IEmailService
    {
        Task SendMail(User user, Workspace workspace);
    }

    public class EmailService : IEmailService
    {
        private const string TITLE = "[MCCE22-Smart-Office] Workspace ready for activation!";

        public async Task SendMail(User user, Workspace workspace)
        {
            try
            {
                var smtpConfig = AppSettings.Current.SmptConfiguration;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(smtpConfig.SenderName, smtpConfig.UserName));
                message.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email));
                message.Subject = TITLE;

                message.Body = new TextPart("plain") { Text = "Your workspace '' is ready for activation!" };

                using var client = new SmtpClient();

                await client.ConnectAsync(smtpConfig.Host, smtpConfig.Port, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(smtpConfig.UserName, smtpConfig.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
    }
}
