using System;
using System.Net;
using System.ServiceModel;

namespace Kulva
{
    class ElisaKulva
    {
        private const ConsoleColor Yellow = ConsoleColor.Yellow;
        private const ConsoleColor Green = ConsoleColor.Green;
        private const ConsoleColor Red = ConsoleColor.Red;

        internal ElisaKulva()
        {
            if (Properties.Settings.Default.IgnoreCertWarnings)
            {
                ServicePointManager.ServerCertificateValidationCallback = ((s, c, ch, ss) => true);
            }
        }

        internal void SendHeartBeat()
        {
            string url = Properties.Settings.Default.Url;
            string clientId = Properties.Settings.Default.ClientID;
            string deviceId = Properties.Settings.Default.DeviceID;

            PrintColors("ClientID: ", clientId, Yellow, false);
            PrintColors("\tDeviceID: ", deviceId, Yellow);

            using (var client = new KulvaSvc.KulvaClient(CustomBindingCreator.Create(url), new EndpointAddress(url)))
            {
                bool heartBeatResult = client.HeartBeat(clientId, deviceId);
                PrintColors("Heartbeat: ", heartBeatResult + "\r\n", (heartBeatResult ? Green : Red));
            }
        }

        internal void SendAccessControlEvent(string cardId, string reasonCode, DateTime? endTime)
        {
            string url = Properties.Settings.Default.Url;
            string clientId = Properties.Settings.Default.ClientID;
            string deviceId = Properties.Settings.Default.DeviceID;

            //PrintColors("ClientID: ", clientId, Yellow, false);
            //PrintColors("\tDeviceID: ", deviceId, Yellow);

            PrintColors("CardID: ", cardId, Yellow, false);
            PrintColors("\tReason: ", reasonCode, Yellow, false);
            var startTime = DateTime.Now;
            PrintColors(" Event time: ", DateString(startTime, "HH:mm:ss"), ConsoleColor.Magenta, false);
            PrintColors(" End time: ", DateString(endTime), ConsoleColor.Magenta);

            using (var client = new KulvaSvc.KulvaClient(CustomBindingCreator.Create(url), new EndpointAddress(url)))
            {
                string stampResult = client.Stamp(clientId, deviceId, cardId, reasonCode, startTime, endTime);
                PrintColors("Access Control Stamp: ", stampResult + "\r\n", (stampResult == "OK" ? Green : Red));
            }
        }

        static private void PrintColors(string text,  string text2, ConsoleColor color, bool newline = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
            Console.ForegroundColor = color;
            if (newline) {
                Console.WriteLine(text2);
                Console.ResetColor();
            } else { Console.Write(text2); }
        }

        static private string DateString(DateTime? dt, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (!dt.HasValue) { return string.Empty; }
            return dt.Value.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}