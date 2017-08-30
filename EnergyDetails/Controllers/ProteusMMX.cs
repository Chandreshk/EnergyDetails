using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace EnergyDetails.Controllers
{
    internal class ProteusMMX
    {
        internal class ProteusCryptographyProvider
        {
            internal static ICryptoTransform Encryptor
            {
                get { return Algorithm.CreateEncryptor(); }
            }

            internal static ICryptoTransform Decryptor
            {
                get { return Algorithm.CreateDecryptor(); }
            }

            private static SymmetricAlgorithm Algorithm
            {
                get
                {
                    Rijndael algorithm = new RijndaelManaged();
                    algorithm.Mode = CipherMode.CBC;
                    algorithm.Key = Key;
                    algorithm.IV = IV;

                    return algorithm;
                }
            }

            private static byte[] Key
            {
                get
                {
                    return new byte[]
                    {
                    0xBC, 0xC7, 0xE8, 0x28, 0x13, 0x52, 0x69, 0x99,
                    0xD8, 0x62, 0x95, 0x7B, 0x88, 0x21, 0x99, 0x60,
                    0xCE, 0x24, 0x8A, 0x4A, 0xA0, 0x71, 0xA8, 0xB9,
                    0x12, 0xFD, 0x6B, 0x70, 0x05, 0x82, 0xD9, 0x80
                    };
                }
            }

            private static byte[] IV
            {
                get
                {
                    return new byte[]
                    {
                    0x94, 0x8F, 0xE7, 0x81, 0x6C, 0x8C, 0xCD, 0xFE,
                    0x0D, 0xBD, 0x86, 0xE6, 0x13, 0x50, 0x50, 0xFF
                    };
                }
            }
        }

        public  class ProteusCryptographer
        {
            public const string ConnectionStringName = "BlackhawkDatabase";

            public static string DecryptedConnectionString
            {
                get { return GetDecryptedConnectionString(); }
            }

            public static byte[] Decrypt(byte[] encrypted)
            {
                try
                {
                    return TransformData(encrypted, ProteusCryptographyProvider.Decryptor);
                }
                catch
                {
                    return new byte[0];
                }
            }

            public static string Decrypt(string encrypted)
            {
                try
                {
                    return Encoding.Unicode.GetString(Decrypt(Convert.FromBase64String(encrypted)));
                }
                catch
                {
                    return String.Empty;
                }
            }

            public static byte[] Encrypt(byte[] unencrypted)
            {
                try
                {
                    return TransformData(unencrypted, ProteusCryptographyProvider.Encryptor);
                }
                catch
                {
                    return new byte[0];
                }
            }

            public static string Encrypt(string unencrypted)
            {
                try
                {
                    return Convert.ToBase64String(Encrypt(Encoding.Unicode.GetBytes(unencrypted)));
                }
                catch
                {
                    return String.Empty;
                }
            }

            public static string GetDecryptedConnectionString(Configuration configuration = null)
            {
                if (configuration == null)
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName] != null ?
                      Decrypt(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString) : null;
                }
                else
                {
                    return configuration.ConnectionStrings.ConnectionStrings[ConnectionStringName] != null ?
                        Decrypt(configuration.ConnectionStrings.ConnectionStrings[ConnectionStringName].ConnectionString) : null;
                }
            }

            private static byte[] TransformData(byte[] input, ICryptoTransform transform)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                        cs.Close();

                        return ms.ToArray();
                    }
                }
            }
        }
    }
}