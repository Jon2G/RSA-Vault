using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Model;
using System.Windows.Input;
using Forms9Patch;
using RSAVault.Data;
using RSAVault.Models;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Kit.Forms.Security.RSA;

namespace RSAVault.ViewModels
{
    public class KeyChainPageViewModel : ModelBase
    {
        private Command<Key> _KeyClickedCommand;

        public Command<Key> KeyClickedCommand
        {
            get => _KeyClickedCommand;
            set
            {
                _KeyClickedCommand = value;
                Raise(() => KeyClickedCommand);
            }
        }
        private ICommand _NewKeyCommand;
        public ICommand NewKeyCommand => _NewKeyCommand ??= new Command(NewKey);
        private List<Key> _Keys;
        public List<Key> Keys
        {
            get => _Keys;
            set
            {
                _Keys = value;
                Raise(() => Keys);
            }
        }
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set
            {
                _Loading = value;
                Raise(() => Loading);
            }
        }
        private bool _IsEmpty;
        public bool IsEmpty
        {
            get => _IsEmpty;
            set
            {
                _IsEmpty = value;
                Raise(() => IsEmpty);
            }
        }

        public KeyChainPageViewModel()
        {
           
        }


        internal Task Refresh()
        {
            this.Loading = true;
            IsEmpty = false;
            try
            {
                this.Keys = new List<Key>(KeyChain.GetKeys());
            }
            finally
            {
                IsEmpty = !this.Keys.Any();
                this.Loading = false;
            }
            return Task.CompletedTask;
        }


        private void NewKey()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            Shell.Current.Navigation.PushAsync(new CertificatePage());
        }
    }
}
