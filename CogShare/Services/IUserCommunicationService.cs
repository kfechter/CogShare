using CogShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogShare.Services
{
    public interface IUserCommunicationService
    {
        Task SendUserNotification(string toEmail, string subject, string messageBody, string fromUser);

        Task SendFriendRequest(string toEmail, string fromUser);

        Task AcceptFriendRequest(Friendship friendShip);

        Task SendItemRequest(Request request);

        Task AcceptItemRequest(Request request);

        // Task SendUserPush(string pushMessage, string userNotificationsId); // Possibly this
    }
}
