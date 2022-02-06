using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CogShare.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSenderOptions Options { get; }

        public EmailSender(IOptions<EmailSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpServer = Options.SMTPServer ?? "";
            var smtpUser = Options.SMTPUser ?? "";
            var smtpPassword = Options.SMTPPassword ?? "";

            return Execute(email, subject, message, smtpServer, Options.SMTPPort, smtpUser, smtpPassword);
        }

        public Task SendUserEmailAsync(string email, string subject, string message, string emailUser)
        {
            var smtpServer = Options.SMTPServer ?? "";
            var smtpUser = Options.SMTPUser ?? "";
            var smtpPassword = Options.SMTPPassword ?? "";

            return Execute(email, subject, message, smtpServer, Options.SMTPPort, smtpUser, smtpPassword, emailUser);
        }

        public Task Execute(string email, string subject, string message, string smtpServer, int smtpPort, string smtpUser, string smtpPass, string emailUser = "CogShare Admin")
        {
            MimeMessage cogshareMessage = new MimeMessage();
            MailboxAddress fromAddress = new MailboxAddress(emailUser, "kfechterbookstack@gmail.com");
            cogshareMessage.From.Add(fromAddress);

            MailboxAddress toAddress = MailboxAddress.Parse(email);
            cogshareMessage.To.Add(toAddress);

            cogshareMessage.Subject = subject;

            BodyBuilder messageBodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };

            cogshareMessage.Body = messageBodyBuilder.ToMessageBody();

            SmtpClient cogshareClient = new SmtpClient();
            cogshareClient.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            cogshareClient.Authenticate(smtpUser, smtpPass);

            cogshareClient.Send(cogshareMessage);
            return cogshareClient.DisconnectAsync(true);
        }
    }
}
