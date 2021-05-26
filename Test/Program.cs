using System;
using RSALibrary;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Kit.NetCore.Tools.Init();
            TestMakeCert();
            Console.WriteLine("Hello World!");
        }

        private static async void TestMakeCert()
        {
            Certificate certificate = await CertificateBuilder.MakeCert("mycert");
        }
    }
}
