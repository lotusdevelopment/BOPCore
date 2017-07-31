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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBopInner" in both code and config file together.
    [ServiceContract]
    public interface IBopInner
    {
        [OperationContract(Name = "GetString")]
        [WebInvoke(Method = "POST", UriTemplate = "GetString/{word}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> GetString(string word);
    }
}
