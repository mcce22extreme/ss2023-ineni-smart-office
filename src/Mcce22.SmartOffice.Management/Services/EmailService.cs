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
        private readonly SmptConfiguration _config;

        public EmailService(SmptConfiguration config)
        {
            _config = config;
        }

        public async Task SendMail(User user, Workspace workspace)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_config.SenderName, _config.UserName));
                message.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email));
                message.Subject = TITLE;

                message.Body = new TextPart("plain") { Text = "Your workspace '' is ready for activation!" };

                using var client = new SmtpClient();

                await client.ConnectAsync(_config.Host, _config.Port, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_config.UserName, _config.Password);

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
