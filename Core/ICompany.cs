using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using LotusViewModels.General;
using ViewModels.ILotusAssistance;
using LotusViewModels.Request;
using LotusViewModels.Response;
using ViewModels.General;

namespace Core
{
    [ServiceContract]
    public interface ICompany
    {
        [OperationContract(Name = "CheckExistingSale")]
        [WebGet(UriTemplate = "CheckExistingSale/{phone}/{id}/{session}", RequestFormat = WebMessageFormat.Json,
    BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<SaleValidation> CheckExistingSale(string phone, string id, string session);

        [OperationContract(Name = "UpdateAndSendData")]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateAndSendData",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void UpdateAndSendData(SaleVal model);


        [OperationContract(Name = "SendPaymentEmail")]
        [WebInvoke(Method = "POST", UriTemplate = "SendPaymentEmail/{name}/{id}/{phone}/{email}/{address}/{birthday}/{plan}" +
                                                  "/{country}/{value}/{buyerEmail}/{buyerName}/{bid}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void SendPaymentEmail(string name, string id, string phone, string email,
            string address, string birthday, string plan, string country, string value, string buyerEmail,
            string buyerName, string bid);

        [OperationContract(Name = "GetRoles")]
        [WebGet(UriTemplate = "GetRoles", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Roles>> GetRoles();

        [OperationContract(Name = "GetLanguages")]
        [WebGet(UriTemplate = "GetLanguages", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Languages>> GetLanguages();

        [OperationContract(Name = "InsertOrUpdateGroup")]
        [WebInvoke(Method = "POST", UriTemplate = "InsertOrUpdateGroup/{entityId}/{descGroup}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string InsertOrUpdateGroup(string entityId, string descGroup);

        [OperationContract(Name = "CreateEntity")]
        [WebInvoke(Method = "POST", UriTemplate = "CreateEntity",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> CreateEntity(CreateEntity createEntity);

        [OperationContract(Name = "GetGroupsEntity")]
        [WebGet(UriTemplate = "GetGroupsEntity/{entityId}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Groups>> GetGroupsEntity(string entityId);

        [OperationContract(Name = "GetCountriesAlone")]
        [WebGet(UriTemplate = "GetCountriesAlone", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Countries>> GetCountriesAlone();

        [OperationContract(Name = "GetEntityTypes")]
        [WebGet(UriTemplate = "GetEntityTypes", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<EntityType>> GetEntityTypes();

        [OperationContract(Name = "GetApps")]
        [WebGet(UriTemplate = "GetApps", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GetApps>> GetApps();

        [OperationContract(Name = "GetTreatments")]
        [WebGet(UriTemplate = "GetTreatments", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Treatments>> GetTreatments();

        [OperationContract(Name = "GetProducts")]
        [WebGet(UriTemplate = "GetProducts", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Products>> GetProducts();

        [OperationContract(Name = "GetEntities")]
        [WebGet(UriTemplate = "GetEntities", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Entity>> GetEntities();

        [OperationContract(Name = "GetContracts")]
        [WebGet(UriTemplate = "GetContracts", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<Contract>> GetContracts();

        [OperationContract(Name = "GetValidityTime")]
        [WebGet(UriTemplate = "GetValidityTime", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GetApps>> GetValidityTime();

        [OperationContract(Name = "GetPaymentPeriod")]
        [WebGet(UriTemplate = "GetPaymentPeriod", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GetApps>> GetPaymentPeriod();

        [OperationContract(Name = "GetInactivationPeriod")]
        [WebGet(UriTemplate = "GetInactivationPeriod", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GetApps>> GetInactivationPeriod();

        [OperationContract(Name = "CreateProduct")]
        [WebInvoke(Method = "POST", UriTemplate = "CreateProduct/{NomProduct}/{CodesTreat}/{ValueCoverage}/{Quantity}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> CreateProduct(string NomProduct, string CodesTreat, string ValueCoverage, string Quantity);

        [OperationContract(Name = "CreateMenuRole")]
        [WebInvoke(Method = "POST", UriTemplate = "CreateMenuRole",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> CreateMenuRole(MenusRole CreateMenuRole);

        [OperationContract(Name = "GetAllContracts")]
        [WebGet(UriTemplate = "GetAllContracts/{gId}", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        List<ContractsSpecificationsRs> GetAllContracts(string gId);

        [OperationContract(Name = "UpdateCommission")]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateCommission/{listIds}/{ListParti}/{Quantity}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Task<string> UpdateCommission(string listIds, string ListParti, string Quantity);

        [OperationContract(Name = "Translate")]
        [WebGet(UriTemplate = "Translate/{phrase}/{langF}/{lanfT}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<string> Translate(string phrase, string langF, string lanfT);

        [OperationContract(Name = "GetUsersPerParent")]
        [WebGet(UriTemplate = "GetUsersPerParent/{uId}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<List<ListChildrenResult>> GetUsersPerParent(string uId);

        [OperationContract(Name = "SalesReport")]
        [WebGet(UriTemplate = "SalesReport/{id}/{type}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<DashboardReportsStatistics> SalesReport(string id, string type);

        [OperationContract(Name = "GroupMetricsReport")]
        [WebGet(UriTemplate = "GroupMetricsReport/{companyId}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GroupMetricsReport>> GroupMetricsReport(string companyId);

        [OperationContract(Name = "ProductsMetricsReport")]
        [WebGet(UriTemplate = "ProductsMetricsReport/{companyId}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<List<GroupMetricsReport>> ProductsMetricsReport(string companyId);

        [OperationContract(Name = "SalesReportSeller")]
        [WebGet(UriTemplate = "SalesReportSeller/{id}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<DashboardReportsStatistics> SalesReportSeller(string id);

        [OperationContract(Name = "GetFullDictionary")]
        [WebGet(UriTemplate = "GetFullDictionary/{lang}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<Dictionary<string, string>> GetFullDictionary(string lang);
    }
}