using System;
using System.Collections.Generic;
using System.Text;
using Kit;
using Kit.Daemon.Devices;
using Kit.Forms.Security.RSA;
using Kit.Forms.Services.Interfaces;
using RSAVault.Data;
using Xamarin.Forms;
using Device = Kit.Daemon.Devices.Device;

namespace RSAVault.Models
{
    internal static class KeyChain
    {
        public static Key PersonalKey
        {
            get
            {
                var key =GetKey(Device.Current.DeviceId);
                if (key is null)
                {
                    key = MakeKey(Device.Current.DeviceId);
                    KeyChain.Save(key);
                }
                return key;
            }

        }
        internal static Key MakeKey(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = $"Key #{AppData.Instance.LiteConnection.Single<int>("select seq from sqlite_sequence WHERE name = 'Key'") + 1}";
            }

            IRSA rsa = DependencyService.Get<IRSA>(DependencyFetchTarget.GlobalInstance);
            Key rsa_key = rsa.MakeKey(name);

            //try
            //{

            //    var keypair = Vault.AsymmetricKey.CreateKeyPair(512);

            //    var keyString = Convert.ToBase64String(keypair.Export());
            //    rsa_key = new Key(name, keyString);
            //}
            //catch (Exception e)
            //{
            //    Log.Logger.Error(e, "While creating a new key");
            //}
            return rsa_key;
        }
        internal static Key GetKey(string Name)
        {
            return AppData.Instance.LiteConnection.Table<Key>().FirstOrDefault(x => x.Name == Name);
        }
        internal static void Save(Key key)
        {
            AppData.Instance.LiteConnection.InsertOrReplace(key);
        }

        public static IEnumerable<Key> GetKeys()
        {
            return AppData.Instance.LiteConnection.Table<Key>();
        }
    }
}
