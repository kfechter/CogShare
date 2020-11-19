using CogShare.Utilities;
using Microsoft.AspNetCore.Identity;

namespace CogShare.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool PublicBorrowerProfile { get; set; }

        public string IdentityQR
        {
            get
            {
                var userIdentityString = $"{this.Id}:{this.NormalizedUserName}";
                return userIdentityString.ConvertToBase64();
            }
        }
    }
}
