using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Kit;
using Kit.Daemon.Devices;
using Kit.Forms.Services.Interfaces;
using RSAVault.Data;
using Xamarin.Forms;
using Device = Kit.Daemon.Devices.Device;

namespace RSAVault.Models
{
    internal static class KeyChain
    {
        public static KeyContainer PersonalKey
        {
            get
            {
                KeyContainer container = GetKey(Device.Current.DeviceId);
                if (container is not null) return container;
                container = MakeKey(Device.Current.DeviceId, RSAEncryptionPadding.Pkcs1);
                KeyChain.Save(container);
                return container;
            }

        }
        internal static KeyContainer MakeKey(string name, RSAEncryptionPadding encryptionPadding)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = $"Key #{AppData.Instance.LiteConnection.Single<int>("select seq from sqlite_sequence WHERE name = 'Key'") + 1}";
            }
            return new KeyContainer(name, Key.Create(4096));
        }
        internal static KeyContainer GetKey(string Name)
        {
            return AppData.Instance.LiteConnection.Table<KeyContainer>().FirstOrDefault(x => x.Name == Name);
        }
        internal static void Save(KeyContainer key)
        {
            AppData.Instance.LiteConnection.InsertOrReplace(key);
        }

        public static IEnumerable<KeyContainer> GetKeys()
        {
            return AppData.Instance.LiteConnection.Table<KeyContainer>();
        }
    }
}
