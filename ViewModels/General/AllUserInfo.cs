using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ILotusAssistance;

namespace LotusViewModels.General
{
    public class AllUserInfo
    {
        public User User { get; set; }
        public Empresas Company { get; set; }
        public List<Roles> Roles { get; set; }
    }
}
