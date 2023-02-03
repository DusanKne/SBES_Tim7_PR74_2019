using System;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;

namespace SecurityManager
{
    public class EncryptionManager
    {
        public static string EncryptMessage(string message)
        {

            if (string.IsNullOrEmpty(message))
            {
                throw new FaultException("No message to encrypt");
            }
            byte[] array;
            using (DES des = DES.Create())
            {
                des.GenerateKey();
                des.GenerateIV();
                des.Mode = CipherMode.CBC;
                KeyLoader.Store(des.Key, true);
                KeyLoader.Store(des.IV, false);
                ICryptoTransform encryptor = des.CreateEncryptor(des.Key, des.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(message);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);

        }

        public static string DecryptMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new FaultException("No message to decrypt");
            }
            byte[] buffer = Convert.FromBase64String(message);
            string plaintext = "";
            using (DES des = DES.Create())
            {
                des.Mode = CipherMode.CBC;
                des.Key = KeyLoader.Load(true);
                des.IV = KeyLoader.Load(false);
                ICryptoTransform decryptor = des.CreateDecryptor(des.Key, des.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            plaintext = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

      
    }
}
