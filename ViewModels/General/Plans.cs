using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    [DataContract]
    public class Plans
    {
        [DataMember]
        public int PlanId { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string PlanName { get; set; }
        [DataMember]
        public string Currency { get; set; }
    }
}
