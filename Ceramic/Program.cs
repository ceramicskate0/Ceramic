using System;
using System.IO;

namespace Ceramic
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintLogo();

            if (args.Length<=0)
            {
                Usage();
                Environment.Exit(0);
            }
            try
            {
                    switch (args[0])
                    {
                        case "-b64":
                            Compress.Base64File(args[1]);
                            break;
                        case "-GZIP":
                            if (args.Length==2)
                            Compress.GZIP(args[1]);
                            else
                            Compress.GZIP(args[1],args[2]);
                            break;
                        case "-xor":
                            Crypto.XORShellcodeFile(args[1], args[2]);
                            break;
                        case "-far":
                            FindAndReplace.ReadFileReplaceString(args[1], args[2], args[3]);
                            break;
                        case "-AVFileCheck":
                            AVChecks.AVTest(args[1]);
                            break;
                        case "-DefenderCheck":
                            DefenderCheck.DefenderCheckScan(args[1]);
                            break;
                        case "-ChunkHTAShellcode":
                            Console.WriteLine("[*] Writing File ChunkHTAShellcode.txt");
                            File.WriteAllText("ChunkHTAShellcode.txt", HTA.ChunkRAWShellcode_HTA(args[1], Convert.ToInt32(args[2])));
                            break;
                        case "-ChunckRAWtoVBArrys":
                            Console.WriteLine("[*] Writing File ChunckRAWtoVBArrys.txt");
                            File.WriteAllText("ChunckRAWtoVBArrys.txt",VBA.ChunckRAWtoVBArrys(args[1]));
                            break;
                        case "-ConvertToIntArray":
                            Console.WriteLine("[*] Writing File ConvertedINTArray.txt");
                            File.WriteAllText("ConvertedINTArray.txt",BinaryOperations.ByteShellcodeToInt(File.ReadAllBytes(args[1])));
                            break;
                        case "-ConvertToINT64Array":
                            Console.WriteLine("[*] Writing File ConvertedINT64Array.txt");
                            File.WriteAllText("ConvertedINT64Array.txt", BinaryOperations.ByteToInt64(File.ReadAllBytes(args[1])));
                            break;
                        case "-ConvertToGUIDArray":
                            Console.WriteLine("[*] Writing File ConvertToGUIDArray.txt");
                            File.WriteAllText("ConvertToGUIDArray.txt", BinaryOperations.ByteToGUID(File.ReadAllBytes(args[1])));
                            break;
                        case "-ConvertShellcodeToRandomWords":
                            if (File.Exists(args[1]))
                            {
                                byte[] tmp = File.ReadAllBytes(args[1]);
                                File.WriteAllText("ConvertShellcodeToRandomWords.txt", Utils.ConvertShellcodeToRandomWordsBasedOnByte(tmp));
                            }
                            else
                            {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                            }
                            break;
                        case "-GenRandomWordArray":
                            if (File.Exists(args[1]))
                            {
                                byte[] tmp = File.ReadAllBytes(args[1]);
                                if (args.Length == 3)
                                {
                                    File.WriteAllText("RandomWordArray.txt", Utils.ConvertShellcodeToPreDefineRandomWordsBasedOnByte(tmp,Convert.ToInt32(args[2])));
                                }
                                else
                                {
                                    File.WriteAllText("RandomWordArray.txt", Utils.ConvertShellcodeToPreDefineRandomWordsBasedOnByte(tmp));
                                }
                            }
                            else
                            {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                            }
                            break;
                         case "-aes":
                            try
                            {
                                if (File.Exists(args[1]))
                                {
                                    if (args.Length == 4)
                                    {
                                        byte[] tmp = Crypto.Encrypt(File.ReadAllBytes(args[1]), Utils.StringToByteArray(args[2]), Utils.StringToByteArray(args[3]));
                                        Console.WriteLine("[*] Writing File 'EncryptedShellcode.bin' and 'EncryptedShellcodeB64.txt' to current dir");
                                        File.WriteAllText("EncryptedShellcodeB64.txt", Convert.ToBase64String(tmp));
                                        File.WriteAllBytes("EncryptedShellcode.bin", tmp);
                                    }
                                    else if (args.Length == 6)
                                    {
                                        byte[] tmp = Crypto.Encrypt(File.ReadAllBytes(args[1]), Utils.StringToByteArray(args[2]), Utils.StringToByteArray(args[3]), Utils.StringToByteArray(args[4]), Utils.StringToByteArray(args[5]));
                                        Console.WriteLine("[*] Writing File 'EncryptedShellcode.bin' and 'EncryptedShellcodeB64.txt' to current dir");
                                        File.WriteAllText("EncryptedShellcodeB64.txt", Convert.ToBase64String(tmp));
                                        File.WriteAllBytes("EncryptedShellcode.bin", tmp);
                                    }
                                    else
                                    {
                                    Console.WriteLine("[!] So you didnt put in the correct number of args count="+args.Length);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("[!] SHIT SOMETHING WENT WRONG! "+e.Message.ToString());
                            }
                            break;
                    case "-ConvertToHEX":
                            try
                            {
                                if (File.Exists(args[1]))
                                {
                                    byte[] tmp = File.ReadAllBytes(args[1]);
                                    Console.WriteLine("[*] Writing File 'HexCodeOuput.txt' to current dir");
                                    File.WriteAllText("HexCodeOuput.txt", BinaryOperations.ByteArrayToHEXString(tmp));
                                }
                                else
                                {
                                    Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("[!] SHIT SOMETHING WENT WRONG! " + e.Message.ToString());
                            }
                            break;
                        case "-Reverse":
                            if (File.Exists(args[1]))
                            {
                                string filename = Path.GetFileName(args[1]).Split('.')[0] + "reverse";
                                string Dir = Path.GetDirectoryName(args[1]);
                                string ext = Path.GetExtension(args[1]);
                                Console.WriteLine("[*] Writing File with reverse string "+Dir+"\\"+filename+ext+" to current dir");
                                File.WriteAllText(Dir+"\\"+filename+ext, Utils.ReverseString(File.ReadAllText(args[1])));
                            }
                            else
                             {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                             }
                            break;
                        case "-AddJunk":
                            if (File.Exists(args[1]))
                            {
                                string filename = Path.GetFileName(args[1]).Split('.')[0] + "junked";
                                string Dir = Path.GetDirectoryName(args[1]);
                                string ext = Path.GetExtension(args[1]);

                                Console.WriteLine("[*] Writing File with Junk in string " + Dir + "\\" + filename + ext + " to current dir");
                                File.WriteAllText(Dir + "\\" + filename + ext, Utils.AddJunkToString(File.ReadAllText(args[1])));
                            }
                            else
                            {
                                Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                            }
                            break;
                        case "-ByteReplaceBin":
                                if (args.Length == 4)
                                {
                                    BinaryOperations.ReplaceBinString(args[1], args[2], args[3]);
                                }
                                else
                                {
                                    BinaryOperations.ReplaceBinString(args[1], args[2],args[3],args[4]);
                                }

                            break;
                        case "-ChunkShellcodeTextToVariableCSharp"://REF:https://github.com/mobdk/Zeta/blob/main/Zeta.cs
                            string code=Utils.StringToVariable(File.ReadAllText(args[1]),"var "," = \""+Utils.RandomString(20)+"\";\n");
                            code += @"

                                    //using System.Linq.Expressions;
                                    //Code Below will turn variable into string that can be used as shellcode. Setup for a forloop right now
                                    public static string GetVariableDataTypeNameAsString<T>(Expression<Func<T>> memberExpression)
                                    {
                                        MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
                                        return expressionBody.Member.Name;
                                    }
                                    ";
                            File.WriteAllText("ChunkShellcodeTextToVariableCSharp.cs", code);
                        break;
                    case "-HextoByteFile":
                        if (File.Exists(args[1]))
                        {
                            File.WriteAllBytes("HextoByteFile.bin", Utils.StringToByteArray(File.ReadAllText(args[1])));
                        }
                        else
                        {
                            Console.WriteLine("[!] So you files isnt where you said it was. " + args[1]);
                        }
                        break;
                    case "-h":
                            Usage();
                            break;
                        case "-help":
                            Usage();
                            break;
                        case "?":
                            Usage();
                            break;
                    }
            }
            catch (Exception e)
            {
                Usage();
                Console.WriteLine("[Error] ERROR messgae =" + e.StackTrace.ToString()+ "\n"+e.Message.ToString());
            }
        }
        private static void PrintLogo()
        {
            Console.WriteLine(@"
 _____                               _       
/  __ \                             (_)      
| /  \/  ___  _ __  __ _  _ __ ___   _   ___ 
| |     / _ \| '__|/ _` || '_ ` _ \ | | / __|
| \__/\|  __/| |  | (_| || | | | | || || (__ 
\____/ \___ ||_|   \__,_||_| |_| |_||_| \___|
created by: Ceramicskate0
");
        }
        static void Usage()
        {
            PrintLogo();
            Console.WriteLine(@"
            CeramicSkate0's 1 stop shop in dotnet core to do random Red Team tasks via 1 app.  
            Cuz I cant remember all the random commandline args and tools to do it.      
            So yes things are case sensitive and yes the commandline inputs must be in order shown below.

            Commandline Params:
            
            -AVFileCheck {Input File Path}
            Run a modified version of DefenderCheck by matterpreter to try and trigger and AV response to test a file. Inspired by code from https://github.com/matterpreter/DefenderCheck          

            -DefenderCheck {Input File Path}
             matterpreter tool. Takes a binary as input and splits it until it pinpoints that exact byte that Microsoft Defender will flag on, and then prints those offending bytes to the screen. This can be helpful when trying to identify the specific bad pieces of code in your tool/payload. This code is from https://github.com/matterpreter/DefenderCheck
            
            -b64 {Input File Path}
            The command above will base64 encode a input file and save it to an output file Base64FileOutput.txt.

            -xor {Input .bin File Path} {XOR KEY}
            The command above will xor a byte file with a key and output it to XorShellcode.bin. This mean when you un-xor it you will need the same key.

            -aes {Input .bin File Path} HEX{AES_KEY} HEX{AES_IV} HEX{Junk bytes to add to front of shellcode}(optinoal) HEX{Junk bytes to add to end of shellcode}(optinoal)
             Read a byte file and will aes encrypt it with the provided Key and IV. Output file is EncryptedShellcode.bin

            -far {Input File or the file you want to search thru} {What you want to change} {What you want to change it to (File or string)(Will check to see if file exists if not assumes you wanted to use a string)}
            'FAR' (Find and Replace) will take a input file(1st arg) and then replace in that file the 2nd arg you specify with either the string your specify or the conents of a file you specify in the 3rd arg.

            -ChunkHTAShellcode {Input a already B64 encoded shellcode File Path} {Optional: Number of chunks}
            Attempts chunk and encode a shellcode input file and output it into a HTA ready to copy and paste output. Optional 2nd arg to tell it how many chunks. Default 100.
            
            -ChunckRAWtoVBArrys {Input File Path}
            Attempts chunk and encode a byte input file and output it into a VBA ready to copy and paste output. Optional 2nd arg to tell it how many chunks. Default 100.

            -Reverse {Input File Path}
            Reads the entire text file as 1 string and will write another file with the first files contents reversed. Output file will be same file name .reverse
    
            -AddJunk {Input File Path}
            Reads a text file and will randomly add a randomly generated junk string into the files contents and then output a new file with the junk in it. Output file will be same file name .junked

            -ByteReplaceBin {Input File Path/Name} {Ouput File Path/Name} {A single term, web site, or file 1 per line of things to randomly replace in the binary} {optional: when using a single term replace it with this term/string}
            Will read in a compiled (.Net) file and bin replace the bad strings with random list (1 per line) from a file or web page, or a single term (randomly generated string if no 4th arg is supplied. If a 4th is applied it will be used to replace the single bad string. (will check length for you).

            -GZIP {Input File Path} {Output File Path}(optional)
            Take a byte file read all the bytes in it and gzip the file and output a compressed version of it. Optional output file can be given. Default output file 'GZIPFileOutput.gz' in cwd

            -ConvertToHEX {Input File Path}
            Take a byte file and output a hex version of it.Ouputs the byte file to a file of string HEX output file is 'HexCodeOuput.txt'

            -ConvertToIntArray {Input File Path}
            Take a byte file and output a txt file with an array of INT's. Output file is ConvertedINTArray.txt

            -ConvertToINT64Array {Input File Path}
            Take a byte file and output a txt file with an array of INT64's. Output file is ConvertedINT64Array.txt

            -ChunkShellcodeTextToVariableCSharp (Input File Path}
            Take a text file (ie b64 shellcode) and obsure it in a way that the shellcode is now variable names chunked. It will also give you the code to reverse this. Output File is ChunkShellcodeTextToVariableCSharp.cs
            
            -ConvertToGUIDArray (Input File Path}
            Take in a byte file and output a text file that is a array of GUID's that represents the input shellcode. Output file is ConvertToGUIDArray.txt.

            -ConvertShellcodeToRandomWords (Input File Path} {Max word length}(optional)
            Convert shellcode to random words that will represent byte values from 0-255 based on length of word. This will be an array of random words per byte that map to byte value. outputs 3 files 1 with c styled array, 1 with csv word list, 1 with byte array key.

            -GenRandomWordArray (Input File Path} {Max word length}(optional)
            Making Array of random words that will represent byte values from 0-255 based on location in array. Can be used to ref in you dropper for shellcode. This will be an array of size 256 that could be used to lookup byte number based on word position in array.

");
        }

    }
}
