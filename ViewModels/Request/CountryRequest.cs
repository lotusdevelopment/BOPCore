using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Request
{
    public class CountryRequest
    {
        public AuthorizeModel Credentials { get; set; }
        public string SellerId { get; set; }
    }
}
