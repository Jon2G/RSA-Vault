using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Forms;
using Forms9Patch;
using Kit;
using Kit.Extensions;
using Kit.Forms.Extensions;
using Kit.Forms.Security.RSA;
using Kit.Forms.Services;
using Kit.Model;
using RSAVault.Resources;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace RSAVault.ViewModels
{
    public class FromPicturePageViewModel : ModelBase
    {
        private ICommand changeCertificateCommand;
        public ICommand ChangeCertificateCommand => changeCertificateCommand ??= new Command(ChangeCertificate);

        private FileImageSource _Image;
        public FileImageSource Image
        {
            get => _Image;
            set
            {
                _Image = value;
                Raise(() => Image);
            }
        }
        public ICommand PickFileCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        private ICommand shareCommand;
        public ICommand ShareCommand => shareCommand ??= new Command(Share);

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

        private Key _Certificate;

        public Key Certificate
        {
            get => _Certificate;
            set
            {
                _Certificate = value;
                Raise(() => Certificate);
            }
        }

        public FromPicturePageViewModel()
        {
            this.PickFileCommand = new Command(PickFile);
            this.CalculateCommand = new Xamarin.Forms.Command<CachedImage>(Calculate);
            this.TakePhotoCommand = new Command(TakePhoto);
            this.CleanCommand = new Command(Clean);
        }
        private async void Clean()
        {
            if (await Acr.UserDialogs.UserDialogs.Instance.
                ConfirmAsync(AppResources.CleanTextAsk,
                    AppResources.Alert, AppResources.Yes, AppResources.Cancel))
            {
                this.Image = null;
            }
        }
        private async void Calculate(CachedImage Image)
        {
            if (Image?.Source is null)
            {
                return;
            }

        }
        private async void TakePhoto()
        {
            var permiso = new Permissions.Camera();
            if (!await Permisos.TenemosPermiso(permiso))
            {
                RequestCameraPage request = new RequestCameraPage();
                await request.ShowDialog();
                if (await permiso.CheckStatusAsync() != PermissionStatus.Granted)
                {
                    await Task.Delay(500);
                    Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedCamera,
                        AppResources.Alert);
                    return;
                }
                await Task.Delay(500);
            }
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo is not null)
            {
                await Task.Delay(500);
                await ReadFile(photo);
            }
        }

        private async void PickFile()
        {
            await Task.Yield();
            try
            {
                var permiso = new Permissions.Photos();
                if (!await Permisos.TenemosPermiso(new Permissions.Photos()))
                {
                    RequestCameraPage request = new RequestCameraPage();
                    await request.ShowDialog();
                    if (await permiso.CheckStatusAsync() != PermissionStatus.Granted)
                    {
                        await Task.Delay(500);
                        Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedCamera,
                            AppResources.Alert);
                        return;
                    }
                    await Task.Delay(500);
                }
                var pfile = await MediaPicker.PickPhotoAsync();
                if (pfile is not null)
                {
                    await Task.Delay(500);
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
                    {
                        await ReadFile(pfile);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "While picking file");
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.ErrorPickingFile,
                    "Lo sentimos...", "Ok");
            }

        }
        private async Task ReadFile(FileResult pfile)
        {
            await Task.Yield();
            this.Image = null;
            if (!pfile.ContentType.Contains("image"))
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(
                    AppResources.ItsAFileNotAnImage, AppResources.Alert,
                    AppResources.Yes);
            }
            FileInfo file = await pfile.LoadPhotoAsync();
            this.Image = (FileImageSource)FileImageSource.FromFile(file.FullName);
        }
        public void Share()
        {

        }
        public async void ChangeCertificate()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            var certificates = new CertificatesPage();
            certificates.Model.KeyClickedCommand = new Xamarin.Forms.Command<Key>(ChangeCertificate);
            await Shell.Current.Navigation.PushAsync(certificates, true);
        }
        private async void ChangeCertificate(Key Certificate)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            await Shell.Current.Navigation.PopAsync(true);
            this.Certificate = Certificate;
        }
    }
}
