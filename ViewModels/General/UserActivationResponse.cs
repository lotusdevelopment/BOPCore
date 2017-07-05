using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    public class UserActivationResponse
    {
        public string UserCellPhone { get; set; }
        public string UserPassword { get; set; }
        public string SaleDate { get; set; }
        public string ValidUntil { get; set; }
    }
}
