using CogShare.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CogShare.Models
{
    public class FriendRequestViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public string OwnerId { get; set; }

        public List<Friendship> Friendships { get; set; }

        public List<Friendship> SentRequests  
        { 
            get
            {
                return Friendships.Where(x => x.User1Id == OwnerId).ToList();
            }
        }

        public List<Friendship> ReceivedRequests
        { 
            get
            {
                return Friendships.Where(x => x.User2Id == OwnerId).ToList();
            }
        }

        public FriendRequestViewModel()
        {
            Friendships = new List<Friendship>();
            ErrorState = false;
            StatusMessage = string.Empty;
            OwnerId = string.Empty;
        }

        public FriendRequestViewModel(List<Friendship> friendships, string ownerId)
        {
            ErrorState = false;
            StatusMessage = string.Empty;
            Friendships = friendships.Where(x => !x.Accepted).ToList();
            OwnerId = ownerId;
        }

        public FriendRequestViewModel(bool errorState, string statusMessage, List<Friendship> friendships, string ownerId)
        {
            ErrorState = errorState;
            StatusMessage = statusMessage;
            Friendships = friendships.Where(x => !x.Accepted).ToList();
            OwnerId = ownerId;
        }
    }
}
