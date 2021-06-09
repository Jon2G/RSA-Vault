using System;
using System.Globalization;
using Kit.Model;
using Kit.Sql.Attributes;
using Kit.Sql.Interfaces;

namespace RSAVault.Models
{
    public class Note : ModelBase, IGuid
    {
        [PrimaryKey]
        public Guid Guid { get; set; }

        private string _Text;

        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                Raise(() => IsEmpty);
                Raise(() => Text);
            }
        }

        private string _Title;

        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                Raise(() => Title);
                Raise(() => IsEmpty);
            }
        }

        public string DisplayLastModificationTime => LastModificationTime.ToString(CultureInfo.CurrentCulture);
        private DateTime _LastModificationTime;

        public DateTime LastModificationTime
        {
            get => _LastModificationTime;
            set
            {
                _LastModificationTime = value;
                Raise(() => LastModificationTime);
                Raise(() => DisplayLastModificationTime);
            }
        }

        public bool IsEmpty => string.IsNullOrEmpty(Title?.Trim()) && string.IsNullOrEmpty(Text?.Trim()) && Guid == Guid.Empty;

        public Note()
        {
            LastModificationTime = DateTime.Now;

        }


    }
}