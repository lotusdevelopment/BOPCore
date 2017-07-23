using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotusViewModels.General;

namespace LotusViewModels.Request
{
    public class CreateEntity
    {
        public int Flag { get; set; }
        public string Name { get; set; }
        public string Nit { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public int EntityType { get; set; }
        public int Neig { get; set; }
        public int Country { get; set; }
        public int Status { get; set; }
        public int FatherId { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        public string Url { get; set; }
        public string Favicon { get; set; }
        public User User { get; set; }
    }
}
