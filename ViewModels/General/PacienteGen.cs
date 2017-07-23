using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class PacienteGen
    {
        public int SerialPac { get; set; }
        public string NombrePac { get; set; }
        public string ApellidoPac { get; set; }
        public DateTime? FecNacPac { get; set; }
        public string SexoPac { get; set; }
        public string DirCasaPac { get; set; }
        public string TelefCasaPac { get; set; }
        public string TelCelPac { get; set; }
        public string Email { get; set; }
        public bool? Vigente { get; set; }
        public long? CnsNuevo { get; set; }
        public string CedulaPac { get; set; }
        public string CedulaBsq { get; set; }
    }
}
