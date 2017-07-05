using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Response;

namespace Core
{
    [ServiceContract]
    public interface IBop
    {
        [OperationContract(Name = "LogIn")]
        [WebInvoke(Method = "POST", UriTemplate = "LogIn/{userName}/{password}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<bool> LogIn(string userName, string password);
    }
}
