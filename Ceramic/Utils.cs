using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ceramic
{
    class Utils
    {
        private static Random random = new Random(DateTime.Now.Millisecond);

        public static int GetRandomInt(int max, int min = 5)
        {
            return random.Next(min, max);
        }
        public static string RandomString(int length = 0)
        {
            const string chars = "AABCDEEFGHIIJKLMNOOPQRSTUUVWXYyZqwertyuaeiouyiopasdfghjklzxcvbnm_1234567890";

            if (length > 0)
            {
                return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(chars, GetRandomInt(10)).Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        public static string RandomStringAlpha(int length = 0)
        {
            const string chars = "AABCDEEFGHIIJKLMNOOPQRSTUUVWXYyZqwertyuaeiouyiopasdfghjklzxcvbnm";

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
        public static string AddJunkToString(string originalString, string JunkToAdd = "", int StringLocationToStartAddingJunk = 1)
        {
            if (JunkToAdd == "")
            {
                JunkToAdd = RandomSpecialChars(GetRandomInt(20, 2));
            }
            Console.WriteLine("[+] Junk string added is:" + JunkToAdd);
            Random rand = new Random();
            int numberOfTimeToAdd = rand.Next(0, originalString.Length);

            for (int x = 0; x < numberOfTimeToAdd; ++x)
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

        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static string ReplaceHexString(List<string> BadStrings, string hexRepresentation, string StringToReplaceItWith = "")
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

        public static string StringToVariable(string InputString, string addBeforeString = "var ", string addAfterString = "")
        {
            List<string> Chunks = ChunkString(InputString, GetRandomInt(30, 10));

            string code = "";

            for (int x = 0; x < Chunks.Count; ++x)
            {
                code += code + addBeforeString + Chunks.ElementAt(x) + addAfterString;
            }
            return code;
        }

        private static List<string> ChunkString(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }

        public static byte[] AddtToEnd(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            //Console.WriteLine(first[first.Length - 3] + " "+ first[first.Length - 2] + " " + first[first.Length - 1]);
            //Console.WriteLine(second[second.Length - 3] + " " + second[second.Length - 2] + " " + second[second.Length - 1]);

            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);

            //Console.WriteLine(bytes[bytes.Length - 3] + " " + bytes[bytes.Length - 2] + " " + bytes[bytes.Length - 1]);
            return bytes;
        }

        public static byte[] AddtToFront(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[second.Length + first.Length];
            //Console.WriteLine(first[0] + " " + first[1] + " " + first[2]);
            //Console.WriteLine(second[0] + " " + second[1] + " " + second[2]);

            Buffer.BlockCopy(second, 0, bytes, 0, second.Length);
            Buffer.BlockCopy(first, 0, bytes, second.Length, first.Length);

            //Console.WriteLine(bytes[0] + " " + bytes[1] + " " + bytes[2]);
            return bytes;
        }

        public static string ConvertShellcodeToRandomWordsBasedOnByte(byte[] shellcode, int wordlength = 20)
        {
            Console.WriteLine("[*] Creating the array to use to lookup shellcode.");
            string shellcodewWords="";
            string Carray = "{";
            Dictionary<string, int> cipher = new Dictionary<string, int>(256);
            for (int i = 0; i <= 255; ++i)
            {
                string thing = RandomStringAlpha(GetRandomInt(wordlength, 2));
                while (cipher.ContainsKey(thing) == true)
                {
                    thing = RandomStringAlpha(GetRandomInt(wordlength, 2));
                }
                cipher.Add(thing, i);
            }
            int x = 0;
            string codedArray = "";
            string c_CodeArray1 = "{";
            foreach (KeyValuePair<string, int> kvp in cipher)
            {
                if (x == cipher.Count-1)
                {
                    codedArray += kvp.Key;
                    c_CodeArray1 += "\"" + kvp.Key + "\"";
                }
                else
                {
                    codedArray += kvp.Key + ",";
                    c_CodeArray1+= "\""+ kvp.Key + "\"" + ",";
                }
                ++x;
            }
            Console.WriteLine("[*] Writting the KeyArray.txt file that contains the array used/the key");
            File.WriteAllText("KeyArray.txt", c_CodeArray1 + "}");
            Console.WriteLine("[*] Using the newly created array to encode shellcode into strings.....this could be awhile (stageless is large)");
            for (int i = 0; i <= shellcode.Length-1; ++i)
            {
                if (i == shellcode.Length-1)
                {
                    shellcodewWords += cipher.FirstOrDefault(x => x.Value == shellcode[i]).Key;
                    Carray+= "\""+ cipher.FirstOrDefault(x => x.Value == shellcode[i]).Key+"\"";
                }
                else
                {
                    shellcodewWords += cipher.FirstOrDefault(x => x.Value == shellcode[i]).Key + ",";
                    Carray+= "\"" + cipher.FirstOrDefault(x => x.Value == shellcode[i]).Key + "\"" + ",";
                }
            }
            Console.WriteLine("[*] Creating c code version of array of word that is the shellcode into C_codeArray.txt");
            File.WriteAllText("C_codeArray.txt", Carray + "}");
            return shellcodewWords;
        }

        public static string ConvertShellcodeToPreDefineRandomWordsBasedOnByte(byte[] shellcode, int wordlength=20)
        {
            Console.WriteLine("[*] Making Array of random words that will represent byte values from 0-254 based on location in array. Can be used to ref in you dropper for shellcode.");
            string codedArray = "";
            Dictionary<string,int>cipher = new Dictionary<string,int> (256);
            for (int i = 0; i <= 255; ++i)
            {
                string thing = RandomStringAlpha(GetRandomInt(wordlength, 2));
                while (cipher.ContainsKey(thing)==true)
                {
                    thing = RandomStringAlpha(GetRandomInt(wordlength, 2));
                }
                cipher.Add(thing, i);
            }
            int x = 0;
            foreach (KeyValuePair<string, int> kvp in cipher)
            {
                if (x == cipher.Count)
                {
                    codedArray += kvp.Key;
                }
                else
                {
                    codedArray += kvp.Key + ",";
                }
                ++x;
            }
            return codedArray;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("\\{0:x2}", b);
            return hex.ToString();
        }

        public static string CSharpAESDecryptCode()
        {
            return @"        
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(\""cipherText\"");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(\""Key\"");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(\""IV\"");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }";
        }
    }
}

