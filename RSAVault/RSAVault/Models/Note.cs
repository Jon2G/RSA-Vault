using System;
using System.Globalization;
using Kit.Model;
using Kit.Sql.Attributes;

namespace RSAVault.Models
{
    public class Note : ModelBase
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
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

        public Note()
        {
            LastModificationTime=DateTime.Now;
            
        }
    }
}