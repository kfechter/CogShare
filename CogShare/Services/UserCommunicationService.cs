using CogShare.Domain.Entities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace CogShare.Services
{
    public class UserCommunicationService : IUserCommunicationService
    {
        public EmailSenderOptions Options { get; }

        public UserCommunicationService(IOptions<EmailSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Task AcceptFriendRequest(Friendship friendship)
        {
            string messageSubject = $"{friendship.User2.Email} has accepted your request";
            string messageBody = $"The request you sent to {friendship.User2.Email} has been accepted";

            return SendUserNotification(friendship.User2.Email, messageSubject, messageBody, "Cogshare Admin");
        }

        public Task SendItemRequest(Request request)
        {
            string messageSubject = request.RequestMessage;
            string messageBody = $"{request.Requestor.Email} has requested an item from you, log in and go to requests to accept or decline";

            return SendUserNotification(request.Requestee.Email, messageSubject, messageBody, "CogShare Admin");
        }

        public Task AcceptItemRequest(Request request)
        {
            string messageSubject = $"{request.Requestee.Email} has accepted your request to borrow an item";
            string messageBody = $"Your request to borrow {request.RequestedItem.DisplayName} has been accepted.";
            return SendUserNotification(request.Requestor.Email, messageSubject, messageBody, "CogShare Admin");

        }

        public Task SendFriendRequest(string toEmail, string fromUser)
        {
            string messageSubject = $"Friend request from {fromUser}";
            string messageBody = $"{fromUser} has sent you a friend request, log into CogShare to accept or decline this request.";

            return SendUserNotification(toEmail, messageSubject, messageBody, fromUser);
        }

        public Task SendUserNotification(string toEmail, string subject, string messageBody, string fromUser)
        {
            MimeMessage cogshareUserMessage = new MimeMessage();
            MailboxAddress fromAddress = new MailboxAddress(fromUser, "kfechterbookstack@gmail.com");

            cogshareUserMessage.From.Add(fromAddress);
            MailboxAddress toAddress = MailboxAddress.Parse(toEmail);

            cogshareUserMessage.To.Add(toAddress);
            cogshareUserMessage.Subject = subject;

            BodyBuilder messageBodyBuilder = new BodyBuilder
            {
                HtmlBody = messageBody
            };

            cogshareUserMessage.Body = messageBodyBuilder.ToMessageBody();
            SmtpClient cogshareClient = new SmtpClient();
            cogshareClient.Connect(Options.SMTPServer, Options.SMTPPort, MailKit.Security.SecureSocketOptions.StartTls);
            cogshareClient.Authenticate(Options.SMTPUser, Options.SMTPPassword);
            cogshareClient.Send(cogshareUserMessage);
            return cogshareClient.DisconnectAsync(true);
        }
    }
}
