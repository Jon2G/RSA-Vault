﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Kit;
using Kit.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class AboutPageViewModel : ModelBase
    {
        public ICommand BuyMeACoffeCommand { get; }
        public ICommand GMailCommand { get; }
        public ICommand XamarinCommand { get; }
        public ICommand GitHubCommand { get; }
        public ICommand ReportBugCommand { get; }
        public ICommand RequestFeatureCommand { get; }
        public AboutPageViewModel()
        {
            this.BuyMeACoffeCommand = new Command(BuyMeACoffe);
            this.XamarinCommand = new Command(Xamarin);
            this.GitHubCommand = new Command(GitHub);
            this.ReportBugCommand = new Command(ReportBug);
            this.RequestFeatureCommand = new Command(RequestFeature);
            this.GMailCommand = new Command(GMail);
        }

        private async void GMail()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "RSA Vault App",
                    Body = "Hi",
                    To = new List<string>() { "jonathan.edu.gar@gmail.com" },
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, nameof(GMail));
            }

        }
        private void ReportBug() => OpenBrowser("https://github.com/Jon2G/RSA-Vault/issues/new?assignees=&labels=&template=bug_report.md&title=");
        private void RequestFeature() => OpenBrowser("https://github.com/Jon2G/RSA-Vault/issues/new?assignees=&labels=&template=feature_request.md&title=");
        private void GitHub() => OpenBrowser("https://github.com/Jon2G/RSA-Vault");
        private void Xamarin() => OpenBrowser("https://dotnet.microsoft.com/learn/xamarin/what-is-xamarin");

        private void BuyMeACoffe() => OpenBrowser("https://www.buymeacoffee.com/JonGG");



        private async void OpenBrowser(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, nameof(OpenBrowser));
            }
        }
    }
}