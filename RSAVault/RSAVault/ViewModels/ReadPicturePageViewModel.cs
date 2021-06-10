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
using Kit.Forms.Services;
using Kit.Model;
using RSAVault.Models;
using RSAVault.Resources;
using RSAVault.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace RSAVault.ViewModels
{
    public class ReadPicturePageViewModel : ModelBase
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

        public ICommand CleanCommand { get; set; }

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

        private KeyContainer _Key;

        public KeyContainer Key
        {
            get => _Key;
            set
            {
                _Key = value;
                Raise(() => Key);
            }
        }

        public ReadPicturePageViewModel()
        {
            this.Key = KeyChain.PersonalKey;
            this.PickFileCommand = new Command(PickFile);
            this.CalculateCommand = new Xamarin.Forms.Command(Calculate);
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
        private async void Calculate()
        {
            if (Image is null)
            {
                return;
            }
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.HiddingPicture))
            {
                string text = await SteganographyHelper.ExtractText(Image);
                this.Text = this.Key.Decrypt(text);
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

        public async void ChangeCertificate()
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            var keys = new KeysPage();
            keys.Model.KeyClickedCommand = new Xamarin.Forms.Command<KeyContainer>(ChangeCertificate);
            await Shell.Current.Navigation.PushAsync(keys, true);
        }
        private async void ChangeCertificate(KeyContainer Key)
        {
            Forms9Patch.Audio.PlaySoundEffect(SoundEffect.KeyClick, EffectMode.On);
            HapticFeedback.Perform(HapticFeedbackType.Click);
            await Shell.Current.Navigation.PopAsync(true);
            this.Key = Key;
        }
    }
}
