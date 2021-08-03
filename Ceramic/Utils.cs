using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ceramic
{
    class Utils
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public static int GetRandomInt(int max,int min=5)
        {
            return random.Next(min, max);
        }
        public static string RandomString(int length = 0)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm_1234567890";

            if (length > 0)
            {
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(chars, GetRandomInt(10)).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        public static string RandomSpecialChars(int length = 0)
        {
            const string chars = "!@#$%^&*()_+=-{}][|`~";

            if (length > 0)
            {
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(chars, GetRandomInt(10)).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        public static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static string AddJunkToString(string originalString, string JunkToAdd = "", int StringLocationToStartAddingJunk=1)
        {
            if (JunkToAdd == "")
            {
                JunkToAdd = RandomSpecialChars(GetRandomInt(20,2));
            }
            Console.WriteLine("[+] Junk string added is:" + JunkToAdd);
            Random rand = new Random();
            int numberOfTimeToAdd=rand.Next(0, originalString.Length);

            for (int x=0; x<numberOfTimeToAdd;++x)
            {
                originalString.Insert(x, JunkToAdd);
            }
            Console.WriteLine(" [+] Junk string added is:" + JunkToAdd);

            Console.WriteLine(" [+] Junk string added " + numberOfTimeToAdd.ToString() + " times");
            return originalString;
        }
        public static string RandomStringWithSPace(int length = 0)
        {
            const string chars = "ABC DEFGHIJ KLMNOPQ RSTUVWXYZqwer tyu iopasdf ghjklzxc vbnm";

            if (length > 0)
            {
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(chars, GetRandomInt(10)).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        public static byte[] CreateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, new byte[] { 0xDE, 0xAD, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF }, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }
        // From: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netframework-4.7.2
        public static byte[] EncryptStringToBytes_Aes(string plainTextInputData, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainTextInputData == null || plainTextInputData.Length <= 0)
                throw new ArgumentNullException("EncryptStringToBytes_Aes() plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("EncryptStringToBytes_Aes() Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("EncryptStringToBytes_Aes() IV");
            byte[] encrypted;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the crypto IO stream.
                            swEncrypt.Write(plainTextInputData);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ReplaceHexString(List<string> BadStrings, string hexRepresentation,string StringToReplaceItWith="")
        {
            string NEWhexRepresentation = hexRepresentation;
            foreach (string sigString in BadStrings)
            {
                string hexReplace = BitConverter.ToString(Encoding.Unicode.GetBytes(sigString)).Replace("-", string.Empty);

                string newData = BitConverter.ToString(Encoding.Unicode.GetBytes(StringToReplaceItWith)).Replace("-", string.Empty);

                string ValueThisPass = NEWhexRepresentation;

                NEWhexRepresentation = NEWhexRepresentation.Replace(hexReplace, newData);

                if (NEWhexRepresentation != ValueThisPass)
                {
                    Console.WriteLine("[+] Replaced in Binary HEX value for '" + sigString + "' with '" + StringToReplaceItWith + "'");
                }
            }
            if (NEWhexRepresentation == hexRepresentation)
            {
                Console.WriteLine("[!] The new binary HEX string equals the old one, thus no bad string have been changed in the newly compiled bin.");
            }
            return NEWhexRepresentation;
        }

    public static string StringToVariable(string InputString, string addBeforeString = "var ",string addAfterString="")
        {
            List<string> Chunks=ChunkString(InputString, GetRandomInt(30, 10));

            string code="";

            for (int x = 0; x < Chunks.Count; ++x)
            {
                code += code + addBeforeString+Chunks.ElementAt(x)+ addAfterString;
            }
            return code;
        }
        private static List<string> ChunkString(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }
    }
}


