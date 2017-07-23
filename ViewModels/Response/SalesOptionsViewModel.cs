using LotusViewModels.General;
using System.Collections.Generic;
using ViewModels;
using ViewModels.General;

namespace LotusViewModels.Response
{
    public class SalesOptionsViewModel
    {
        public List<ValidityTimeViewModel> ValidityTime { get; set; }
        public List<PaymentPeriodViewModel> PaymentPeriod { get; set; }
        public List<InfoContractProdViewModel> InfoContractProd { get; set; }
        public List<CurrencyProdViewModel> CurrencyProd { get; set; }
        public List<CountriesViewModel> Countries { get; set; }
        public GenericError Error { get; set; }
    }
}
