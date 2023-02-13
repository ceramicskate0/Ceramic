using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ceramic
{
    class BinaryOperations
    {

       private const string formatter = "{0,5}{1,27}{2,24}";
        
       //REF: https://docs.microsoft.com/en-us/dotnet/api/system.bitconverter.toint64?view=net-5.0
        private static Int64 BAToInt64(byte[] bytes, int index)
        {
            long value = BitConverter.ToInt64(bytes, index);
            return value;
        }

        public static string ByteToGUID(byte[] shellcode)
        {
            int count = 16;
            string guidArrays = "Guid[] GUIDArray={";

            for (int x=0;x<shellcode.Length;x+= count)
            {
                var selected = shellcode.Skip(x).Take(count).ToArray();
                string guid2 = new Guid(selected).ToString();
                guidArrays += guid2+",";
            }

            return guidArrays.TrimEnd(',') + "};";

        }

        public static string ByteToInt64(byte[] shellcode)
        {
            string t= "long[] nums = { ";
            int byteBlock = 16;
            string ConvertBack=@"
        foreach (var value in values) 
        {
        byte[] byteArray = BitConverter.GetBytes(value);
        }
";
            Console.WriteLine("[*] 1 example way to convert back to bytes:" + ConvertBack);
            for (int n = 0; n < shellcode.Length; n += byteBlock)
            {
                t += BAToInt64(shellcode, n)+",";
            }

            return t.TrimEnd(',')+"};";
        }

        public static string ByteShellcodeToInt(byte[] shellcode)
        {
            int[] bytesAsInts = Array.ConvertAll(shellcode, c => (int)c);
            string result = string.Join(",", bytesAsInts);
            return result;
        }

        public static string ByteArrayToHEXString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] ReadPEFile(string FilePath)
        {
            return Convert.FromBase64String(Convert.ToBase64String(File.ReadAllBytes(FilePath)));
        }

        public static byte[] BinaryFileRead(string FilePath)
        {
            System.IO.FileStream fileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binReader = new System.IO.BinaryReader(fileStream);
            byte[] fileBytes = binReader.ReadBytes((int)fileStream.Length);
            binReader.Close();
            fileStream.Close();
            return fileBytes;
        }

        public static void ReplaceBinString(string inputFile, string outputfile, string ListOfBadStrings,string ReplaceWithString="")
        {
            try
            {
                try
                {
                    List<string> BadStrings;
                    bool singlething = false;
                    if (ListOfBadStrings.ToLower().Contains("http"))//list of things from url
                    {
                        WebClient webClient = new WebClient();
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        Console.WriteLine("[*] Downloading bad strings from '" + ListOfBadStrings + "'");
                        BadStrings = webClient.DownloadString(ListOfBadStrings).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    }
                    else if (File.Exists(ListOfBadStrings) == true)//list of things from file system
                    {
                        Console.WriteLine("[*] Reading bad strings from '" + ListOfBadStrings + "'");
                        BadStrings = File.ReadAllLines(ListOfBadStrings).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                    }
                    else//single thing
                    {
                        Console.WriteLine("[*] Replacing bad strings '" + ListOfBadStrings + "'");
                        BadStrings = new List<string> { ListOfBadStrings };
                        singlething = true;
                    }

                    if (singlething == false)
                    {
                        string hexRepresentation = Utils.ReplaceHexString(BadStrings, BitConverter.ToString(File.ReadAllBytes(inputFile)).Replace("-", string.Empty));
                        File.WriteAllBytes(outputfile, Utils.StringToByteArray(hexRepresentation));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ReplaceWithString))
                        {
                            ReplaceWithString = Utils.RandomString(ListOfBadStrings.Length);
                            Console.WriteLine("[+] Random string being used to replace string is '" + ReplaceWithString+"'");
                        }

                        if (ListOfBadStrings.Length == ReplaceWithString.Length)
                        {
                            string hexRepresentation = Utils.ReplaceHexString(BadStrings, BitConverter.ToString(File.ReadAllBytes(inputFile)).Replace("-", string.Empty), ReplaceWithString);
                            File.WriteAllBytes(outputfile, Utils.StringToByteArray(hexRepresentation));
                        }
                        else
                        {
                            throw new IOException("[ERROR] The string you wanted to replace is not the same size as the string your replacing it with");
                        }
                    }

                    Console.WriteLine("[*] Output File Path: " + outputfile);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] ERROR FAILED to download and apply string replacement for list of bad strings from " + ListOfBadStrings + "[!] ERROR Reason: " + e.Message.ToString());
                    throw new IOException("[ERROR] ERROR FAILED to download and apply string replacement for list of bad strings");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Unable to write assembly to disk. Reason, " + e.Message.ToString());
                throw new IOException("[ERROR] Unable to write assembly to disk.");
            }
        }
    }
}
