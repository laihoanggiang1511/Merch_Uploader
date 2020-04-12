using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Miscellaneous
{
    public class Crypt
    {
        const string ENCRYPT_KEY = "kXp2s5v8y/B?E(H+MbQeThWmZq3t6w9z";
        public static string Encrypt(string toEncrypt, bool useHashing, string ENCRYPT_KEY= ENCRYPT_KEY)
        {

            if (string.IsNullOrEmpty(toEncrypt))
                return string.Empty;

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            string key = ENCRYPT_KEY;
            //IF HASHING USE GET HASH CODE REGARDS TO YOUR KEY
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //ALWAYS RELEASE THE RESOURCES AND FLUSH DATA
                // OF THE CRYPTOGRAPHIC SERVICE PROVIDE. BEST PRACTICE
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //SET THE SECRET KEY FOR THE TRIPLEDES ALGORITHM
            tdes.Key = keyArray;
            //MODE OF OPERATION. THERE ARE OTHER 4 MODES.
            //WE CHOOSE ECB(ELECTRONIC CODE BOOK)
            tdes.Mode = CipherMode.ECB;
            //PADDING MODE(IF ANY EXTRA BYTE ADDED)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //RELEASE RESOURCES HELD BY TRIPLEDES ENCRYPTOR
            tdes.Clear();
            //RETURN THE ENCRYPTED DATA INTO UNREADABLE STRING FORMAT
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing, string ENCRYPT_KEY = ENCRYPT_KEY)
        {

            if (string.IsNullOrEmpty(cipherString))
                return string.Empty;
            byte[] keyArray;
            //GET THE BYTE CODE OF THE STRING
            try
            {
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                string key = ENCRYPT_KEY;

                if (useHashing)
                {
                    //IF HASHING WAS USED GET THE HASH CODE WITH REGARDS TO YOUR KEY
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //RELEASE ANY RESOURCE HELD BY THE MD5CRYPTOSERVICEPROVIDER
                    hashmd5.Clear();
                }
                else
                {
                    //IF HASHING WAS NOT IMPLEMENTED GET THE BYTE CODE OF THE KEY
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //SET THE SECRET KEY FOR THE TRIPLEDES ALGORITHM
                tdes.Key = keyArray;
                //MODE OF OPERATION. THERE ARE OTHER 4 MODES. 
                //WE CHOOSE ECB(ELECTRONIC CODE BOOK)

                tdes.Mode = CipherMode.ECB;
                //PADDING MODE(IF ANY EXTRA BYTE ADDED)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //RELEASE RESOURCES HELD BY TRIPLEDES ENCRYPTOR                
                tdes.Clear();
                //RETURN THE CLEAR DECRYPTED TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (System.Exception)
            {
                return cipherString;
            }
        }
    }
}
