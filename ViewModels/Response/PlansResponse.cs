using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ViewModels.General;

namespace ViewModels.Response
{
    [DataContract]
    public class PlansResponse
    {
        [DataMember]
        public List<Plans> Plans { get; set; }
        [DataMember]
        public GenericError Error { get; set; }
    }
}
