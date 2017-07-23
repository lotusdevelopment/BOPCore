using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    [DataContract]
    public class ConfigPlanes
    {
        [DataMember]
        public int IdConfigPlan { get; set; }
        [DataMember]
        public int SerialPlan { get; set; }
        [DataMember]
        public string Titulo1 { get; set; }
        [DataMember]
        public string Titulo2 { get; set; }
        [DataMember]
        public decimal Valor { get; set; }
        [DataMember]
        public int Duracion { get; set; }
        [DataMember]
        public decimal Descuento { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public DateTime FechaIncio { get; set; }
        [DataMember]
        public DateTime FechaFinal { get; set; }
        [DataMember]
        public Guid? Guid { get; set; }
        [DataMember]
        public string Html { get; set; }
        [DataMember]
        public int? SerialPoliza { get; set; }
        [DataMember]
        public int? StripeTableId { get; set; }
        [DataMember]
        public string StripeDescription { get; set; }
        [DataMember]
        public int StripeInterval { get; set; }
        [DataMember]
        public int StripeTrial { get; set; }
        [DataMember]
        public string Moneda { get; set; }
        [DataMember]
        public int? CantidadDias { get; set; }
        [DataMember]
        public string UrlCarnet { get; set; }
        [DataMember]
        public string UrlHtml { get; set; }
        [DataMember]
        public string UrlCondiciones { get; set; }
    }
}
