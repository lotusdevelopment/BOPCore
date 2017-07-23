using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class Sale
    {
        public int SerialPpac { get; set; }
        public int SerialPac { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime AffiliationDate { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string SellerId { get; set; }
        public string SellerCode { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public decimal PlanValue { get; set; }
        public string Currency { get; set; }
        public int LifeQuantity { get; set; }
        public string LifeDescription { get; set; }
        public string Session { get; set; }
        public string SaleId { get; set; }        
    }
}
