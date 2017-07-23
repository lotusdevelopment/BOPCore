using ViewModels;

namespace LotusViewModels.Request
{
    public class GeneralSalesRequests
    {
        public AuthorizeModel Authorization { get; set; }
        public string CellPhone { get; set; }
        public string SaleValue { get; set; }
        public int PlanId { get; set; }
    }
}
