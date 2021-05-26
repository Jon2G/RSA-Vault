using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Forms9Patch;
using Kit.Model;
using RSALibrary;
using RSAVault.Models;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class FromTextPageViewModel : ModelBase
    {
        private ICommand changeCertificateCommand;
        public ICommand ChangeCertificateCommand => changeCertificateCommand ??= new Command(ChangeCertificate);
        private ICommand shareCommand;
        public ICommand ShareCommand => shareCommand ??= new Command(Share);
        private ICommand saveAsNoteCommand;
        public ICommand SaveAsNoteCommand => saveAsNoteCommand ??= new Command(SaveAsNote);
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

        private Certificate _Certificate;

        public Certificate Certificate
        {
            get => _Certificate;
            set
            {
                _Certificate = value;
                Raise(()=> Certificate);
            }
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
        public async void ChangeCertificate()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            var certificates = new CertificatesPage();
            certificates.CertificateClicked = new Command<Certificate>(ChangeCertificate);
            await Shell.Current.Navigation.PushAsync(certificates, true);
        }
        private async void ChangeCertificate(Certificate Certificate)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            await Shell.Current.Navigation.PopAsync( true);
            this.Certificate = Certificate;
        }
    }
}
