using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Kit;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using RSAVault.Data;
using RSAVault.Resources;


namespace RSAVault.Models
{
    internal static class Vault
    {
      
        public static DirectoryInfo VaultDirectory =>
            new DirectoryInfo(Path.Combine(Tools.Instance.LibraryPath, "Vault"));
        static Vault()
        {
            if (!VaultDirectory.Exists)
            {
                VaultDirectory.Create();
            }
        }

        internal static async Task<bool> Authenticate()
        {
            var fingerprint_settings =
                new AuthenticationRequestConfiguration(AppResources.LockTitle, AppResources.LockReason)
                { AllowAlternativeAuthentication = true };
            var result = await CrossFingerprint.Current.AuthenticateAsync(fingerprint_settings);
            return result.Authenticated;
        }

        internal static IEnumerable<Note> GetNotes()
        {
            return AppData.Instance.LiteConnection.Table<Note>();
        }

        internal static void SaveNote(Note note)
        {
            note.LastModificationTime = DateTime.Now;
            KeyContainer key = KeyChain.PersonalKey;
            note.Text = key.Encrypt(note.Text);
            AppData.Instance.LiteConnection.InsertOrReplace(note);
        }

        public static void Decrypt(Note note)
        {
            KeyContainer key = KeyChain.PersonalKey;
            note.Text = key.Decrypt(note.Text);
        }
    }
}
