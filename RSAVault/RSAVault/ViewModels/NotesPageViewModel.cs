using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Model;
using System.Windows.Input;
using Forms9Patch;
using RSAVault.Models;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class NotesPageViewModel : ModelBase
    {

        private bool _IsLocked;
        public bool IsLocked
        {
            get => _IsLocked;
            set
            {
                _IsLocked = value;
                Raise(() => IsLocked);
            }
        }

        private ICommand viewNoteCommand;
        public ICommand ViewNoteCommand => viewNoteCommand ??= new Command<Note>(ViewNote);
        private List<Note> _Notes;
        public List<Note> Notes
        {
            get => _Notes;
            set
            {
                _Notes = value;
                Raise(() => Notes);
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

        public NotesPageViewModel()
        {
            this.IsLocked = true;
        }
        internal Task Refresh()
        {
            this.Loading = true;
            IsEmpty = false;
            try
            {
                this.Notes = new List<Note>(Vault.GetNotes());
            }
            finally
            {
                IsEmpty = !this.Notes.Any();
                this.Loading = false;
            }
            return Task.CompletedTask;
        }
    }
}
