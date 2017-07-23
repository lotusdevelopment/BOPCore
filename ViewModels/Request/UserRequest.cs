using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Request
{
    public class UserRequest
    {
        public AuthorizeModel Credentials { get; set; }
        public string Email { get; set; }
    }
}
