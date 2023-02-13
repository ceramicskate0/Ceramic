using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Ceramic
{
    class FindAndReplace
    {
        public static void ReadFileReplaceString(string InputFilePath, string FindThis, string ReplaceItWithThis)
        {
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine(InputFilePath + " Does not exist.");
                Environment.Exit(1);
            }
            if (File.Exists(ReplaceItWithThis))
            {
                ReplaceItWithThis = File.ReadAllText(ReplaceItWithThis);
            }
            string FileContents = File.ReadAllText(InputFilePath);
            var regex = new Regex(FileContents);
            FileContents = regex.Replace(FindThis, ReplaceItWithThis, 1);
            File.WriteAllText(InputFilePath, FileContents);
            Console.WriteLine("Replaced contents of file with what you wanted if it was there.");
        }
    }
}
