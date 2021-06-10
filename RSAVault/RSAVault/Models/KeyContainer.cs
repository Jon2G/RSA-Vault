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



        public string Modulus => this.Key.Modulus;
        public string PrivateKeyExponent => this.Key.PrivateKeyExponent;
        public string PublicKeyExponent => this.Key.PublicKeyExponent;


        public string XML
        {
            get => Key?.XML;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Key = Key.Create(value);
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


            string encrypted = Encrypt(TestString);
            string s_crypted = Decrypt(encrypted);
            return TestString == s_crypted;

        }

        internal string Decrypt(string text) => Key.Decrypt(text);
        internal string Encrypt(string text)=>Key.Encrypt(text);
    }
}
