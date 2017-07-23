using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Request
{
    public class PlanByCountryRequest
    {
        public AuthorizeModel Credentials { get; set; }
        public string SellerCode { get; set; }
        public string CountryId { get; set; }
    }
}
