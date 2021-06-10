using System;
using System.Linq;
using System.Text;
using Kit.Sql.Attributes;
using Kit.Sql.Interfaces;
using System.Security.Cryptography;
using Kit;
using Kit.Extensions;
using Kit.Security.Encryption;

// ReSharper disable once CheckNamespace
namespace RSAVault.Models
{

    public sealed class Key
    {

        public int KeySize { get; set; }
        private RSA Algorithm { get; set; }
        public string Modulus { private set; get; }
        public string PrivateKeyExponent { private set; get; }
        public string PublicKeyExponent { private set; get; }
        public string XML => Algorithm.ToXmlString(true);


        private void ReadKey()
        {
            var key_parameters = Algorithm.ExportParameters(true);
            Modulus = Convert.ToBase64String(key_parameters.Modulus);
            PrivateKeyExponent = Convert.ToBase64String(key_parameters.D);
            PublicKeyExponent = Convert.ToBase64String(key_parameters.Exponent);
        }
        private Key(string xml) 
        {
            Algorithm = RSA.Create();
            int size = Algorithm.LegalKeySizes.Max(x => x.MaxSize);
            KeySize = size;
            Algorithm = RSA.Create(KeySize);
            Algorithm.FromXmlString(xml);
            ReadKey();
        }
        private Key() 
        {
            Algorithm = RSA.Create();
            int size = Algorithm.LegalKeySizes.Max(x => x.MaxSize);
            KeySize = size;
            Algorithm = RSA.Create(size);
            ReadKey();
        }

        public string Encrypt(string Value)
        {
            try
            {
                byte[] encrypted = Encoding.UTF8.GetBytes(Value);
                encrypted = Algorithm.Encrypt(encrypted, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encrypted);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e,"At Encrypt");
            }
            return  string.Empty;
        }
        public string Decrypt(string Value)
        {
            byte[] encrypted = Convert.FromBase64String(Value);
            if (encrypted.Length < 128)
            {
                return string.Empty;
            }
            encrypted = Algorithm.Decrypt(encrypted, RSAEncryptionPadding.Pkcs1);
            string base_64 = Encoding.UTF8.GetString(encrypted);
            return base_64;
        }
        public static Key Create() => new Key();
        public static Key Create(string xml) => new Key(xml);
    }
}
