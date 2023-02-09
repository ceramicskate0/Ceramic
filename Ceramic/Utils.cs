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
    }
}


