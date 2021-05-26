using System.Linq;
using System.Windows.Input;
using Kit.Forms.Extensions;
using Kit.Forms.Pages;
using RSAVault.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;
using PopupNavigation = Rg.Plugins.Popup.Services.PopupNavigation;

namespace RSAVault.ViewModels
{
    public class RequestCameraPageViewModel
    {
        public ICommand ContinueCommand { get; set; }
        public RequestCameraPageViewModel()
        {
            this.ContinueCommand = new Command(Continue);
        }

        public static bool ShouldAsk()
        {
            if (Permisos.IsDisabled(new Permissions.Camera()))
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedCamera, AppResources.Alert, "Ok");
                return false;
            }
            if (Permisos.IsDisabled(new Permissions.Photos()))
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedPhotos, AppResources.Alert, "Ok");
                return false;
            }

            return true;
        }
        private async void Continue()
        {
            await Permisos.PedirPermiso(new Permissions.Camera(), AppResources.AllowAccess);
            await Permisos.PedirPermiso(new Permissions.Photos(), AppResources.AllowAccess);
            await (PopupNavigation.Instance.PopupStack.First() as BasePopUp)?.Close();
        }
    }
}
