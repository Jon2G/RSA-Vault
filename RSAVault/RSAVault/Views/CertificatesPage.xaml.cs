using RSALibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CertificatesPage : ContentPage
    {
        private Command<Certificate> _CertificateClicked;

        public Command<Certificate> CertificateClicked
        {
            get => _CertificateClicked;
            set
            {
                _CertificateClicked = value;
                OnPropertyChanged();
            }
        }

        public CertificatesPage()
        {
            InitializeComponent();
        }
    }
}