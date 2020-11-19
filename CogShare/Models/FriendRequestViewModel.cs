using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogShare.Models
{
    public class FriendRequestViewModel
    {
        public string RequestMessage { get; set; }

        public string User1Id { get; set; }

        public string User2Id { get; set; }
    }
}
