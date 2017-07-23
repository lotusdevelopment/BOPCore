using ViewModels;
using System.Collections.Generic;
using ViewModels.General;

namespace LotusViewModels.Response
{
    public class ProductsRsViewModel
    {
        public GenericError Error { get; set; }
        public List<RsProductsInner> Products { get; set; }
    }

    public class RsProductsInner
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Currency { get; set; }
        public string ValidityTime { get; set; }
        public string PaymentPeriod { get; set; }
        public string InfoContractProduct { get; set; }
        public double ProductPrice { get; set; }
    }
}
