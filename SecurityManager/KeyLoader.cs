using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class KeyLoader
    {
        public static void Store(byte[] secretKey, bool isKey)
        {
            string outFile = "";
            if (isKey)
            {
                outFile = "../../../Server/bin/Debug/secretkey";
            }
            else
            {
                outFile = "../../../Server/bin/Debug/iv";

            }
            try
            {
                File.WriteAllBytes(outFile, secretKey.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("SecretKeys.StoreKey:: ERROR {0}", e.Message);
            }

        }
        public static byte[] Load(bool isKey)
        {
            string inFile = "";

            if (isKey)
            {
                inFile = "../../../Server/bin/Debug/secretkey";
            }
            else
            {
                inFile = "../../../Server/bin/Debug/iv";

            }

            byte[] buffer = new byte[8];

            try
            {
                buffer = File.ReadAllBytes(inFile);

            }
            catch (Exception e)
            {
                Console.WriteLine("SecretKeys.LoadKey:: ERROR {0}", e.Message);
            }


            return buffer;
        }
    }
}
