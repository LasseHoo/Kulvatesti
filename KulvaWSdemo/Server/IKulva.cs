using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace KulvaWCF
{
    [ServiceContract(Namespace = "http://www.elisa.fi/kulva")]
    public interface IKulva
    {
        [OperationContract]
        GenericResult Stamp(Entry entry);

        [OperationContract]
        List<GenericResult> Stamps(List<Entry> entries);

        [OperationContract]
        GenericResult HeartBeat(Identity identity);

        [OperationContract]
        List<GenericResult> HeartBeats(List<Identity> identities);
    }

    [DataContract]
    public class Identity
    {
        [DataMember(IsRequired = true, Order = 0)]
        public string ClientId;
        [DataMember(IsRequired = true, Order = 1)]
        public string DeviceId;
    }

    [DataContract]
    public class Entry
    {
        [DataMember(IsRequired = true, Order = 0)]
        public Identity Identity;
        [DataMember(IsRequired = true, Order = 1)]
        public string CardId;
        [DataMember(IsRequired = true, Order = 2)]
        public string ReasonCode;
        [DataMember(IsRequired = true, Order = 3)]
        public DateTime OccurredTime;
        [DataMember(IsRequired = false, Order = 4)]
        public DateTime? EndTime;
    }

    [DataContract]
    public class GenericResult
    {
        [DataMember(Order = 0)]
        public string DeviceId;
        [DataMember(Order = 1)]
        public string CardId;
        [DataMember(Order = 2)]
        public int Code;
        [DataMember(Order = 3)]
        public string Message;
    }
}