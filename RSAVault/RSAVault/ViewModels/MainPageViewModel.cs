using System.Threading;
using System.Windows.Input;
using Forms9Patch;
using Kit.Model;
using RSAVault.Resources;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class MainPageViewModel : ModelBase
    {
        public ICommand CertificatesCommand { get; set; }
        public ICommand NotesCommand { get; set; }
        public ICommand FromTextCommand { get; set; }
        public ICommand FromPictureCommand { get; set; }
        public ICommand FingerPrintCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand ChangeLanguajeCommand { get; set; }
        public MainPageViewModel()
        {
            this.CertificatesCommand = new Command(Certificates);
            this.NotesCommand = new Command(Notes);
            this.AboutCommand = new Command(About);
            this.FromTextCommand = new Command(FromText);
            this.FromPictureCommand = new Command(FromPicture);
            this.FingerPrintCommand = new Command(FingerPrint);
            this.ChangeLanguajeCommand = new Command(ChangeLanguaje);
        }

        private async void Certificates() => await Shell.Current.Navigation.PushAsync(new CertificatesPage(), true);

        private async void Notes() => await Shell.Current.Navigation.PushAsync(new NotesPage(), true);

        private async void FromText() => await Shell.Current.Navigation.PushAsync(new FromTextPage(), true);

        private async void FromPicture() => await Shell.Current.Navigation.PushAsync(new FromPicturePage(), true);

        private async void About() => await Shell.Current.Navigation.PushAsync(new AboutPage(), true);

        private async void FingerPrint()
        {
            
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
