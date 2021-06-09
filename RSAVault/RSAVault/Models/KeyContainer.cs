using System;
using System.Collections.Generic;
using System.Text;
using Kit.Sql.Attributes;
using Kit.Sql.Interfaces;

namespace RSAVault.Models
{
    [Preserve]
    public class KeyContainer : IGuid
    {
        private Key Key;
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public Guid Guid { get; set; }

        public string Name { get; set; }
        [Ignore]
        public string PrivateKey { get; set; }
        [Ignore]
        public string PublicKey { get; set; }
        [Ignore]
        public string Modulus { get; set; }
        public string XML
        {
            get => Key?.XML;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Key = Key.Create(value);
                    PrivateKey = Key.PrivateKeyExponent;
                    PublicKey = Key.PublicKeyExponent;
                    Modulus = Key.Modulus;
                }
            }

        }
        public KeyContainer()
        {

        }
        public KeyContainer(string Name, Key key)
        {
            this.Key = key;
            this.Name = Name;
        }
        public bool TestKey(string TestString = "This is a test")
        {
            //byte[] original = System.Text.Encoding.UTF8.GetBytes(TestString);
            //byte[] crypted =this.Key.Encrypt(TestString);


            string encrypted = this.Key.Encrypt(TestString);
            string s_crypted = this.Key.Decrypt(encrypted);
            if (encrypted == s_crypted)
            {
                string decrypted = this.Key.Decrypt(encrypted);
                return TestString == decrypted;
            }
            return false;

        }

        internal string Decrypt(string text) => Key.Decrypt(text);
        internal string Encrypt(string text)=>Key.Encrypt(text);
    }
}
