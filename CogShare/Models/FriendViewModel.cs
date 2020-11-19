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

        public List<Friendship> Requests { get; set; }

        public FriendViewModel()
        {
            Friendships = new List<Friendship>();
            Requests = new List<Friendship>();
            ErrorState = false;
            StatusMessage = string.Empty;
        }

        public FriendViewModel(List<Friendship> friendships)
        {
            ErrorState = false;
            StatusMessage = string.Empty;
            Requests = friendships.Where(x => !x.Accepted).ToList();
            Friendships = friendships.Where(x => x.Accepted).ToList();
        }

        public FriendViewModel(bool errorState, string statusMessage, List<Friendship> friendships)
        {
            ErrorState = errorState;
            StatusMessage = statusMessage;
            Requests = friendships.Where(x => !x.Accepted).ToList();
            Friendships = friendships.Where(x => x.Accepted).ToList();
        }
    }
}
