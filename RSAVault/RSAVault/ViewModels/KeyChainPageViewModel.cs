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

namespace RSAVault.ViewModels
{
    public class KeyChainPageViewModel : ModelBase
    {
        private Command<KeyContainer> _KeyClickedCommand;

        public Command<KeyContainer> KeyClickedCommand
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
        private ICommand _DeleteCommand;
        public ICommand DeleteCommand => _DeleteCommand ??= new Command<KeyContainer>(Delete);
        private ICommand _ShareCommand;
        public ICommand ShareCommand => _ShareCommand ??= new Command<KeyContainer>(Share);
        private List<KeyContainer> _Keys;
        public List<KeyContainer> Keys
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
            KeyClickedCommand = new Command<KeyContainer>(KeyClicked);
        }

        private void KeyClicked(KeyContainer key)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            Shell.Current.Navigation.PushAsync(new KeyPage(key));
        }


        internal Task Refresh()
        {
            this.Loading = true;
            IsEmpty = false;
            try
            {
                this.Keys = new List<KeyContainer>(KeyChain.GetKeys());
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
            Shell.Current.Navigation.PushAsync(new KeyPage());
        }
        private void Delete(KeyContainer key)
        {
            KeyChain.Delete(key);
           Task.Run(Refresh);
        }

        private void Share(KeyContainer key) => KeyChain.Share(key);

    }
}
