using CogShare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogShare.Models
{
    public class UserSearchViewModel
    {
        public bool ErrorState { get; set; }

        public string StatusMessage { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public UserSearchViewModel()
        {
            ErrorState = false;
            StatusMessage = string.Empty;
            Users = new List<ApplicationUser>();
        }

        public UserSearchViewModel(bool errorState, string statusMessage)
        {
            ErrorState = errorState;
            StatusMessage = statusMessage;
            Users = new List<ApplicationUser>();
        }

        public UserSearchViewModel(List<ApplicationUser> users)
        {
            ErrorState = false;
            StatusMessage = string.Empty;
            Users = users;
        }
    }
}
