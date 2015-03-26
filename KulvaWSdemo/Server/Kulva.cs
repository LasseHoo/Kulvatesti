using System;

namespace KulvaServer
{
    public class Kulva : IKulva
    {
        private const ConsoleColor Purple = ConsoleColor.Magenta;
        private const ConsoleColor Yellow = ConsoleColor.Yellow;

        public string Stamp(string clientId, string deviceId, string cardId, string reasonCode, 
            DateTime occurredTime, DateTime? endTime)
        {
            Printer.Colored("\r\n## Received Access Control Stamp ## " + Printer.CurrentTimeString, Purple);

            Printer.Colored("ClientID: ", clientId, Yellow, false);
            Printer.Colored("\tDeviceID: ", deviceId, Yellow);
            if (string.IsNullOrEmpty(clientId)) return "ERROR : clientId is invalid!";
            if (string.IsNullOrEmpty(deviceId)) return "ERROR : deviceId is invalid!";

            Printer.Colored("Card#: ", cardId, ConsoleColor.Green, false);
            Printer.Colored("\tReason: ", reasonCode, ConsoleColor.Cyan, !endTime.HasValue);
            if (string.IsNullOrEmpty(cardId)) return "ERROR : cardId is invalid!";
            if (string.IsNullOrEmpty(reasonCode)) return "ERROR : reasonCode is invalid!";

            if (endTime.HasValue)
            {
                Printer.Colored("\tEndtime: ", Printer.DateTimeString(endTime.Value), ConsoleColor.Cyan); 
            }

            return "OK";
        }

        public bool HeartBeat(string clientId, string deviceId)
        {
            Printer.Colored("## Received HeartBeat ## " + Printer.CurrentTimeString, Purple);
            Printer.Colored("ClientID: ", clientId, Yellow, false);
            Printer.Colored("\tDeviceID: ", deviceId, Yellow);
            if (string.IsNullOrEmpty(clientId)) return false;
            if (string.IsNullOrEmpty(deviceId)) return false;

            return true;
        }
    }
}