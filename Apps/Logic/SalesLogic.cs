using System;
using System.Collections.Generic;
using System.Linq;
using ViewModels.General;
using Apps.Resources;
using LotusViewModels.General;
using LotusViewModels.ILotusAssistance;
using Newtonsoft.Json;
using LotusViewModels.Response;
using ViewModels;
using System.Text;
using LotusViewModels.Request;
using Apps.App_Start;
using ViewModels.Request;

namespace Apps.Logic
{
    public class SalesLogic
    {
        private readonly CoreConnect _core = new CoreConnect();

        public string GetSellerCode(string id, bool external)
        {
            var ip = CoreConnections.Company + "GetSellerCode";
            var lst = new List<string>
            {
                id,external.ToString()
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<string>(result);
        }

        public void SendPaymentEmail(string name, string id, string phone, string email,
            string address, string birthday, string plan, string country, string value, string buyerEmail,
            string buyerName, string bid)
        {
            var ip = CoreConnections.Company + "SendPaymentEmail";
            var lst = new List<string>
            {
                name,id,phone,email,address,birthday,plan,country,value,buyerEmail,buyerName,bid
            };
            _core.GetResponse(ip, lst);
        }

        public List<Countries> GetCountries(string sellerId)
        {
            var ip = CoreConnections.Company + "GetCountries";
            var result = _core.GetResponseGet(ip, sellerId);
            return JsonConvert.DeserializeObject<List<Countries>>(result);
        }

        public List<Plans> GetLatamPlans(bool familiar, bool anual, string sellerCode)
        {
            var ip = CoreConnections.Sales + "GetLatamPlans";
            var lst = new List<string>
            {
                familiar.ToString(),anual.ToString(),sellerCode
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<List<Plans>>(result);
        }

        public List<Plans> GetLatamCr(int country, bool anual, string sellerCode)
        {
            var ip = CoreConnections.Sales + "GetCrPlans";
            var lst = new List<string>
            {
                country.ToString(),anual.ToString(),sellerCode
            };
            var result = _core.GetResponseGet(ip, lst);
            return JsonConvert.DeserializeObject<List<Plans>>(result);
        }

        public string GetSellerId(string sellerCode)
        {
            var ip = CoreConnections.Company + "sellerCode";
            var result = _core.GetResponseGet(ip, sellerCode);
            return JsonConvert.DeserializeObject<string>(result);
        }

        public List<Seller> GetSellersFromFile(string location)
        {
            var sellers = new List<Seller>();
            return sellers;
        }

        public List<string> GetPaymentForms(string id)
        {
            var ip = CoreConnections.Sales + "GetPaymentForms";
            var result = _core.GetResponseGet(ip, id);
            return JsonConvert.DeserializeObject<List<string>>(result);
        }

        public void CreateGroup(string name, string cId)
        {
            var ip = CoreConnections.Company + "InsertOrUpdateGroup";
            var lst = new List<string>
            {
                cId,name
            };
            var result = _core.GetResponse(ip, lst);
        }

        public string GetGenerals()
        {
            var credentials = new AuthorizeModel
            {
                IsEncrypted = false,
                Password = "admin",
                User = "admin"
            };
            var url = CoreConnections.LotusSales + "GetSaleOptions";
            return _core.GetResponse(url, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(credentials)));
        }

        public string GetNewProducts(int pType, int vType, int pPeriod, int curr)
        {
            var credentials = new AuthorizeModel
            {
                IsEncrypted = false,
                Password = "admin",
                User = "admin"
            };
            var rq = new GetProductsViewModel
            {
                Authorization = credentials,
                Currency = curr,
                InfoContractProduct = pType,
                paymentPeriod = pPeriod,
                ValidityTime = vType,
                SellerCode = SessionConfig.User.SellerCode
            };
            var url = CoreConnections.LotusSales + "GetProducts";
            return _core.GetResponse(url, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rq)));
        }

        public string SetSale(SaleRequestBeneficiaries sale)
        {
            var url = CoreConnections.LotusSales + "SaleRequest";
            var rq = new NewSaleFormat
            {
                Process = "Inner Lotus App Process - " + DateTime.Now.ToString(),
                LotusPayment = true,
                SaleInfo = sale
            };
            return _core.GetResponse(url, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rq)));
        }
    }
}