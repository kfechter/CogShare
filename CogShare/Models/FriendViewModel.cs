using CogShare.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CogShare.Models
{
    public class FriendViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public List<Friendship> Friendships { get; set; }

        public string OwnerId { get; set; }

        public FriendViewModel()
        {
            Friendships = new List<Friendship>();
            ErrorState = false;
            StatusMessage = string.Empty;
            OwnerId = string.Empty;
        }

        public FriendViewModel(List<Friendship> friendships, string ownerId)
        {
            ErrorState = false;
            StatusMessage = string.Empty;
            Friendships = friendships.Where(x => x.Accepted).ToList();
            OwnerId = ownerId;
        }

        public FriendViewModel(bool errorState, string statusMessage, List<Friendship> friendships, string ownerId)
        {
            ErrorState = errorState;
            StatusMessage = statusMessage;
            Friendships = friendships.Where(x => x.Accepted).ToList();
            OwnerId = ownerId;
        }
    }
}
