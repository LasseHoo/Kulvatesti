using System;
using System.ServiceModel;

namespace KulvaServer
{
    [ServiceContract(Namespace = "http://www.elisa.fi/kulva")]
    public interface IKulva
    {
        [OperationContract]
        string Stamp(string clientId, string deviceId, string cardId, string reasonCode, DateTime occurredTime, DateTime? endTime);

        [OperationContract]
        bool HeartBeat(string clientId, string deviceId);
    }
}