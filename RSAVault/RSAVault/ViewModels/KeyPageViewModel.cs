using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Input;
using Forms9Patch;

using RSAVault.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RSAVault.ViewModels
{
    public class KeyPageViewModel : ModelBase
    {
        private KeyContainer _Key;
        public KeyContainer Key
        {
            get => _Key;
            set
            {
                _Key = value;
                Raise(() => Key);
            }
        }

        public KeyPageViewModel(KeyContainer Key)
        {
            this.Key = Key;

        }
    }
}
