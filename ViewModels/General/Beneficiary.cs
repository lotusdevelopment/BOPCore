using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.General
{
    public class Beneficiary
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Id { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string Parent { get; set; }
        public string Mother { get; set; }
    }
}
