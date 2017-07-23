using LotusViewModels.Request;
using LotusViewModels.Response;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using ViewModels;
using ViewModels.Request;

namespace Core
{
    [ServiceContract]
    public interface ILotusSales
    {
        [OperationContract(Name = "GetSaleOptions")]
        [WebInvoke(Method = "POST", UriTemplate = "GetSaleOptions",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<SalesOptionsViewModel> GetSaleOptions(AuthorizeModel request);

        [OperationContract(Name = "GetProducts")]
        [WebInvoke(Method = "POST", UriTemplate = "GetProducts",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<ProductsRsViewModel> GetProducts(GetProductsViewModel request);

        [OperationContract(Name = "SaleRequest")]
        [WebInvoke(Method = "POST", UriTemplate = "SaleRequest",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<SaleResponse> SaleRequest(NewSaleFormat request);
    }
}
