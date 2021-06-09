using System.Threading;
using System.Windows.Input;
using Forms9Patch;
using Kit.Model;
using RSAVault.Resources;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Settings = RSAVault.Models.Settings;

namespace RSAVault.ViewModels
{
    public class MainPageViewModel : ModelBase
    {
        private ICommand _CertificatesCommand;
        public ICommand CertificatesCommand => _CertificatesCommand ??= new Command(Certificates);
        private ICommand _NotesCommand;
        public ICommand NotesCommand => _NotesCommand ??= new Command(Notes);
        private ICommand _FromTextCommand;
        public ICommand FromTextCommand => _FromTextCommand ??= new Command(FromText);
        private ICommand _FromPictureCommand;
        public ICommand FromPictureCommand => _FromPictureCommand ??= new Command(FromPicture);
        private ICommand _FingerPrintCommand;
        public ICommand FingerPrintCommand => _FingerPrintCommand ??= new Command(FingerPrint);
        private ICommand _AboutCommand;
        public ICommand AboutCommand => _AboutCommand ??= new Command(About);
        private ICommand _ChangeLanguajeCommand;
        public ICommand ChangeLanguajeCommand => _ChangeLanguajeCommand ??= new Command(ChangeLanguaje);
        private Settings _Settings;

        public Settings Settings
        {
            get => _Settings;
            set
            {
                _Settings = value;
                Raise(() => Settings);
            }
        }

        public MainPageViewModel()
        {
            GetSettings();
        }

        private void GetSettings()
        {
            Settings = Settings.Get();
        }

        private async void Certificates() => await Shell.Current.Navigation.PushAsync(new KeysPage(), true);

        private async void Notes() => await Shell.Current.Navigation.PushAsync(new NotesPage(), true);

        private async void FromText() => await Shell.Current.Navigation.PushAsync(new FromTextPage(), true);

        private async void FromPicture() => await Shell.Current.Navigation.PushAsync(new FromPicturePage(), true);

        private async void About() => await Shell.Current.Navigation.PushAsync(new AboutPage(), true);

        private void FingerPrint()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            this.Settings.IsFingerPrintActive = !this.Settings.IsFingerPrintActive;
        }

        private void ChangeLanguaje()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            if (AppResources.Culture.TwoLetterISOLanguageName == "en")
            {
                Thread.CurrentThread.CurrentUICulture = App.MexCultureInfo;
                AppResources.Culture = App.MexCultureInfo;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = App.UsaCultureInfo;
                AppResources.Culture = App.UsaCultureInfo;
            }
            App.Current.MainPage = new AppShell();
        }
    }
}
