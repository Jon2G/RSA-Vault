using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Forms9Patch;
using Kit.Forms.Security.RSA;
using Kit.Model;
using RSAVault.Models;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

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
        
        private Key _Key;

        public Key Key
        {
            get => _Key;
            set
            {
                _Key = value;
                Raise(()=> Key);
            }
        }
        public FromTextPageViewModel()
        {
            this.Key = KeyChain.PersonalKey;
        }
        public async void SaveAsNote()
        {
            var note = new Note() { Text = this.Text };
            //save
            await Shell.Current.Navigation.PopToRootAsync(true);
            await Shell.Current.Navigation.PushAsync(new NotePage(note), true);
        }
        public void Share()
        {

        }
        private void Update()
        {
            this.Encrypted = Key.EncryptToString(Text);
        }
        public async void ChangeKey()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            var certificates = new CertificatesPage();
            certificates.Model.KeyClickedCommand = new Command<Key>(ChangeKey);
            await Shell.Current.Navigation.PushAsync(certificates, true);
        }
        private async void ChangeKey(Key Certificate)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            await Shell.Current.Navigation.PopAsync( true);
            this.Key = Certificate;
        }
    }
}
