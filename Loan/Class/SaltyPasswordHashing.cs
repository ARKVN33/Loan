using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Loan.Class
{
    public class SaltyPasswordHashing
    {
        public int MaxHashSize { get; set; }

        public int SaltSize { get; set; }

        public string ComputeHash(string myPassword, byte[] saltBytes = null)
        {

            try
            {
                // randomly create the Salt
                // unless it has been passed in
                if (saltBytes == null)
                {
                    saltBytes = new byte[SaltSize];
                    var rng = new RNGCryptoServiceProvider();
                    rng.GetNonZeroBytes(saltBytes);
                }

                // Concat the Salt and Password
                var myPasswordBytes = Encoding.UTF8.GetBytes(myPassword);
                var myPasswordWithSaltBytes =
                    saltBytes.Concat(myPasswordBytes).ToArray();

                // Hash the Salt + Password
                HashAlgorithm hash = new SHA512Managed();
                var hashBytes = hash.ComputeHash(myPasswordWithSaltBytes);
                var hashWithSaltBytes = saltBytes.Concat(hashBytes).ToArray();

                // Convert to BASE64 and cut off after the maximum size
                var hashValue = Convert.ToBase64String(hashWithSaltBytes);
                return hashValue.Substring(0, MaxHashSize);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool VerifyHash(string myPassword, string hashValue)
        {

            try
            {
                // Convert base64-encoded hash value into a byte array.
                var hashWithSaltBytes = Convert.FromBase64String(hashValue);

                // Copy salt from the beginning of the hash to the new array.
                var saltBytes = hashWithSaltBytes.Take(SaltSize).ToArray();

                // Compute a new hash string.
                var expectedHashString =
                    ComputeHash(myPassword, saltBytes);

                // If the computed hash matches the specified hash,
                // the myPassword value must be correct.
                return hashValue == expectedHashString;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
