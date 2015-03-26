using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace KulvaServer
{
    class ProgramSRV
    {
        static void Main()
        {
            //try
            //{
                //netsh http add urlacl url=http://+:4321/Kulva user=DEV\hynnila
                var httpBaseAddress = new Uri("http://localhost:4321/Kulva/Kulva.svc");

                using (var host = new ServiceHost(typeof (Kulva),new[] {httpBaseAddress}))
                {
                    host.AddServiceEndpoint(typeof (IKulva), new BasicHttpBinding(), "");
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
                    host.Open();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Service is now running at : {0}", httpBaseAddress);
                    Console.WriteLine("- press any key to exit -");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            //}
            //catch (AddressAccessDeniedException e)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine(e.Message);
            //    Console.WriteLine(@"netsh http add urlacl url=http://+:4321/Kulva user=DEV\hynnila");
            //    Console.ResetColor();
            //}
        }
    }
}