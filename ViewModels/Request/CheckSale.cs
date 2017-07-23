using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Request
{
    public class CheckSale
    {
        public AuthorizeModel Authorization { get; set; }
        public string Email { get; set; }
        public string SellerCode { get; set; }
        public string CellPhone { get; set; }
        public string CompanySaleId { get; set; }
    }
}
