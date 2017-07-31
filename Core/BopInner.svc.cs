using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BopInner : IBopInner
    {
        public string GetString(string word)
        {
            return word;
        }
    }
}
