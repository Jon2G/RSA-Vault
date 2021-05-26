using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Kit.Model;

namespace RSALibrary
{
    public sealed class Certificate : ModelBase
    {
        private string _Name;

        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                Raise(() => _Name);
            }
        }

        public string DisplayEndDate => EndDate.ToString(CultureInfo.CurrentCulture);
        private DateTime _EndDate;

        public DateTime EndDate
        {
            get => _EndDate;
            set
            {
                _EndDate = value;
                Raise(() => EndDate);
                Raise(() => DisplayEndDate);
            }
        }
        public Certificate(string Name, string key_path, string certificate_path, string password)
        {
            this.Name = Name;
        }
    }
}
