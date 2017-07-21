using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Response
{
    public class FuncionalProperties
    {
    }

    public class FuncionalState : CustomState
    {
        public List<FuncionalCity> FCities { get; set; }
    }

    public class FuncionalCity : TemCity
    {
        public int localityCode { get; set; }
        public List<FuncionalNeighborhood> Neighborhoods { get; set; }
    }

    public class FuncionalNeighborhood : CustomNeighborhood
    {
        public int NeighborhoodCode { get; set; }
    }

    public class FuncionalAccredited
    {
        public string Neighborhood { get; set; }
        public string Cep { get; set; }
        public int AccreditedCode { get; set; }
        public int RegionCode { get; set; }
        public string Email { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string AccreditedName { get; set; }
        public string City { get; set; }
        public string Attendant { get; set; }
        public string Phone { get; set; }
    }
}
