using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using ViewModels.Response;

namespace Core
{
    [ServiceContract]
    public interface IExternal
    {
        [OperationContract(Name = "GetTemSpecialties")]
        [WebGet(UriTemplate = "GetTemSpecialties", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GeneralIdName>> GetTemSpecialties();

        [OperationContract(Name = "GetTemStates")]
        [WebGet(UriTemplate = "GetTemStates", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<TemState>> GetTemStates();

        [OperationContract(Name = "GetTemCities")]
        [WebGet(UriTemplate = "GetTemCities/{state}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<TemCity>> GetTemCities(string state);

        [OperationContract(Name = "GetTemNeighborhoods")]
        [WebGet(UriTemplate = "GetTemNeighborhoods/{state}/{city}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<TemNeighborhood>> GetTemNeighborhoods(string state, string city);

        [OperationContract(Name = "GetTemAccredited")]
        [WebGet(UriTemplate = "GetTemAccredited/{latitude}/{longitude}/{range}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<TemAccredited>> GetTemAccredited(string latitude, string longitude, string range);

        [OperationContract(Name = "GetTemAccreditedDetails")]
        [WebGet(UriTemplate = "GetTemAccreditedDetails", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GeneralIdName>> GetTemAccreditedDetails();

        [OperationContract(Name = "GetFuncionalStates")]
        [WebGet(UriTemplate = "GetFuncionalStates", RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<FuncionalState>> GetFuncionalStates();

        [OperationContract(Name = "GetFuncionalCities")]
        [WebGet(UriTemplate = "GetFuncionalCities/{state}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<FuncionalCity>> GetFuncionalCities(string state);

        [OperationContract(Name = "GetFuncionalNeighborhoods")]
        [WebGet(UriTemplate = "GetFuncionalNeighborhoods/{cityCode}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<FuncionalNeighborhood>> GetFuncionalNeighborhoods(string cityCode);

        [OperationContract(Name = "GetFuncionalDrugStores")]
        [WebGet(UriTemplate = "GetFuncionalDrugStores/{latitude}/{longitude}/{range}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<FuncionalAccredited>> GetFuncionalDrugStores(string latitude, string longitude, string range);
    }
}
