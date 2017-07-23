using System.Collections.Generic;
using LotusViewModels.General;
using LotusViewModels.ILotusAssistance;

namespace LotusViewModels.Response
{
    public class LoginResponse
    {
        public User User { get; set; }
        public List<Roles> Roles { get; set; }
        public List<Apps> Apps { get; set; }
        public Empresas Company { get; set; }
        public bool Valid { get; set; }
    }
}
