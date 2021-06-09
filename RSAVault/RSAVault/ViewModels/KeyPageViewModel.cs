using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Forms9Patch;
using RSAVault.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Kit.Forms.Security.RSA;

namespace RSAVault.ViewModels
{
    public class KeyPageViewModel : ModelBase
    {
        private Key _Key;
        public Key Key
        {
            get => _Key;
            set
            {
                _Key = value;
                Raise(() => Key);
            }
        }

        public KeyPageViewModel()
        {
            this.Key = KeyChain.MakeKey(string.Empty);
        }
    }
}
