using ViewModels.General;

namespace ViewModels.Response
{
    public class SaleMade
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string SaleDate { get; set; }
        public string ValidUntil { get; set; }
        public GenericError Error { get; set; }
    }
}
