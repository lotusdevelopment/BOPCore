using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [ServiceContract]
    public interface IBopInner
    {
        [OperationContract(Name = "GetString")]
        [WebGet(UriTemplate = "GetString/{word}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        string GetString(string word);
    }
}
