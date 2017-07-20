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
        [WebGet(UriTemplate = "GetTemAccredited/{latitude}/{longitude}/{specialties}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<TemAccredited>> GetTemAccredited(string latitude, string longitude, string specialties);

        [OperationContract(Name = "GetTemAccreditedDetails")]
        [WebGet(UriTemplate = "GetTemAccreditedDetails", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GeneralIdName>> GetTemAccreditedDetails();
    }
}
