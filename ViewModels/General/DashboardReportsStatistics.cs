using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class DashboardReportsStatistics
    {
        public int SalesQuantityDay { get; set; }
        public double SaleValueDay { get; set; }
        public int SalesQuantityWeek { get; set; }
        public double SaleValueWeek { get; set; }
        public int SalesQuantityMonth { get; set; }
        public double SaleValueMonth { get; set; }
        public List<DashboardReportsSellerStatistics> DaySales { get; set; }
    }
    public class DashboardReportsSellerStatistics
    {
        public string idSale { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string product { get; set; }
        public double value { get; set; }
        public double commssion { get; set; }
    }
}
