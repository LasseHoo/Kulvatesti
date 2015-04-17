using System;
using System.Net;
using System.ServiceModel;

namespace Kulva
{
    class KulvaTalker
    {
        private const ConsoleColor Yellow = ConsoleColor.Yellow;
        private const ConsoleColor Green = ConsoleColor.Green;
        private const ConsoleColor Red = ConsoleColor.Red;

        private readonly System.ServiceModel.Channels.Binding _bind;
        private readonly EndpointAddress _endpoint;
        private readonly string _username, _password;
        internal KulvaTalker()
        {
            if (Properties.Settings.Default.IgnoreCertWarnings)
            {
                ServicePointManager.ServerCertificateValidationCallback = ((s, c, ch, ss) => true);
            }
            var url = Properties.Settings.Default.Url;
            _endpoint = new EndpointAddress(url);
            _bind = CustomBindingCreator.Create(url);
            if (url.StartsWith("https")) //local test server is http://, production test is https://
            {
                _username = Properties.Settings.Default.ProdTestUsername;
                _password = Properties.Settings.Default.ProdTestPassword;
            }
        }

        private static KulvaSvc.Identity Identity
        {
            get
            {
                string clientId = Properties.Settings.Default.ClientID;
                string deviceId = Properties.Settings.Default.DeviceID;
                return new KulvaSvc.Identity {ClientId = clientId, DeviceId = deviceId};
            }
        }

        internal void SendHeartBeat()
        {
            string url = Properties.Settings.Default.Url;
            var i = Identity;
            PrintColors("ClientID: ", i.ClientId, Yellow, false);
            PrintColors("\tDeviceID: ", i.DeviceId, Yellow);

            using (var client = new KulvaSvc.KulvaClient(_bind, _endpoint))
            {
                if (!string.IsNullOrEmpty(_username))
                {
                    client.ClientCredentials.UserName.UserName = _username;
                    client.ClientCredentials.UserName.Password = _password;
                }
                var heartBeatResult = client.HeartBeat(i);
                PrintColors("Heartbeat: ", heartBeatResult.Message + "\r\n", (heartBeatResult.Code==0 ? Green : Red));
            }
        }

        internal void SendAccessControlEvent(string cardId, string reasonCode, DateTime? endTime)
        {
            string url = Properties.Settings.Default.Url;
            PrintColors("CardID: ", cardId, Yellow, false);
            PrintColors("\tReason: ", reasonCode, Yellow, false);
            var startTime = DateTime.Now;
            PrintColors(" Event time: ", DateString(startTime, "HH:mm:ss"), ConsoleColor.Magenta, false);
            PrintColors(" End time: ", DateString(endTime), ConsoleColor.Magenta);

            using (var client = new KulvaSvc.KulvaClient(_bind, _endpoint))
            {
                if (!string.IsNullOrEmpty(_username))
                {
                    client.ClientCredentials.UserName.UserName = _username;
                    client.ClientCredentials.UserName.Password = _password;
                }

                var stampResult = client.Stamp(CreateStampEntry(cardId, reasonCode, endTime));
                PrintColors("Access Control Stamp: ", stampResult.Message + "\r\n",
                    (stampResult.Code == 0 ? Green : Red));
            }
        }

        internal static KulvaSvc.Entry CreateStampEntry(string cardId, string reasonCode, DateTime? endTime)
        {
            return new KulvaSvc.Entry
            {
                CardId = cardId,
                EndTime = endTime,
                Identity = Identity,
                OccurredTime = DateTime.Now,
                ReasonCode = reasonCode
            };
        }

        internal void SendMultipleAccessControlEvents(KulvaSvc.Entry[] stamps)
        {
            string url = Properties.Settings.Default.Url;
            using (var client = new KulvaSvc.KulvaClient(_bind, _endpoint))
            {
                if (!string.IsNullOrEmpty(_username))
                {
                    client.ClientCredentials.UserName.UserName = _username;
                    client.ClientCredentials.UserName.Password = _password;
                }

                var results = client.Stamps(stamps);
                for(int i = 0; i<results.Length;i++)
                {
                    KulvaSvc.Entry s = stamps[i];
                    PrintColors("CardID: ", s.CardId, Yellow, false);
                    PrintColors("\tReason: ", s.ReasonCode, Yellow, false);
                    PrintColors(" Event time: ", DateString(s.OccurredTime, "HH:mm:ss"), ConsoleColor.Magenta, false);
                    PrintColors(" End time: ", DateString(s.EndTime), ConsoleColor.Magenta);
                    PrintColors("Access Control Stamp: ", results[i].Message, //+ "\r\n",
                    (results[i].Code == 0 ? Green : Red));
                }
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