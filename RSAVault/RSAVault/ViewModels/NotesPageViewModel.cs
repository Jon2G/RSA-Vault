using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;
using System.Windows.Input;
using Forms9Patch;
using RSAVault.Models;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class NotesPageViewModel:ModelBase
    {

        private ICommand viewNoteCommand;
        public ICommand ViewNoteCommand => viewNoteCommand ??= new Command<Note>(ViewNote);

        private void ViewNote(Note note)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            Shell.Current.Navigation.PushAsync(new NotePage(note));
        }

        private ICommand addCommand;
        public ICommand AddCommand => addCommand ??= new Command(Add);

        private void Add()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            Shell.Current.Navigation.PushAsync(new NotePage(new Note()));
        }
    }
}
