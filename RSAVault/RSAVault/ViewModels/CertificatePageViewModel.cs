using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Forms9Patch;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class CertificatePageViewModel : ModelBase
    {
        private string _Name;
        public string Name
        {
            get => _Name;
            set => _Name = value;
        }

        private int _Months;

        public int Months
        {
            get => _Months;
            set
            {
                _Months = value;
                Raise(()=>Months);
            }
        }

        private string _Password;

        public string Password
        {
            get => _Password;
            set => _Password = value;
        }

        private ICommand newCertificateCommand;
        public ICommand NewCertificateCommand => newCertificateCommand ??= new Command(NewCertificate);

        public CertificatePageViewModel()
        {
            Months = 6;
        }
        private void NewCertificate()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
        }
    }
}
