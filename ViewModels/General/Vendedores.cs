using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class Vendedores
    {
        public string GUID { get; set; }
        public string Company { get; set; }
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Clave { get; set; }
        public string Correo { get; set; }
        public decimal? Movil { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string CodigoVendedor { get; set; }
        public string LlaveVendedor { get; set; }
        public string Ciudad { get; set; }
        public string Moneda { get; set; }
        public string Pais { get; set; }
    }
}
