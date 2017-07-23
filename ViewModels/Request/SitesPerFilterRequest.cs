using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Request
{
    public class SitesPerFilterRequest
    {
        public AuthorizeModel Credentials { get; set; }
        public string Parameter { get; set; }
    }
}
