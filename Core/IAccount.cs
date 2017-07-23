using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ILotusAssistance;
using LotusViewModels.Request;
using LotusViewModels.Response;
using ViewModels;
using ViewModels.Request;
using LotusViewModels.General;

namespace Core
{
    [ServiceContract]
    public interface IAccount
    {
        [OperationContract(Name = "LoginSystem")]
        [WebInvoke(Method = "POST", UriTemplate = "LoginSystem",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<LoginResponse> LoginSystem(LoginRequest login);

        [OperationContract(Name = "CreateUser")]
        [WebInvoke(Method = "POST", UriTemplate = "CreateUser",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> CreateUser(User createUser);

        [OperationContract(Name = "ChangePassword")]
        [WebInvoke(Method = "POST", UriTemplate = "ChangePassword",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<bool> ChangePassword(User user);

        [OperationContract(Name = "ResetAccount")]
        [WebInvoke(Method = "POST", UriTemplate = "ResetAccount",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> ResetAccount(User user);

        [OperationContract(Name = "UpdateLastActivity")]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateLastActivity",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> UpdateLastActivity(User user);

        [OperationContract(Name = "UpdateUser")]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateUser",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<bool> UpdateUser(User user);

        [OperationContract(Name = "UpdateProfilePicture")]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateProfilePicture/{url}/{id}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void UpdateProfilePicture(string url, string id);
    }
}
