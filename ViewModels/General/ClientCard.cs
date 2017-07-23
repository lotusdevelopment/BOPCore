using System.Runtime.Serialization;

namespace ViewModels.General
{
    [DataContract]
    public class ClientCard
    {
        [DataMember]
        public string CardAddressLine1 { get; set; }
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
    }
}
