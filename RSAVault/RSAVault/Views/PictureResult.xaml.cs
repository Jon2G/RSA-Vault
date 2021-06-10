using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSAVault.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureResult 
    {
        public PictureResult(PictureResultViewModel model)
        {
            this.BindingContext = model;
            InitializeComponent();
        }
    }
}