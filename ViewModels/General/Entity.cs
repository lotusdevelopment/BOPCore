using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class Entity
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityNit { get; set; }
        public double EntityLatitud { get; set; }
        public double EntityLongitud { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityType { get; set; }
        public int NeigId { get; set; }
        public string Neig { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public bool Status { get; set; }
        public int? Entityfather { get; set; }
        public string EntityDescription { get; set; }
        public string EntityLogo { get; set; }
        public string EntityBackground { get; set; }
        public string EntityMaincolor { get; set; }
        public string EntitySeccolor { get; set; }
        public string EntityUrl { get; set; }
        public string EntityFavicon { get; set; }
        public string EntityEmailHolder { get; set; }
        public string EntityEmailCopies { get; set; }
        public string EntitySalesEmail { get; set; }
    }
}
