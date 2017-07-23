using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViewModels.General;

namespace ViewModels
{
    [DataContract]
    public class SaleResponse
    {
        [DataMember]
        public bool ProcessSucess { get; set; }
        [DataMember]
        public string Annotations { get; set; }
        [DataMember]
        public GenericError Error { get; set; }
    }
}
