using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Response
{
    [DataContract]
    public class UserResponse
    {
        [DataMember]
        public bool UserExists { get; set; }
        [DataMember]
        public GenericError Error { get; set; }
    }
}
