using System;
using System.Threading.Tasks;
using Kit.Forms.Services.Interfaces;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using RSAVault.Data;
using RSAVault.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {

        public SplashScreen()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Run(() => { while (Logo.IsLoading) { } });
            AppData.Init();
            AppShell shell = new AppShell();
            Settings settings = Settings.Get();
            if (settings.IsFingerPrintActive)
            {
                if (!await CrossFingerprint.Current.IsAvailableAsync(true))
                {
                    settings.IsFingerPrintActive = false;
                    settings.Save();
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("La autenticación biométrica no esta disponible  o no esta configurada.", "Atención", "OK");
                    OnAppearing();
                }
                var authResult = await Xamarin.Forms.Device.InvokeOnMainThreadAsync(() =>
                CrossFingerprint.Current.AuthenticateAsync(
                    new AuthenticationRequestConfiguration(RSAVault.Resources.AppResources.LockTitle,
                    RSAVault.Resources.AppResources.LockReason)
                    {
                        AllowAlternativeAuthentication = true
                    }, new System.Threading.CancellationToken(false)));

                if (authResult.Authenticated)
                {
                    GotoApp(shell);
                }
            }
            else
            {
                GotoApp(shell);
            }

        }

        private void GotoApp(AppShell shell)
        {
            DependencyService.Get<IRSA>().TestKey(KeyChain.PersonalKey);

            App.Current.MainPage = shell;
        }
    }
}