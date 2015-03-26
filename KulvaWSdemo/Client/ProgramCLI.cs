using System;

namespace Kulva
{
    class ProgramCLI
    {
        static void Main()
        {
            var ek = new ElisaKulva();
            ek.SendHeartBeat();
            System.Threading.Thread.Sleep(500);

            ek.SendAccessControlEvent("800", "02", null);
            System.Threading.Thread.Sleep(500);

            var dt = DateTime.Now;
            ek.SendAccessControlEvent("800", "03", dt.AddHours(1));
            ek.SendAccessControlEvent("802", "02", null);
            dt = dt.AddDays(14);
            ek.SendAccessControlEvent("803", "02", new DateTime(dt.Year, dt.Month, dt.Day));
            
            System.Threading.Thread.Sleep(2000);
            ek.SendAccessControlEvent("13", "02", null);

            Console.WriteLine(" -- press any key to exit --");
            Console.ReadKey();
        }
    }
}