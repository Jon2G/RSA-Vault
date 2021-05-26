using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;
using System.Windows.Input;
using Forms9Patch;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
  public class CertificatesPageViewModel:ModelBase
    {
        private ICommand newCertificateCommand;
        public ICommand NewCertificateCommand => newCertificateCommand ??= new Command(NewCertificate);

        public CertificatesPageViewModel()
        {
            
        }

        private void NewCertificate()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick,EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            Shell.Current.Navigation.PushAsync(new CertificatePage());
        }
    }
}
