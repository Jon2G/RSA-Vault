using System;
using System.IO;
using Kit.Forms.Security.RSA;
using Kit.Model;
using Kit.Sql.Sqlite;
using RSAVault.Models;

namespace RSAVault.Data
{
    public class AppData : ModelBase
    {
        public static AppData Instance { get; private set; }

        public SQLiteConnection LiteConnection { get; private set; }
        private AppData()
        {

        }

        private static FileInfo LiteDbPath => new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SchoolOrganizer.db"));

        public static void Init()
        {
            AppData.Instance = new AppData
            {
                LiteConnection = new SQLiteConnection(LiteDbPath, 107)
            };
            AppData.Instance.LiteConnection.CheckTables(typeof(Settings),typeof(Key), typeof(Note));
        }
    }
}
