using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Ceramic
{
    class Compress
    {
        public static string Base64File(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine(FilePath + " input file does not exists where you say it is.");
                Environment.Exit(1);
            }
            byte[] bytes = File.ReadAllBytes(FilePath);
            Console.WriteLine("Writing file to Base64FileOutput.txt");
            File.WriteAllText("Base64FileOutput.txt", Convert.ToBase64String(bytes));
            return Convert.ToBase64String(bytes);
            //Console.WriteLine(Convert.ToBase64String(bytes));
        }
        
        public static void GZIP(string FilePath,string Outpath= "GZIPFileOutput.gz")
        {
            var bytes = File.ReadAllBytes(FilePath);
            using (FileStream fs = new FileStream(Outpath, FileMode.CreateNew))
            using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
            {
                zipStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
