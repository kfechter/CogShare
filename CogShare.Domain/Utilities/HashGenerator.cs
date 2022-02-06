using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CogShare.Domain.Utilities
{
    // Generates API Tokens for users to build their own apps.
    public static class HashGenerator
    {
        public static string GenerateHash()
        {
            var guidSeed = Guid.NewGuid().ToString();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(guidSeed));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }
}
