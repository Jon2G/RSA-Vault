using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSAVault.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CertificatePage : ContentPage
    {
        public CertificatePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            KeyChain.Save(Model.Key);
        }
    }
}