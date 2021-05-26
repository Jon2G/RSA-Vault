using Xamarin.Forms.Xaml;

namespace RSAVault.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestCameraPage 
    {
        public RequestCameraPage()
        {
            this.LockModal();
            InitializeComponent();
        }

    }
}