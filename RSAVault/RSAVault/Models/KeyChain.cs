using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Kit;
using Kit.Daemon.Devices;
using Kit.Forms.Extensions;
using Kit.Forms.Services.Interfaces;
using RSAVault.Data;
using RSAVault.Resources;
using Xamarin.Essentials;
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
                name = $"Key #{AppData.Instance.LiteConnection.Single<int>($"select seq from sqlite_sequence WHERE name = '{nameof(KeyContainer)}'") + 1}";
            }
            return new KeyContainer(name, Key.Create());
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

        public static async void Share(KeyContainer key)
        {
            if (!await Permisos.RequestStorage())
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedStorage);
                return;
            }
            FileInfo file = new FileInfo(Path.Combine(Tools.Instance.TemporalPath, $"{key.Guid:N}.xml"));
            if (file.Exists)
            {
                file.Delete();
            }
            using (FileStream mStream = new FileStream(file.FullName, FileMode.OpenOrCreate))
            {
                using (XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        // Load the XmlDocument with the XML.
                        document.LoadXml(key.XML);
                        writer.Formatting = Formatting.Indented;
                        // Write the XML into a formatting XmlTextWriter
                        document.WriteContentTo(writer);
                        writer.Flush();
                        mStream.Flush();

                    }
                    catch (XmlException ex)
                    {
                        Log.Logger.Error(ex, "KeyChain.Share");
                        // Handle the exception
                    }
                }
            }

            ShareFileRequest request = new ShareFileRequest(new ShareFile(file.FullName))
            {
                Title = key.Name
            };
            await Xamarin.Essentials.Share.RequestAsync(request);

        }

        public static void Delete(KeyContainer key) => AppData.Instance.LiteConnection.Delete(key);
    }
}
