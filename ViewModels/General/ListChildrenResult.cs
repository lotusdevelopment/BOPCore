using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class ListChildrenResult
    {
        public string user_id { get; set; }
        public string user_icard { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string user_icardtitular { get; set; }
        public int? user_uc { get; set; }
        public int status_id { get; set; }
        public string user_email { get; set; }
        public string user_sellerCode { get; set; }
        public DateTime? user_birthdate { get; set; }
        public string user_gender { get; set; }
        public string user_cellphone { get; set; }
        public string user_address { get; set; }
        public int country_id { get; set; }
        public string role_description { get; set; }
        public int id_grupo { get; set; }
        public string group_name { get; set; }
    }
}
