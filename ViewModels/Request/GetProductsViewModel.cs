using ViewModels;

namespace LotusViewModels.Request
{
    public class GetProductsViewModel
    {
        public AuthorizeModel Authorization { get; set; }
        public int ValidityTime { get; set; }
        public int Currency { get; set; }
        public int paymentPeriod { get; set; }
        public int InfoContractProduct { get; set; }        
        public string SellerCode { get; set; }
    }
}
