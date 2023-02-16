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
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv, byte[] frontjunk = default, byte[] Backjunk= default)
        {
            byte[] encrypted;
            Console.WriteLine("[*] First 2 bytes of unencrypted bin: " + data[0] + " " + data[1]);
            Console.WriteLine("[*] Last 2 bytes of unencrypted bin: " + data[data.Length - 2] + " " + data[data.Length - 1]+"\n");
            Console.WriteLine("[*] Length of unencrypted bin file: " + data.Length);
            if (frontjunk != default && Backjunk != default)
            {
                byte[] bytes = new byte[data.Length];
                using (Aes aes = Aes.Create())
                {
                    try
                    {
                        aes.Key = key;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[!] AES error = " + e.Message.ToString());
                        Console.WriteLine("[!] Making random key to fix error for you");
                        aes.GenerateKey();
                    }

                    if (iv.Length <= 0)
                    {
                        aes.GenerateIV();
                    }
                    else
                    {
                        try
                        {
                            aes.IV = iv;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("[!] AES error = " + e.Message.ToString());
                            Console.WriteLine("[!] Making random IV to fix error for you");
                            aes.GenerateIV();
                        }
                    }
                    Console.WriteLine("[+] Key = " + Utils.ByteArrayToHexString(aes.Key));
                    Console.WriteLine("[+] IV = " + Utils.ByteArrayToHexString(aes.IV));
                    Console.WriteLine("[+] C# Key = " + Utils.ByteArrayToHexString(aes.Key).Replace("\\", "\\\\")); 
                    Console.WriteLine("[+] C# IV = " + Utils.ByteArrayToHexString(aes.IV).Replace("\\", "\\\\"));
                    Console.WriteLine("[*] Writing AES KEy and IV to disk AESinfo.txt");
                    File.WriteAllText("AESinfo.txt", "KEY=" + Utils.ByteArrayToHexString(aes.Key)+"\nIV=" + Utils.ByteArrayToHexString(aes.IV)+ "\nfrontjunk="+ frontjunk.Length+ "\nBackjunk="+ Backjunk.Length);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.Zeros;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                        // Create the streams used for encryption.
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(data, 0, data.Length);
                                csEncrypt.FlushFinalBlock();

                                encrypted = msEncrypt.ToArray();
                            }
                        }

                    Console.WriteLine("\n[*] Starting Encrypted Shellcode Length: " + encrypted.Length + "\n");
                    bytes=Utils.AddToArray(encrypted, Backjunk);
                    Console.WriteLine("[*] First 2 bytes of start Array: " + encrypted[0] + " " + encrypted[1]);
                    Console.WriteLine("[*] Last 2 bytes of back Junk Array: " + Backjunk[Backjunk.Length-2] + " " + Backjunk[Backjunk.Length-1]);
                    Console.WriteLine("[*] First 2 bytes of back New/final Array: " + bytes[0] + " " + bytes[1]);
                    Console.WriteLine("[*] Last 2 bytes of back New/final Array: " + Backjunk[Backjunk.Length - 2] + " " + Backjunk[Backjunk.Length - 1]);
                    Console.WriteLine("[*] Byte Length at end/back of shellcode for junk bytes = " + Backjunk.Length);
                    Console.WriteLine("[*] Code appended to back Shellcode Length: " + bytes.Length + "\n");

                    byte[] bytes2 = Utils.AddToArray(frontjunk, bytes);
                    Console.WriteLine("[*] First 2 bytes of start Array: " + bytes[0] + " " + bytes[1]);
                    Console.WriteLine("[*] First 2 bytes of front Junk Array: " + frontjunk[0] + " " + frontjunk[1]);
                    Console.WriteLine("[*] First 2 bytes of front New/final Array: " + bytes2[0] + " " + bytes2[1]);
                    Console.WriteLine("[*] Last 2 bytes of back Junk Array: " + Backjunk[Backjunk.Length - 2] + " " + Backjunk[Backjunk.Length - 1]);
                    Console.WriteLine("[*] Last 2 bytes of New/final Array: " + bytes2[bytes2.Length - 2] + " " + bytes2[bytes2.Length - 1]);
                    Console.WriteLine("[*] Byte Length at start/front of shellcode for junk bytes = " + frontjunk.Length);
                    Console.WriteLine("[*] Code appended to front Shellcode Length: " + bytes2.Length + "\n");
                    Console.WriteLine("\n[*] Shellcode Length should be: " + (Backjunk.Length + frontjunk.Length + encrypted.Length));
                    Console.WriteLine("[*] Encrypted Shellcode Length: " + bytes2.Length);
                    return bytes2;
                }
            }
            else
            {
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = key;

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
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
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
