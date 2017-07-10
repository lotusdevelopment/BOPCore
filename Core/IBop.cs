using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using ViewModels.General;

namespace Core
{
    [ServiceContract]
    public interface IBop
    {
        /*Bop Methods*/

        [OperationContract(Name = "LogIn")]
        [WebInvoke(Method = "POST", UriTemplate = "LogIn/{userName}/{password}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<bool> LogIn(string userName, string password);

        [OperationContract(Name = "GetUser")]
        [WebGet(UriTemplate = "GetUser/{userName}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<GeneralUser> GetUser(string userName);

        [OperationContract(Name = "ForgotPassword")]
        [WebInvoke(Method = "POST", UriTemplate = "ForgotPassword/{userName}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void ForgotPassword(string userName);

        [OperationContract(Name = "UpdateUser")]
        [WebInvoke(Method = "PUT", UriTemplate = "UpdateUser",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<bool> UpdateUser(GeneralUser user);

        /*Others Methods*/

        [OperationContract(Name = "GetDrugStores")]
        [WebGet(UriTemplate = "GetDrugStores/{latitude}/{longitude}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> GetDrugStores(string latitude, string longitude);
    }
}
