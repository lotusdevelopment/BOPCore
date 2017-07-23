using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusViewModels.General
{
    public class Apps
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public List<Menu> Menu { get; set; }
    }
}
