using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Ceramic
{
    class VBA
    {
        public static string ChunckRAWtoVBArrys(string FilePath)
        {
            int ChunkSizes = 100;
            List<string> Chunks = new List<string>();
            String ShellcodeHex = "";

            string VBAArrayName = "buf";//Utils.RandomString(DateTime.Now.Second);
            string VBA = "";

            FileStream fs = new FileStream(FilePath, FileMode.Open);
            int hexIn;
            for (int i = 0; (hexIn = fs.ReadByte()) != -1; i++)
            {
                ShellcodeHex += string.Format(hexIn + ",");
            }

            int stringLength = ShellcodeHex.Length;
            ChunkSizes = ShellcodeHex.Length / 2;
            for (int i = 0; i < stringLength; i += ChunkSizes)
            {
                if (i + ChunkSizes > stringLength) ChunkSizes = stringLength - i;
                string item = ShellcodeHex.Substring(i, ChunkSizes);
                if (item.ElementAt(item.Length - 1) == ',')
                {
                    item = item.Substring(0, item.Length - 1);
                }
                if (item.ElementAt(0) == ',')
                {
                    item = item.Substring(1);
                }
                Chunks.Add(item);

            }

            VBA += VBAArrayName + "=Join(Array(" + Chunks.ElementAt(0) + "))\r\n";

            for (int x = 1; x < Chunks.Count; ++x)
            {
                VBA += VBAArrayName + "= Join(Array(" + Chunks.ElementAt(x) + "))\r\n";
            }

            VBA += "\r\n";
            return VBA;
        }

    }
}
