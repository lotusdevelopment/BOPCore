using System;
using ViewModels;

namespace LotusViewModels.Response
{
    public class SaleResponseViewModel
    {
        public string SaleId { get; set; }
        public string Annotations { get; set; }
        public string UsageBeginning { get; set; }
        public string UsageEnding { get; set; }
        public GenericError Error { get; set; }
    }
}
