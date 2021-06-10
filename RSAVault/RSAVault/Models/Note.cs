using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Kit;
using Kit.Forms.Extensions;
using Kit.Model;
using Kit.Sql.Attributes;
using Kit.Sql.Interfaces;
using RSAVault.Resources;
using Xamarin.Essentials;

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
                if (value != _Text)
                {
                    _Text = value;
                    Raise(() => IsEmpty);
                    Raise(() => Text);
                }
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

        internal async void Share()
        {
            if (!await Permisos.RequestStorage())
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedStorage);
                return;
            }
            FileInfo file = new FileInfo(Path.Combine(Tools.Instance.TemporalPath, $"{Guid.NewGuid():N}.txt"));
            if (file.Exists)
            {
                file.Delete();
            }
            using (FileStream mStream = new FileStream(file.FullName, FileMode.OpenOrCreate))
            {
                using (TextWriter writer = new StreamWriter(mStream, Encoding.UTF8))
                {
                    try
                    {
                        await writer.WriteAsync(this.Text);
                        await writer.FlushAsync();
                        await mStream.FlushAsync();

                    }
                    catch (XmlException ex)
                    {
                        Log.Logger.Error(ex, "KeyChain.Share");
                    }
                }
            }

            ShareFileRequest request = new ShareFileRequest(new ShareFile(file.FullName));
            await Xamarin.Essentials.Share.RequestAsync(request);

        }
    }
}