using System;

namespace Kulva
{
    class ProgramCLI
    {
        static void Main()
        {
            System.Threading.Thread.Sleep(2000); //wait for WCF server startup
            Console.WriteLine("...");
            var ek = new KulvaTalker();
            ek.SendHeartBeat();
            System.Threading.Thread.Sleep(500);

            ek.SendAccessControlEvent("800", "02", null);
            System.Threading.Thread.Sleep(500);

            var dt = DateTime.Now;
            ek.SendAccessControlEvent("800", "03", dt.AddHours(1));


            System.Threading.Thread.Sleep(500);
            //multiple ACS events:
            var e = new System.Collections.Generic.List<KulvaSvc.Entry>();
            e.Add(KulvaTalker.CreateStampEntry("802", "03", null));
            e.Add(KulvaTalker.CreateStampEntry("844", "03", null));
            e.Add(KulvaTalker.CreateStampEntry("847", "03", null));
            dt = dt.AddDays(14);
            e.Add(KulvaTalker.CreateStampEntry("702", "20", dt));
            dt = dt.AddDays(7);
            e.Add(KulvaTalker.CreateStampEntry("714", "20", dt));
            e.Add(KulvaTalker.CreateStampEntry("708", "03", null));
            e.Add(KulvaTalker.CreateStampEntry("650", "03", null));
            ek.SendMultipleAccessControlEvents(e.ToArray());

            Console.WriteLine(" -- press any key to exit --");
            Console.ReadKey();
        }
    }
}