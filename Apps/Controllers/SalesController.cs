using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Apps.App_Start;
using Apps.Logic;
using Apps.Logic.Authorize;
using Apps.LotusSalesService;
using Encrypt;
using ViewModels;
using ViewModels.General;
using ViewModels.Request;
using ViewModels.Response;

namespace Apps.Controllers
{
    public class SalesController : Controller
    {
        private readonly IThirdPartyServices _svc = new ThirdPartyServicesClient();
        private readonly Encryption _enc = new Encryption();
        private readonly SalesLogic _salesLogic = new SalesLogic();
        private readonly Generics _gen = new Generics();
        private readonly SalesLogic _sl = new SalesLogic();

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult NewSale()
        {
            return View();
        }

        public ActionResult CreateGroup()
        {
            return View();
        }

        public ActionResult CreateEntity()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult CreateUserDefault()
        {
            return View();
        }

        public ActionResult Comissions()
        {
            return View();
        }

        public ActionResult SaleReview()
        {
            return View();
        }

        public ActionResult ExternalSale()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPf()
        {
            return Json(_salesLogic.GetPaymentForms(SessionConfig.User.UserId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPlansLatam(bool familiar, bool anual)
        {
            return Json(_salesLogic.GetLatamPlans(familiar, anual, SessionConfig.User.UserId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLatamCr(int country, bool anual)
        {
            return Json(_salesLogic.GetLatamCr(country, anual, SessionConfig.User.UserId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlansByCountries(string country)
        {
            var svc = _svc.PlansRequestByCountry(new PlanByCountryRequest
            {
                Credentials = new AuthorizeModel
                {
                    IsEncrypted = true,
                    Password = _enc.Encrypt(WebConfigurationManager.AppSettings["BusPsw"]),
                    User = _enc.Encrypt(WebConfigurationManager.AppSettings["BusId"])
                },
                CountryId = country,
                SellerCode = _salesLogic.GetSellerCode(SessionConfig.User.UserId, false)
            });
            return svc.Error.IsError ? Json(new PlansResponse
            {
                Plans = new List<Plans>()
            }) : Json(svc.Plans);
        }

        public JsonResult SetSale(SaleRequestBeneficiaries sale)
        {            
            sale.Credentials = new AuthorizeModel
            {
                IsEncrypted = true,
                Password = _enc.Encrypt(WebConfigurationManager.AppSettings["BusPsw"]),
                User = _enc.Encrypt(WebConfigurationManager.AppSettings["BusId"])
            };
            sale.SellerCode = SessionConfig.User.SellerCode;
            if (sale.CardData != null)
            {
                sale.CardData.CardName = _enc.Encrypt(sale.CardData.CardName);
                sale.CardData.CardNumber = _enc.Encrypt(sale.CardData.CardNumber);
                sale.CardData.CardCvc = _enc.Encrypt(sale.CardData.CardCvc);
                sale.CardData.CardExpirationMonth = _enc.Encrypt(sale.CardData.CardExpirationMonth);
                sale.CardData.CardExpirationYear = _enc.Encrypt(sale.CardData.CardExpirationYear);
            }
            var sl = _sl.SetSale(sale);
            return Json(sl);
        }

        /*private string InsertSale(SaleRequestBeneficiaries sale)
        {
            try
            {
                var ts = _bd.TemporarySales.Add(new TemporarySales
                {
                    Address = sale.Street,
                    Birthday = sale.Birthday,
                    Cedula = sale.CustomerId,
                    Email = sale.Email,
                    Id = Guid.NewGuid().ToString(),
                    LastName = sale.LastName,
                    Mobile = sale.Cellphone,
                    Name = sale.FirstName,
                    PlanId = Convert.ToInt32(sale.Plan),
                    SellerId = User.Identity.GetUserId(),
                    BuyerName = sale.BuyerName,
                    BuyerPhone = sale.BuyerCellPhone,
                    SaleDate = DateTime.Now,
                    SaleStatus = false,
                    BuyerEmail = sale.BuyerEmail
                });
                _bd.SaveChanges();
                return ts.Id;
            }
            catch (Exception e)
            {
                return null;
            }
        }*/

        [HttpPost]
        [CustomAuthorize]
        public JsonResult CreateGroupI(string name)
        {
            var cId = SessionConfig.Company.id.ToString();
            var groups = name.Split('-');
            foreach (var group in groups)
                if (!string.IsNullOrEmpty(group))
                    Task.Factory.StartNew(() => _salesLogic.CreateGroup(group, cId));
            return Json(true);
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult UpdateComissions(string commit, string percs, string q)
        {
            Task.Factory.StartNew(() => _gen.UpdateComissions(commit, percs, q));
            return Json(true);
        }


    }
}