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
        public static byte[] Encrypt(byte[] str, byte[] keys, byte[] iv, byte[] frontjunk = default, byte[] Backjunk= default)
        {
            byte[] encrypted;
            if (frontjunk != default && Backjunk != default)
            {
                byte[] bytes = new byte[str.Length + frontjunk.Length + Backjunk.Length];
                Console.WriteLine("[*] Byte Length at start/front of shellcode for junk bytes = " + frontjunk.Length);
                Console.WriteLine("[*] Byte Length at end/back of shellcode for junk bytes = " + Backjunk.Length);
                Utils.AddtToEnd(str, Backjunk);
                Utils.AddtToFront(str, frontjunk);
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = keys;

                    if (iv.Length <= 0)
                    {
                        aes.GenerateIV();
                    }
                    else
                    {
                        aes.IV = iv;
                    }
                    Console.WriteLine("IV = " + aes.IV.ToString());
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                        ICryptoTransform encoder = aes.CreateEncryptor();
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encoder, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(bytes);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            else
            {
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = keys;

                    if (iv.Length <= 0)
                    {
                        aes.GenerateIV();
                    }
                    else
                    {
                        aes.IV = iv;
                    }
                    Console.WriteLine("IV = " + aes.IV.ToString());
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                        ICryptoTransform encoder = aes.CreateEncryptor();
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encoder, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(str);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }
        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
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
