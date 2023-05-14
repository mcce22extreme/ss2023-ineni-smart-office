using System.Reflection;
using MailKit.Net.Smtp;
using MailKit.Security;
using Mcce22.SmartOffice.Core.Providers;
using Mcce22.SmartOffice.Notifications.Entities;
using MimeKit;
using Serilog;

namespace Mcce22.SmartOffice.Notifications.Services
{
    public interface IEmailService
    {
        Task SendMail(Booking booking);
    }

    public class EmailService : IEmailService
    {
        private const string TITLE = "[MCCE22-Smart-Office] Workspace ready for activation!";
        private const string PLACEHOLDER_FIRSTNAME = "{FIRSTNAME}";
        private const string PLACEHOLDER_LASTNAME = "{LASTNAME}";
        private const string PLACEHOLDER_WORKSPACENUMBER = "{WORKSPACENUMBER}";
        private const string PLACEHOLDER_ROOMNUMBER = "{ROOMNUMBER}";
        private const string PLACEHOLDER_STARTTIME = "{STARTTIME}";
        private const string PLACEHOLDER_ENDTIME = "{ENDTIME}";
        private const string PLACEHOLDER_LINK = "{LINK}";
        private const string PLACEHOLDER_TIMESTAMP = "{TIMESTAMP}";

        private readonly IAppConfigProvider _appConfigProvider;

        public EmailService(IAppConfigProvider appConfigProvider)
        {
            _appConfigProvider = appConfigProvider;
        }

        public async Task SendMail(Booking booking)
        {
            try
            {
                var appConfig = await _appConfigProvider.GetAppConfig();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(appConfig.SmtpSenderName, appConfig.SmtpUserName));
                message.To.Add(new MailboxAddress($"{booking.FirstName} {booking.LastName}", booking.Email));
                message.Subject = TITLE;

                var content = await LoadTemplate();

                content = content
                    .Replace(PLACEHOLDER_FIRSTNAME, booking.FirstName)
                    .Replace(PLACEHOLDER_LASTNAME, booking.LastName)
                    .Replace(PLACEHOLDER_WORKSPACENUMBER, booking.WorkspaceNumber)
                    .Replace(PLACEHOLDER_ROOMNUMBER, booking.RoomNumber)
                    .Replace(PLACEHOLDER_STARTTIME, $"{booking.StartDate.ToShortDateString()} {booking.StartTime.ToShortTimeString()}")
                    .Replace(PLACEHOLDER_ENDTIME, $"{booking.EndDate.ToShortDateString()} {booking.EndTime.ToShortTimeString()}")
                    .Replace(PLACEHOLDER_LINK, $"{appConfig.ActivatorEndpointAddress}/activate?activationcode={booking.ActivationCode}")
                    .Replace(PLACEHOLDER_TIMESTAMP, DateTime.Now.ToString());

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = content;

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                await client.ConnectAsync(appConfig.SmtpHost, appConfig.SmtpPort, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(appConfig.SmtpUserName, appConfig.SmtpPassword);

                Log.Debug($"Sending invitation email to '{booking.Email}'...");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Log.Debug($"Successfully sent invitation email to '{booking.Email}'.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        private async Task<string> LoadTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Mcce22.SmartOffice.Notifications.Templates.email-template.txt";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();

        }
    }
}
