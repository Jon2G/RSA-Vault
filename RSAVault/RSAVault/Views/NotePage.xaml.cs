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
    public partial class NotePage : ContentPage
    {
        public Note Note { get; set; }
        public NotePage(Note Note)
        {
            this.Note = Note;
            this.BindingContext = this.Note;
            InitializeComponent();
        }
    }
}