using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AuthorizeModel
    {
        public string User { get; set; }
        public string Password { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
