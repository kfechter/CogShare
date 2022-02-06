using Microsoft.AspNetCore.Identity;
using CogShare.Domain.Utilities;

namespace CogShare.Domain.Entities
{
    public class CogShareUser : IdentityUser
    {
        public string? PersonalAPIKey { get; set; }

        public string IdentityQR { 
            get
            {
                return PersonalAPIKey!.ConvertToBase64();
            } 
        }
    }
}
