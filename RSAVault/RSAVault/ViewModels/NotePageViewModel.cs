using Kit.Model;
using RSAVault.Fonts;
using RSAVault.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RSAVault.ViewModels
{
    public class NotePageViewModel : ModelBase
    {
        public string LockIcon => IsLocked ? FontelloIcons.LockClosed : FontelloIcons.LockOpen;

        private bool _IsLocked;
        public bool IsLocked
        {
            get => _IsLocked; set
            {
                _IsLocked = value;
                Raise(() => IsLocked);
                Raise(() => LockIcon);
            }
        }
        public Note Note { get; set; }
        public NotePageViewModel(Note Note)
        {
            this.Note = Note;
            this.IsLocked = true;
        }


      
    }
}