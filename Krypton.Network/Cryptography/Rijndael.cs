using Krypton.Network.Cryptography.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Network.Cryptography
{
    public class Rijndael : ICryptoProvider
    {
        private RijndaelManaged BuildRigndaelCommon(out byte[] rgbIV, out byte[] key)
        {
            rgbIV = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x5, 0x6, 0x7, 0x8, 0xA, 0xB, 0xC, 0xD, 0xF, 0x10, 0x11, 0x12 };
            key = new byte[] { 0x0, 0x1, 0x2, 0x3, 0x5, 0x6, 0x7, 0x8, 0xA, 0xB, 0xC, 0xD, 0xF, 0x10, 0x11, 0x12 };

            RijndaelManaged rijndael = new RijndaelManaged 
            { 
                BlockSize = 128, 
                IV = rgbIV, 
                KeySize = 128, 
                Key = key, 
                Padding = PaddingMode.PKCS7 
            };

            return rijndael;
        }

        public string Decrypt(string input)
        {
            byte[] input_bytes = GetBytesFromHex(input);
            int length = input_bytes.Length;

            int block_size = 16 * (1 + (length / 16));
            Array.Resize(ref input_bytes, block_size);

            for (int i = length; i < block_size; i++)
            {
                input_bytes[i] = 0;
            }

            byte[] key, rgb;
            BuildRigndaelCommon(out rgb, out key);

            string text = string.Empty;
            using(var symmetryc = new RijndaelManaged())
            {
                symmetryc.Mode = CipherMode.CFB;
                symmetryc.BlockSize = 128;
                symmetryc.Padding = PaddingMode.None;

                ICryptoTransform decryptor = symmetryc.CreateDecryptor(key, rgb);
                
                using(MemoryStream ms = new MemoryStream(input_bytes))
                {
                    using(CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] decrypted_data = new byte[length];
                        int str_size = cs.Read(decrypted_data, 0, length);
                        cs.Close();

                        byte[] trimmed = new byte[str_size];
                        Array.Copy(decrypted_data, trimmed, length);
                        Array.Resize(ref trimmed, length);

                        text = Encoding.UTF8.GetString(trimmed);
                    }
                }
            }

            return text;
        }

        public string Encrypt(string input)
        {
            byte[] input_bytes = Encoding.UTF8.GetBytes(input);
            byte[] cipher_bytes = null;

            using(RijndaelManaged symmetryc = new RijndaelManaged())
            {
                byte[] rgb, key;
                var transform = BuildRigndaelCommon(out rgb, out key);
                symmetryc.Mode = CipherMode.CFB;
                ICryptoTransform encryptor = symmetryc.CreateEncryptor(key, rgb);

                using(MemoryStream ms = new MemoryStream())
                {
                    using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(input_bytes, 0, input_bytes.Length);
                        cs.Flush();
                        cs.FlushFinalBlock();

                        ms.Position = 0;
                        cipher_bytes = ms.ToArray();

                        ms.Close();
                        cs.Close();
                    }
                }
            }

            return BitConverter.ToString(cipher_bytes).Replace("-", "");
        }

        private byte[] GetBytesFromHex(string input)
        {
            if (input == null)
            {
                return new byte[0];
            }

            var numberChars = input.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
