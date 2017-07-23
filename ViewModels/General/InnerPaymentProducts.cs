using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class InnerPaymentProducts
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Currency { get; set; }
        public int Ptime { get; set; }
        public string Pdescription { get; set; }
        public string Vtime { get; set; }
        public int Vdays { get; set; }
        public double Price { get; set; }
    }
}
