using System.Net;
using System.ServiceModel.Channels;

namespace Kulva
{
    public class CustomBindingCreator
    {
        public static CustomBinding Create(string url)
        {
            return new CustomBinding(new BindingElement[]
            {
                new TextMessageEncodingBindingElement
                {
                    MessageVersion = MessageVersion.Soap11,
                    ReaderQuotas = {MaxNameTableCharCount = int.MaxValue}
                },
                url.StartsWith("https")
                    ? new HttpsTransportBindingElement
                    {
                        MaxReceivedMessageSize = int.MaxValue,
                        AuthenticationScheme = AuthenticationSchemes.Negotiate
                    }
                    : new HttpTransportBindingElement
                    {
                        MaxReceivedMessageSize = int.MaxValue,
                        AuthenticationScheme = AuthenticationSchemes.Negotiate
                    }
            })
            {
                SendTimeout = System.TimeSpan.FromMinutes(5),
                ReceiveTimeout = System.TimeSpan.FromMinutes(2),
                OpenTimeout = System.TimeSpan.FromMinutes(2),
                CloseTimeout = System.TimeSpan.FromMinutes(2)
            };
        }
    }
}