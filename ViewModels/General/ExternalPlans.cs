using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class ExternalPlans
    {
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string PlanType { get; set; }
        public string Currency { get; set; }
        public float Price { get; set; }
        public bool Monthly { get; set; }
        public int PeopleQ { get; set; }
        public int IdPlan { get; set; }
        public int CountryId { get; set; }
    }
}
