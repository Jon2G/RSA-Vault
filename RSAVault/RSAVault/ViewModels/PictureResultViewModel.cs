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
    public class PictureResultViewModel : ModelBase
    {
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
     
        private ICommand shareCommand;
        public ICommand ShareCommand => shareCommand ??= new Command(Share);

        private string _Encrypted;
        public string Encrypted
        {
            get => _Encrypted;
            set
            {
                _Encrypted = value;
                Raise(() => Encrypted);
            }
        }

        public PictureResultViewModel(FileImageSource image,string encrypted)
        {
            this.Image = image;
            this.Encrypted = encrypted;
        }
        public void Share()
        {
            ShareFileRequest request = new ShareFileRequest(new ShareFile(Image.File));
            Xamarin.Essentials.Share.RequestAsync(request);
        }

    }
}
