using System;
using Kit.Model;
using Kit.Sql.Attributes;
using Kit.Sql.Interfaces;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using RSAVault.Data;
using RSAVault.Resources;

namespace RSAVault.Models
{
    [Preserve]
    public class Settings : ModelBase, IGuid
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private bool _IsFingerPrintActive;
        public bool IsFingerPrintActive
        {
            get => _IsFingerPrintActive;
            set
            {
                _IsFingerPrintActive = value;
                Raise(() => IsFingerPrintActive);
            }
        }

        public async void IsFingerPrintAvaible()
        {
            if (!await CrossFingerprint.Current.IsAvailableAsync(true) && this.IsFingerPrintActive)
            {
                this.IsFingerPrintActive = false;
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.FingerPrintNotAvaible, AppResources.Alert);
                return;
            }
            if (!await Vault.Authenticate())
            {
                this.IsFingerPrintActive = false;
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.AuthFailed, AppResources.Alert);
            }
        }

        public Settings()
        {
            this.IsFingerPrintActive = false;
        }
        public Guid Guid { get; set; }
        public void Save()
        {
            AppData.Instance.LiteConnection.InsertOrReplace(this);
        }

        public static Settings Get()
        {
            var settings = AppData.Instance.LiteConnection.Table<Settings>().FirstOrDefault();
            if (settings is null)
            {
                settings = new Settings();
                settings.Save();
            }
            return settings;
        }
    }
}
