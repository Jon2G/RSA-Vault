using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Kit;

namespace RSALibrary
{
    public static class Vault
    {
        public static DirectoryInfo VaultDirectory =>
            new DirectoryInfo(Path.Combine(Tools.Instance.LibraryPath, "Vault"));
        static Vault()
        {
            if (!VaultDirectory.Exists)
            {
                VaultDirectory.Create();
            }
        }
    }
}
