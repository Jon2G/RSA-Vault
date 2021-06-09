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
    public partial class CertificatesPage : ContentPage
    {


        public CertificatesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(this.Model.Refresh);
        }
    }
}