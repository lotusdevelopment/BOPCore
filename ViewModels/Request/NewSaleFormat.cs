using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Request
{
    public class NewSaleFormat
    {
        public SaleRequestBeneficiaries SaleInfo { get; set; }
        public bool LotusPayment { get; set; }
        public string Process { get; set; }
    }
}
