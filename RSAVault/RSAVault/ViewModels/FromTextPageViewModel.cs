using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Xml;
using Forms9Patch;
using Kit;
using Kit.Forms.Extensions;
using Kit.Model;

using RSAVault.Models;
using RSAVault.Resources;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace RSAVault.ViewModels
{
    public class FromTextPageViewModel : ModelBase
    {
        private ICommand changeCertificateCommand;
        public ICommand ChangeCertificateCommand => changeCertificateCommand ??= new Command(ChangeKey);
        private ICommand shareCommand;
        public ICommand ShareCommand => shareCommand ??= new Command(Share);
        private ICommand saveAsNoteCommand;
        public ICommand SaveAsNoteCommand => saveAsNoteCommand ??= new Command(SaveAsNote);

        private ICommand _UpdateCommand;
        public ICommand UpdateCommand => _UpdateCommand ??= new Command(Update);


        private string _Text;

        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                Raise(() => Text);
            }
        }
        private string _Encrypted;

        public string Encrypted
        {
            get => _Encrypted;
            set
            {
                _Encrypted = value;
                Raise(() => Encrypted);
            }
        }

        private KeyContainer _Key;

        public KeyContainer Key
        {
            get => _Key;
            set
            {
                _Key = value;
                Raise(() => Key);
            }
        }
        public FromTextPageViewModel()
        {
            this.Key = KeyChain.PersonalKey;
        }
        public async void SaveAsNote()
        {
            this.Key = KeyChain.PersonalKey;
            Update();
            //save
            await Shell.Current.Navigation.PopToRootAsync(true);
            await Shell.Current.Navigation.PushAsync(new NotePage(new Note() { Text = this.Encrypted }), true);
        }
        public async void Share()
        {
            if (!await Permisos.RequestStorage())
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedStorage);
                return;
            }
            FileInfo file = new FileInfo(Path.Combine(Tools.Instance.TemporalPath, $"{Guid.NewGuid():N}.txt"));
            if (file.Exists)
            {
                file.Delete();
            }
            using (FileStream mStream = new FileStream(file.FullName, FileMode.OpenOrCreate))
            {
                using (TextWriter writer = new StreamWriter(mStream, Encoding.UTF8))
                {
                    try
                    {
                        await writer.WriteAsync(this.Encrypted);
                        await writer.FlushAsync();
                        await mStream.FlushAsync();

                    }
                    catch (XmlException ex)
                    {
                        Log.Logger.Error(ex, "KeyChain.Share");
                    }
                }
            }

            ShareFileRequest request = new ShareFileRequest(new ShareFile(file.FullName));
            await Xamarin.Essentials.Share.RequestAsync(request);
        }
        private void Update()
        {
            this.Encrypted = Key.Encrypt(Text);
            int count = this.Encrypted.Length;
        }
        public async void ChangeKey()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            var certificates = new KeysPage
            {
                Model = { KeyClickedCommand = new Command<KeyContainer>(ChangeKey) }
            };
            await Shell.Current.Navigation.PushAsync(certificates, true);
        }
        private async void ChangeKey(KeyContainer key)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            await Shell.Current.Navigation.PopAsync(true);
            this.Key = key;
        }
    }
}
