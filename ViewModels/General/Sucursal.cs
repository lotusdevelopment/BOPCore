using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class Sucursal
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string ZipCode { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string GeoAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Specialities { get; set; }
    }
}
