using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    public class PlanInfo
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CellPhone { get; set; }
        public string OrderDate { get; set; }
        public string SerialId { get; set; }
        public PlanDetails PlanDetails { get; set; }
    }
}
