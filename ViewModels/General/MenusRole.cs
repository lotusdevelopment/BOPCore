using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class MenusRole
    {
        public string NameMenu { get; set; }
        public int AppId { get; set; }
        public int RoleId { get; set; }
        public int IsParent { get; set; }
        public int ParentId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Icon { get; set; }
    }
}
