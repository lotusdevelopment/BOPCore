using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Apps.Logic.Authorize;
using Apps.Logic;
using LotusViewModels.General;

namespace Apps.Controllers
{
    public class AppsAdminController : Controller
    {
        private readonly Generics _gen = new Generics();

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult CreateEntity()
        {
            return View();
        }

        public ActionResult CreateMenu()
        {
            return View();
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        public ActionResult CreateContract()
        {
            return View();
        }

        public ActionResult CreateContractProduct()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult CreateProduct(string name, string treat, string per, int q)
        {
            _gen.CreateProduct(name, treat, per, q);
            return Json(true);
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult SetMenu(MenusRole model)
        {
            Task.Factory.StartNew(()=> _gen.SetMenu(model));
            return Json(true);
        }
    }
}