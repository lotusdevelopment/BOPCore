using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    public class GenericBusAuth
    {
        public string Entity { get; set; }
        public string SystemUser { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
