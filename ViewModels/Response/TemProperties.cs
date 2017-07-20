using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Response
{
    class TemProperties
    {
    }

    public class GeneralIdName
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TemState
    {
        public string state { get; set; }
    }

    public class TemCity
    {
        public string city { get; set; }
    }

    public class TemNeighborhood
    {
        public string neighborhood { get; set; }
    }

    public class TemAccreditedResult
    {
        public List<TemAccredited> data { get; set; }
    }

    public class TemAccredited
    {
        public int? DT_RowId { get; set; }
        public string specialty { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public string state { get; set; }
        public string zone { get; set; }
    }

    public class CustomState : TemState
    {
        public string StateName { get; set; }
        public List<CustomCity> Cities { get; set; }
    }

    public class CustomCity : TemCity
    {
        public List<CustomNeighborhood> Neighborhoods { get; set; }
    }

    public class CustomNeighborhood : TemNeighborhood
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class BrazilianPlaces
    {
        public List<CustomState> GetBrazilianPlaces()
        {
            try
            {
                var str = (@"AC;Acre
                            AL;Alagoas
                            AP;Amapá
                            AM;Amazonas
                            BA;Bahia
                            CE;Ceará
                            DF;Distrito Federal
                            ES;Espírito Santo
                            GO;Goiás
                            MA;Maranhão
                            MT;MatoGrosso
                            MS;MatoGrosso do Sul
                            MG;Minas Gerais
                            PA;Pará
                            PB;Paraíba
                            PR;Paraná
                            PE;Pernambuco
                            PI;Piauí
                            RJ;Rio de Janeiro
                            RN;Rio Grande do Norte
                            RS;Rio Grande do Sul
                            RO;Rondônia
                            RR;Roraima
                            SC;Santa Catarina
                            SP;São Paulo
                            SE;Sergipe
                            TO;Tocantins")
                            .Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)
                            .ToList();
                var newLst = new List<string>();
                foreach (var item in str) newLst.Add(item.Trim());
                var rtn = new List<CustomState>();
                foreach (var item in newLst)
                {
                    var splt = item.Split(';');
                    rtn.Add(new CustomState
                    {
                        state = splt[0],
                        StateName = splt[1]
                    });
                }
                return rtn;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
