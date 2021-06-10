using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RSAVault.Models;
using RSAVault.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeyPage 
    {
        public KeyPageViewModel Model { get; set; }
        public KeyPage(KeyContainer KeyContainer)
        {
            Model = new KeyPageViewModel(KeyContainer);
            this.BindingContext = this.Model;
            InitializeComponent();
        }
        public KeyPage():this(KeyChain.MakeKey(string.Empty, RSAEncryptionPadding.Pkcs1))
        {
       
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
            KeyChain.Save(Model.Key);
        }

    }
}