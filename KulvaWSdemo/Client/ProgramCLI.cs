using System;

namespace Kulva
{
    class ProgramCLI
    {
        static void Main()
        {
            System.Threading.Thread.Sleep(2000); //wait for WCF server startup

            Random r = new Random();
            KulvaTalker ek = new KulvaTalker();
            
            ek.SendHeartBeat();
            System.Threading.Thread.Sleep(500);


            ek.SendAccessControlEvent(CardId(r), ReasonCode(r), null);
            System.Threading.Thread.Sleep(500);

            var dt = DateTime.Now;
            ek.SendAccessControlEvent(CardId(r), ReasonCode(r), dt.AddHours(1));


            System.Threading.Thread.Sleep(500);

            // Multiple ACS events:
            var e = new System.Collections.Generic.List<KulvaSvc.Entry>();
            for (int i = 0; i < r.Next(5, 15); i++)
            {
                e.Add(KulvaTalker.CreateStampEntry(CardId(r), ReasonCode(r), EndTime(r)));
            }
            ek.SendMultipleAccessControlEvents(e.ToArray());

            Console.WriteLine(" -- press any key to exit --");
            Console.ReadKey();
        }
        private static string CardId(Random r) { return r.Next(1, 1000).ToString(); }
        private static string ReasonCode(Random r) { return r.Next(0, 15).ToString(); }
        private static DateTime? EndTime(Random r)
        {
            if (r.NextDouble() < 0.6) { return null; }
            return DateTime.Now.AddDays(r.Next(1, 21)).Date;
        }
    }
}