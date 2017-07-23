using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using Apps.App_Start;
using Apps.Logic;
using Apps.Logic.Authorize;
using Apps.LotusSalesService;
using Encrypt;
using LotusViewModels.General;
using LotusViewModels.ILotusAssistance;
using ViewModels;
using ViewModels.General;
using ViewModels.Request;
using System.IO;

namespace Apps.Controllers
{
    public class OthersController : Controller
    {
        private readonly Generics _cnt = new Generics();
        private readonly SalesLogic _sl = new SalesLogic();
        private readonly Encryption _enc = new Encryption();
        private readonly IThirdPartyServices _svc = new ThirdPartyServicesClient();

        [SuppressHeaders]
        public ActionResult Maps2()
        {
            return View();
        }

        [SuppressHeaders]
        public ActionResult Carnet(string id)
        {
            var img = _cnt.GetCarnet(id);
            if (img == null)
                return Content("");
            return new FileContentResult(img.Url, "image/jpeg");
        }

        public ActionResult SaleReview()
        {
            return View();
        }

        public JsonResult ValidateSale(string phone, string id, string format)
        {
            Session["SaleValidationId"] = format;
            phone = Regex.Replace(phone, "[^0-9.]", "");            
            Session["SaleValidationSaleId"] = id;
            Session["SaleValidationSalePhone"] = phone;
            return Json(_cnt.CheckExistingSale(phone, id, format));
        }

        public void UpdateAndSendData(SaleVal model)
        {
            if (!Session["SaleValidationId"].ToString().ToUpper().Equals(model.Session.ToUpper()))
                return;
            Task.Factory.StartNew(() => _cnt.UpdateAndSendData(model));
        }

        [HttpGet]
        public async Task<JsonResult> GetAllSites()
        {
            return await Task<JsonResult>.Factory.StartNew(() =>
            {
                try
                {
                    var rtn = _cnt.GetAllSites();
                    return Json(rtn, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new List<Sucursal>(), JsonRequestBehavior.AllowGet);
                }
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetSitePerFilter(string param, string filter, string lat, string lng)
        {
            return await Task<JsonResult>.Factory.StartNew(() =>
            {
                try
                {
                    var cnt = Convert.ToInt32(filter);
                    if (string.IsNullOrEmpty(param))
                        param = GetCityName(lat, lng);
                    var rtn = _cnt.GetSpecificMapPlaces(param, cnt);
                    if (rtn.Count > 0)
                        return Json(rtn, JsonRequestBehavior.AllowGet);
                    param = GetCityName(lat, lng);
                    rtn = _cnt.GetSpecificMapPlaces(param, cnt);
                    return Json(rtn, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
            });
        }

        private string GetCityName(string lat, string lng)
        {
            try
            {
                var xDoc = new XmlDocument();
                xDoc.Load("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat + "," + lng +
                          "&location_type=ROOFTOP&result_type=street_address&key=" +
                          ConfigurationManager.AppSettings["GoogleApiKey"]);
                var xNodelst = xDoc.GetElementsByTagName("result");
                var xNode = xNodelst.Item(0);
                if (xNode == null) return "BOGOTÁ";
                var il = xNode.SelectSingleNode("address_component[5]/long_name").InnerText;
                return il;
            }
            catch (Exception e)
            {
                return "BOGOTÁ";
            }
        }

        public Empresas CompanyProperties()
        {
            return _cnt.CompanyProperties(GetCompanyId());
        }

        public string GetCompanyId()
        {
            try
            {
                var storedId = System.Web.HttpContext.Current.Session["storedId"];
                if (storedId != null)
                    return storedId.ToString();
                var url = System.Web.HttpContext.Current.Request.Url.ToString().ToUpper();
                url = url.Replace("%2F", "");
                url = url.Replace("RETURNURL=", "");
                url = url.Replace("%3F", "");
                url = url.Replace("%3D", "=");
                var myUri = new Uri(url);
                var id = HttpUtility.ParseQueryString(myUri.Query).Get("cpn");
                if (!string.IsNullOrEmpty(id))
                {
                    System.Web.HttpContext.Current.Session["storedId"] = id;
                    return id;
                }
                var cp = _cnt.GetCompanyId(SessionConfig.User.UserId);
                if (cp != null)
                {
                    System.Web.HttpContext.Current.Session["storedId"] = cp;
                    return cp;
                }
            }
            catch (Exception e)
            {
                //
            }
            return string.Empty;
        }

        [HttpGet]
        public JsonResult GetExternalPlans(string cn, string ci, string f, string cr)
        {
            return Json(_cnt.GetExternalPlans(cn, ci, Convert.ToBoolean(f), cr,
                _sl.GetSellerId(WebConfigurationManager.AppSettings["ExternalSeller"])), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SetSale(SaleModl saleData)
        {
            var sale = new SaleRequestBeneficiaries
            {
                Credentials = new AuthorizeModel
                {
                    IsEncrypted = true,
                    Password = _enc.Encrypt(WebConfigurationManager.AppSettings["BusPsw"]),
                    User = _enc.Encrypt(WebConfigurationManager.AppSettings["BusId"])
                },
                SellerCode = WebConfigurationManager.AppSettings["ExternalSeller"],
                BuyerName = saleData.BuyerName,
                Birthday = saleData.Birthday,
                CustomerId = saleData.CustomerId,
                Email = saleData.Email,
                FirstName = saleData.FirstName,
                Gender = saleData.Gender,
                LastName = saleData.LastName,
                MotherName = saleData.MotherName,
                Neighborhood = saleData.Neighborhood,
                PaymentType = saleData.PaymentType,
                Plan = saleData.Plan,
                PlateNumber = saleData.PlateNumber,
                State = saleData.State,
                Street = saleData.Street,
                Url = saleData.Url,
                ZipCode = saleData.ZipCode,
                BuyerCellPhone = saleData.BuyerCellPhone,
                BuyerEmail = saleData.BuyerEmail,
                Cellphone = saleData.Cellphone,
                City = saleData.City,
                CompanySaleId = saleData.CompanySaleId,
                Complement = saleData.Complement,
                CountryId = saleData.CountryId
            };
            if (saleData.CardData != null)
            {
                sale.CardData = new ClientCard
                {
                    CardCvc = _enc.Encrypt(saleData.CardData.CardCvc),
                    CardExpirationMonth = _enc.Encrypt(saleData.CardData.CardExpirationMonth),
                    CardExpirationYear = _enc.Encrypt(saleData.CardData.CardExpirationYear),
                    CardName = _enc.Encrypt(saleData.CardData.CardName),
                    CardNumber = _enc.Encrypt(saleData.CardData.CardNumber)
                };
            }
            if (saleData.Beneficiaries != null)
            {
                sale.Beneficiaries = saleData.Beneficiaries.Select(ben => new Beneficiary
                {
                    Id = ben.Id,
                    Lastname = ben.Lastname,
                    Name = ben.Name
                }).ToList();
            }
            var sl = _svc.SaleProcessCardPayment(sale);
            return Json(sl);
        }

        [HttpGet]
        public JsonResult GetCountryInfo(string name)
        {
            return Json(_cnt.GetCountryInfo(name), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            return Json(_sl.GetCountries(SessionConfig.User.UserId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGenerals()
        {
            return Json(_sl.GetGenerals());
        }

        [HttpPost]
        public JsonResult GetNewProducts(int pType, int vType, int pPeriod, int curr)
        {
            return Json(_sl.GetNewProducts(pType, vType, pPeriod, curr));
        }

        [HttpPost]
        public void UpdateProfileImg()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                try
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["ImageUpload"];
                    var filename = string.Concat(SessionConfig.User.Name, SessionConfig.User.UserIcard,
                        DateTime.Now.ToShortDateString(), DateTime.Now.Second,
                        DateTime.Now.Millisecond, ".", pic.FileName.Split('.').Last());
                    filename = filename.Replace("/", "");
                    var upload = WebConfigurationManager.AppSettings["ProfilePicturesUpload"].ToString();                    
                    var path = Path.Combine(upload, filename);
                    pic.SaveAs(path);
                    _cnt.UpdatePicture(filename, SessionConfig.User.UserId);
                }
                catch (Exception es)
                {
                    //
                }
            }
        }
    }
}