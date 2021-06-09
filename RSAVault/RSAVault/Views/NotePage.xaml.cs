using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RSAVault.Models;
using RSAVault.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        public NotePageViewModel Model { get; set; }
        public ICommand LockCommand { get; set; }
        public NotePage(Note Note)
        {
            this.Model = new NotePageViewModel(Note);
            this.BindingContext = this.Model;
            this.LockCommand = new Command(Lock);
            InitializeComponent();
        }

 
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!this.Model.Note.IsEmpty)
            {
                this.Editor.Text = this.Model.Note.Text;

                await this.LockSeal.FadeTo(.5, 1000);
                Vault.Decrypt(this.Model.Note);
                this.Model.IsLocked = false;
                await this.LockSeal.FadeTo(0, 1000);
                this.Editor.Focus();
                //await Task.Delay(500);
                string previoustext = this.Editor.Text;
                this.Editor.Text += "@";
                //await Task.Delay(100);
                this.Editor.Text = previoustext;
            }
            else {
                this.Model.IsLocked = false;
                await this.LockSeal.FadeTo(0, 1000); 
                this.Editor.Focus();
            }
        }
        private async void Lock()
        {
            await this.LockSeal.FadeTo(.5, 1000);
            Vault.SaveNote(this.Model.Note);
            this.Model.IsLocked = true;
            await this.LockSeal.FadeTo(1, 1000);
            await Shell.Current.Navigation.PopAsync(true);
        }

        protected override bool OnBackButtonPressed()
        {
            if (!this.Model.Note.IsEmpty)
            {
                Lock();
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private void Image_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Image.Source))
            {
                var item = ToolbarItems.First();
                ToolbarItems.Clear();
                item.IconImageSource = (sender as Image).Source;
                ToolbarItems.Add(item);
            }

        }
    }
}