using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace LotusViewModels.Request
{
    class SalesOptionsRequestViewModel
    {
        public AuthorizeModel Credentials { get; set; }
        public string SellerCode { get; set; }
    }
}
