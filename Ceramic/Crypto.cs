using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ceramic
{    
    static class Crypto
    {
        //private static string aes_iv = "bsxnWolsAyO7kCfWuyrnqg==";
        //private static string aes_key = "AXe8YwuIn1zxt3FPWTZFlAa14EHdPAdN9FaZ9RQWihc=";
        
        public static byte[] Encrypt(byte[] bytesToEncrypt, string password,string iv)
        {
            byte[] ivSeed = Guid.NewGuid().ToByteArray();

            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            //byte[] Key = Convert.FromBase64String(aes_key);
            //byte[] IV = Convert.FromBase64String(aes_iv);;
            byte[] Key = Encoding.Unicode.GetBytes(password);
            byte[] IV = Encoding.Unicode.GetBytes(iv);

            byte[] encrypted;
            using (MemoryStream mstream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream, aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    }
                }
                encrypted = mstream.ToArray();
            }

            //var messageLengthAs32Bits = Convert.ToInt32(bytesToEncrypt.Length);
            //var messageLength = BitConverter.GetBytes(messageLengthAs32Bits);

            //encrypted = encrypted.Prepend(ivSeed);
            //encrypted = encrypted.Prepend(messageLength);

            return encrypted;
        }

        //c# code to return unencrypted byte code
        public static byte[] Decrypt(byte[] bytesToDecrypt, string password,string iv)
        {
            byte[] tmp;

            var length = bytesToDecrypt.Length;
            byte[] ivSeed = Guid.NewGuid().ToByteArray();
            var rfc = new Rfc2898DeriveBytes(password, ivSeed);
            //byte[] Key = Encoding.Unicode.GetBytes(password);

            byte[] Key = Encoding.Unicode.GetBytes(password);
            byte[] IV = Encoding.Unicode.GetBytes(iv);

            using (MemoryStream mStream = new MemoryStream(bytesToDecrypt))
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider() { Padding = PaddingMode.None })
            using (CryptoStream cryptoStream = new CryptoStream(mStream, aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
            {
                cryptoStream.Read(bytesToDecrypt, 0, length);
                tmp = mStream.ToArray().Take(length).ToArray();
            }
            return tmp;
        }

        // Simple XOR routine from https://github.com/djhohnstein/CSharpCreateThreadExample
        private static byte[] XorByteArray(byte[] origBytes, char[] cryptor)
        {
            byte[] result = new byte[origBytes.Length];
            int j = 0;
            for (int i = 0; i < origBytes.Length; i++)
            {
                // If we're at the end of the encryption key, move
                // pointer back to beginning.
                if (j == cryptor.Length - 1)
                {
                    j = 0;
                }
                // Perform the XOR operation
                byte res = (byte)(origBytes[i] ^ Convert.ToByte(cryptor[j]));
                // Store the result
                result[i] = res;
                // Increment the pointer of the XOR key
                j += 1;
            }
            // Return results
            return result;
        }

        public static void XORShellcodeFile(string FileLocation, string key)
        {
            if (!File.Exists(FileLocation))
            {
                Console.WriteLine("Could not find path to shellcode bin file: {0}", FileLocation);
                Environment.Exit(1);
            }
            byte[] shellcodeBytes = File.ReadAllBytes(FileLocation);
            // This is the encryption key. If changed, must also be changed in the
            // project that runs the shellcode.
            char[] cryptor = key.ToCharArray();
            byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
            File.WriteAllBytes("XorShellcode.bin", encShellcodeBytes);
            Console.WriteLine("Wrote encoded binary to XorShellcode.bin.");
        }
    }
}
