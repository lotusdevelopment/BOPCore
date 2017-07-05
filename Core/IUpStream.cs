using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using ViewModels.Response;

namespace Core
{
    [ServiceContract]
    public interface IUpStream
    {
        [OperationContract(Name = "UserActivation")]
        [WebInvoke(Method = "POST", UriTemplate = "UserActivation/{phoneNumber}/{phoneOp}/{plan}/{sellerCode}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<SaleMade> UserActivation(string phoneNumber, string phoneOp, string plan, string sellerCode);

        [OperationContract(Name = "ChargeNotification")]
        [WebInvoke(Method = "POST", UriTemplate = "ChargeNotification/{phoneNumber}/{phoneOp}/{plan}/{sellerCode}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<SaleUpdate> ChargeNotification(string phoneNumber, string phoneOp, string plan, string sellerCode);

        [OperationContract(Name = "Suspension")]
        [WebInvoke(Method = "POST", UriTemplate = "Suspension/{phoneNumber}/{sellerCode}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<SaleUpdate> Suspension(string phoneNumber, string sellerCode);

        [OperationContract(Name = "Deactivation")]
        [WebInvoke(Method = "POST", UriTemplate = "Deactivation/{phoneNumber}/{sellerCode}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<SaleUpdate> Deactivation(string phoneNumber, string sellerCode, string plan);
    }
}
