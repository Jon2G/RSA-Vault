using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kit;
using Kit.Extensions;

namespace RSALibrary
{
    public class CertificateBuilder
    {

        public static async Task<Certificate> MakeCert(string name, string password = null)
        {
            Certificate certificate=null;
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = Password.Generate(8, 4);
                }
                var ECDsa = System.Security.Cryptography.ECDsa.Create(); // generate asymmetric key pair
                var request = new CertificateRequest("cn=foobar", ECDsa, HashAlgorithmName.SHA256);
                var cert = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));


                string key_path =Path.Combine(Vault.VaultDirectory.FullName, $"{name}.pfx");
                using (FileStream CertFile = new FileStream(key_path, FileMode.OpenOrCreate))
                {
                    // Create PFX (PKCS #12) with private key
                    using (MemoryStream memory = new MemoryStream(cert.Export(X509ContentType.Pfx, password)))
                    {
                        await memory.CopyToAsync(CertFile);
                    }
                }


                string certificate_path = Path.Combine(Vault.VaultDirectory.FullName, $"{name}.cer");
                using (FileStream CertFile = new FileStream(certificate_path, FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(CertFile,Encoding.UTF8))
                    {
                        await writer.WriteAsync("-----BEGIN CERTIFICATE-----\r\n");
                        await writer.WriteAsync(Convert.ToBase64String(cert.Export(X509ContentType.Cert),
                            Base64FormattingOptions.InsertLineBreaks));
                        await writer.WriteAsync("\r\n-----END CERTIFICATE-----");
                    }
                    // Create PFX (PKCS #12) with private key
                    using (MemoryStream memory = new MemoryStream(cert.Export(X509ContentType.Cert, password)))
                    {
                        await memory.CopyToAsync(CertFile);
                    }
                }
                certificate = new Certificate(name,key_path, certificate_path, password);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "While creating a new certificate");
            }

            return certificate;
        }
    }
}
