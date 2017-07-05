using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

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
