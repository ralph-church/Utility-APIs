using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace repair.service.shared.shared
{
    public class CryptoHelper
    {

        // https://docs.microsoft.com/en-us/dotnet/standard/security/encrypting-dat
        public static async ValueTask<string> EncryptAsync(string cleartext, string secretKey)
        {
            MemoryStream outStream = new MemoryStream(200);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(secretKey);

                /* make a new init. vector each time we call this method */
                byte[] iv = aes.IV;

                /* write the IV into the encrypted data */
                await outStream.WriteAsync(iv, 0, iv.Length);

                /* write out the rest of the input cleartext into the crypto stream */
                using (CryptoStream stream = new CryptoStream(outStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using StreamWriter encryptWriter = new(stream);
                    await encryptWriter.WriteAsync(cleartext);
                }
            }

            byte[] result = outStream.ToArray();
            outStream.Close();
            return Convert.ToBase64String(result);
        }

        // https://docs.microsoft.com/en-us/dotnet/standard/security/decrypting-data
        public static async ValueTask<string> DecryptAsync(string encrypted, string secretKey)
        {

            MemoryStream inStream = new MemoryStream(Convert.FromBase64String(encrypted));
            string result = "";

            using (Aes aes = Aes.Create())
            {
                /* read initialization vector from the beginning of the encrypted bytes */
                int ivLength = aes.IV.Length;
                byte[] iv = new byte[ivLength];
                int bytesRead = 0;
                int bytesRemaining = ivLength;

                while (bytesRemaining > 0)
                {
                    int bytes = await inStream.ReadAsync(iv, bytesRead, bytesRemaining);
                    if (bytes == 0) break;

                    bytesRead += bytes;
                    bytesRemaining -= bytes;
                }

                /* now read the rest of the stream to get the cleartext */
                using CryptoStream stream = new CryptoStream(inStream, aes.CreateDecryptor(Encoding.UTF8.GetBytes(secretKey), iv), CryptoStreamMode.Read);
                using StreamReader decryptReader = new(stream);
                result = await decryptReader.ReadToEndAsync();
            }

            inStream.Close();
            return result;
        }
    }
}

