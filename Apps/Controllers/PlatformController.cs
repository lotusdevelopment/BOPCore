using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Apps.App_Start;
using Apps.Logic;
using Apps.Logic.Authorize;
using Apps.Resources;
using LotusViewModels.General;
using LotusViewModels.Request;

namespace Apps.Controllers
{
    public class PlatformController : Controller
    {
        private readonly Generics _gen = new Generics();
        public ActionResult Home()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult ChangeApp(string appid)
        {
            SessionConfig.ChangeApp(Convert.ToInt32(appid));
            return Json(true);
        }

        [HttpGet]
        public JsonResult GetCs()
        {
            var rtn = new List<Dictionary<string, string>>();
            var dct = new Dictionary<string, string>();
            dct.Add(nameof(CoreConnections.Account), CoreConnections.Account);
            dct.Add(nameof(CoreConnections.Company), CoreConnections.Company);
            dct.Add(nameof(CoreConnections.LotusSales), CoreConnections.LotusSales);
            dct.Add(nameof(CoreConnections.Sales), CoreConnections.Sales);
            rtn.Add(dct);
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUs()
        {
            var rtn = new AllUserInfo
            {
                User = SessionConfig.User,
                Roles = SessionConfig.Roles,
                Company = SessionConfig.Company
            };
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult CreateEntity(CreateEntity createEntity)
        {
            createEntity.User.SellerCode = string.Concat(createEntity.Name.Substring(0, 4), createEntity.User.Country.Substring(0, 2), createEntity.User.UserIcard).ToUpper();
            createEntity.User.Password = string.Concat(createEntity.Name.Replace(" ", ""), "123*");
            createEntity.Logo = SessionConfig.Company.logo;
            createEntity.Background = SessionConfig.Company.background;
            createEntity.MainColor = SessionConfig.Company.maincolor;
            createEntity.SecondaryColor = SessionConfig.Company.seccolor;
            createEntity.Url = SessionConfig.Company.url;
            createEntity.Favicon = SessionConfig.Company.favicon;
            Task.Factory.StartNew(() => _gen.CreateEntity(createEntity));
            return Json(true);
        }

        public string GetTitle(string title)
        {
            try
            {
                if (SessionConfig.User.UserLanguage != "en")
                    return _gen.TranslateText(title, SessionConfig.User.UserLanguage, null);
                return title;
            }
            catch (Exception)
            {
                return _gen.TranslateText(title, "en", null);
            }

        }
        [HttpPost]
        public JsonResult SalesReport(string id, string type)
        {
            var rtn = this.GetUs();
            Console.WriteLine(rtn);
            //  var iden = rtn.User.UserId;
            // var rol = rtn.Roles[0].RoleId; SalesReport(iden.ToString(), rol.ToString())

            return Json(true);
        }
    }
}