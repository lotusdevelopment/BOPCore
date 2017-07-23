using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    [DataContract]
    public class ClientCardModel
    {
        [DataMember]
        public string CardAddressCountry { get; set; }
        [DataMember]
        public string CardAddressLine1 { get; set; }
        [DataMember]
        public string CardAddressLine2 { get; set; }
        [DataMember]
        public string CardAddressCity { get; set; }
        [DataMember]
        public string CardAddressZip { get; set; }
        [DataMember]
        public string CardCvc { get; set; }
        [DataMember]
        public string CardExpirationMonth { get; set; }
        [DataMember]
        public string CardExpirationYear { get; set; }
        [DataMember]
        public string CardName { get; set; }
        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string Currency { get; set; }
    }
}
