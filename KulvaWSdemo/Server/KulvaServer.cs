using System;
using System.Linq;
using System.Collections.Generic;

namespace KulvaServer
{
    public class KulvaSRV : KulvaWCF.IKulva
    {
        private const ConsoleColor Purple = ConsoleColor.Magenta;
        private const ConsoleColor Yellow = ConsoleColor.Yellow;

        private KulvaWCF.GenericResult GetResult(int code, string message, string device, string card)
        {
            return new KulvaWCF.GenericResult { Code = code, Message = message, DeviceId = device, CardId = card };
        }

        public KulvaWCF.GenericResult Stamp(KulvaWCF.Entry entry)
        {
            Printer.Colored("\r\n## Received Access Control Stamp ## " + Printer.CurrentTimeString, Purple);

            Printer.Colored("ClientID: ", entry.Identity.ClientId, Yellow, false);
            Printer.Colored("\tDeviceID: ", entry.Identity.DeviceId, Yellow);
            if (string.IsNullOrEmpty(entry.Identity.ClientId))
                return GetResult(1, "ERROR : clientId is invalid!", entry.Identity.DeviceId, entry.CardId);
            if (string.IsNullOrEmpty(entry.Identity.DeviceId))
                return GetResult(2, "ERROR : deviceId is invalid!", entry.Identity.DeviceId, entry.CardId);
            if (string.IsNullOrEmpty(entry.CardId))
                return GetResult(3, "ERROR : cardId is invalid!", entry.Identity.DeviceId, entry.CardId);
            if (string.IsNullOrEmpty(entry.ReasonCode))
                return GetResult(4, "ERROR : reasonCode is invalid!", entry.Identity.DeviceId, entry.CardId);

            Printer.Colored("Card#: ", entry.CardId, ConsoleColor.Green, false);
            Printer.Colored("\tReason: ", entry.ReasonCode, ConsoleColor.Cyan, !entry.EndTime.HasValue);

            if (entry.EndTime.HasValue)
            {
                Printer.Colored("\tEndtime: ", Printer.DateTimeString(entry.EndTime.Value), ConsoleColor.Cyan);
            }

            return GetResult(0, "OK", entry.Identity.DeviceId, entry.CardId);
        }

        public List<KulvaWCF.GenericResult> Stamps(List<KulvaWCF.Entry> entries)
        {
            return entries.Select(Stamp).ToList();
        }

        public KulvaWCF.GenericResult HeartBeat(KulvaWCF.Identity identity)
        {
            Printer.Colored("## Received HeartBeat ## " + Printer.CurrentTimeString, Purple);
            Printer.Colored("ClientID: ", identity.ClientId, Yellow, false);
            Printer.Colored("\tDeviceID: ", identity.DeviceId, Yellow);
            if (string.IsNullOrEmpty(identity.ClientId))
                return new KulvaWCF.GenericResult
                {
                    Code = 1,
                    CardId = string.Empty,
                    Message = "ERROR : clientId is invalid!",
                    DeviceId = identity.DeviceId
                };
            if (string.IsNullOrEmpty(identity.DeviceId))
                return new KulvaWCF.GenericResult { Code = 2, Message = "ERROR : deviceId is invalid!", CardId = string.Empty };

            return new KulvaWCF.GenericResult { Code = 0, Message = "OK", DeviceId = identity.DeviceId, CardId = string.Empty };
        }

        public List<KulvaWCF.GenericResult> HeartBeats(List<KulvaWCF.Identity> identities)
        {
            return identities.Select(HeartBeat).ToList();
        }
    }
}