using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Icon { get; set; }
        public bool IsParent { get; set; }
        public int ParentId { get; set; }
    }
}
