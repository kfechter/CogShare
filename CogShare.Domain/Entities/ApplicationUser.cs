using CogShare.Utilities;
using Microsoft.AspNetCore.Identity;

namespace CogShare.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
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
