using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    public class GMapsResponse
    {
        public List<GMaps> results { get; set; }
        public string status { get; set; }
    }

    public class GMaps
    {
        public object address_components { get; set; }
        public object formatted_address { get; set; }
        public Ggeometry geometry { get; set; }
        public bool partial_match { get; set; }
        public string place_id { get; set; }
        public object types { get; set; }
    }

    public class Ggeometry
    {
        public object bounds { get; set; }
        public Glocation location { get; set; }
        public string location_type { get; set; }
        public object viewport { get; set; }
    }

    public class Glocation
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
