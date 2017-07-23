using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.App_Start;
using Apps.Resources;
using LotusViewModels.General;
using ViewModels.ILotusAssistance;
using LotusViewModels.Request;
using Newtonsoft.Json;
using Menu = System.Web.UI.WebControls.Menu;

namespace Apps.Logic
{
    public class Generics
    {
        private readonly CoreConnect _core = new CoreConnect();

        public List<Sucursal> GetSpecificMapPlaces(string filter, int type)
        {
            var ip = CoreConnections.Company + "GetSpecificMapPlaces";
            var lst = new List<string>
            {
                filter,type.ToString()
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<List<Sucursal>>(result);
        }

        public List<Sucursal> GetAllSites()
        {
            var ip = CoreConnections.Company + "GetAllSites";
            var result = _core.GetResponseGet(ip);
            return JsonConvert.DeserializeObject<List<Sucursal>>(result);
        }

        public void InsertTracing(string inicial, string final, string respo)
        {
            var ip = CoreConnections.Company + "InsertTracing";
            var lst = new List<string>
            {
                inicial,final,respo
            };
            _core.GetResponse(ip, lst);
        }

        public Carnets GetCarnet(string id)
        {
            var ip = CoreConnections.Company + "GetCarnet";
            var result = _core.GetResponseGet(ip, id);
            return JsonConvert.DeserializeObject<Carnets>(result);
        }

        public Empresas CompanyProperties(string id)
        {
            var ip = CoreConnections.Company + "CompanyProperties";
            var result = _core.GetResponseGet(ip, id);
            return JsonConvert.DeserializeObject<Empresas>(result);
        }

        public string GetCompanyId(string id)
        {
            var ip = CoreConnections.Company + "GetCompanyId";
            var result = _core.GetResponseGet(ip, id);
            return JsonConvert.DeserializeObject<string>(result);
        }

        public List<IGrouping<string, ExternalPlans>> GetExternalPlans(string country, string city, bool familiar,
            string currency, string s)
        {
            var ip = CoreConnections.Company + "GetExternalPlans";
            var lst = new List<string>
            {
                country,city,familiar.ToString(),currency,s
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<List<IGrouping<string, ExternalPlans>>>(result);
        }

        public IEnumerable<CountrymapInfo> GetCountryInfo(string name)
        {
            var ip = CoreConnections.Company + "GetCountryInfo";
            var result = _core.GetResponseGet(ip, name);
            return JsonConvert.DeserializeObject<IEnumerable<CountrymapInfo>>(result);
        }

        public SaleValidation CheckExistingSale(string phone, string id, string session)
        {
            var ip = CoreConnections.Company + "CheckExistingSale";
            var lst = new List<string>
            {
                phone,id,session
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<SaleValidation>(result);
        }

        public void UpdateAndSendData(SaleVal model)
        {
            var ip = CoreConnections.Company + "UpdateAndSendData";
            _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
        }

        public void InsertToMap()
        {
            var ip = CoreConnections.Company + "InsertToMap";
            _core.GetResponseGet(ip);
        }

        public void CreateEntity(CreateEntity createEntity)
        {
            var ip = CoreConnections.Company + "CreateEntity";
            _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(createEntity)));
        }

        public void CreateProduct(string name, string treat, string per, int q)
        {
            var ip = CoreConnections.Company + "CreateProduct";
            var lst = new List<string>
            {
                name,treat,per,q.ToString()
            };
            _core.GetResponse(ip, lst);
        }

        public void UpdateComissions(string commit, string percs, string q)
        {
            var ip = CoreConnections.Company + "UpdateCommission";
            var lst = new List<string> { commit, percs, q };
            _core.GetResponse(ip, lst);
        }

        public void CreateUser(User usr)
        {
            var ip = CoreConnections.Account + "CreateUser";
            _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(usr)));
        }

        public bool ChangePassword(string password)
        {
            var ip = CoreConnections.Account + "ChangePassword";
            var model = new User
            {
                Password = password,
                UserEmail = SessionConfig.User.UserEmail
            };
            var rtn = _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
            return JsonConvert.DeserializeObject<bool>(rtn);
        }

        public void ResetAccount(string email)
        {
            var ip = CoreConnections.Account + "ResetAccount";
            var model = new User
            {
                UserEmail = email
            };
            _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
        }

        public void SetMenu(MenusRole model)
        {
            var ip = CoreConnections.Company + "CreateMenuRole";
            _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
        }

        public void UpdateLastActivity(string email)
        {
            var ip = CoreConnections.Account + "UpdateLastActivity";
            var model = new User
            {
                UserEmail = email
            };
            _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
        }

        public string TranslateText(string text, string to, string from)
        {
            if (string.IsNullOrEmpty(from)) from = "en";
            if (string.IsNullOrEmpty(text)) return null;
            var ip = CoreConnections.Company + "Translate";
            var lst = new List<string>
            {
                text,from,to
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<string>(result);
        }

        public void UpdatePicture(string route, string id)
        {
            var ip = CoreConnections.Account + "UpdateProfilePicture";
            var lst = new List<string>
            {
                route,id
            };
            var result = _core.GetResponse(ip, lst);
        }
    }
}