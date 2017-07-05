using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.General;

namespace ViewModels.Response
{
    public class SaleUpdate
    {
        public string SaleDate { get; set; }
        public string ValidUntil { get; set; }
        public bool UpdateStatus { get; set; }
        public GenericError Error { get; set; }
    }
}
