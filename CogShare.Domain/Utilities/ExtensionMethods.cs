using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogShare.Domain.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsNullWhiteSpaceOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target) || string.IsNullOrWhiteSpace(target);
        }

        public static string ConvertToBase64(this string target)
        {
            if (target.IsNullWhiteSpaceOrEmpty())
            {
                return string.Empty;
            }
            else
            {
                byte[] textAsBytes = Encoding.UTF8.GetBytes(target);
                return Convert.ToBase64String(textAsBytes);
            }
        }
    }
}
