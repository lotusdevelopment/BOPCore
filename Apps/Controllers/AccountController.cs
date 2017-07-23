using System.Threading.Tasks;
using System.Web.Mvc;
using Apps.App_Start;
using Apps.Logic;
using Apps.Models;
using Apps.Logic.Authorize;
using LotusViewModels.General;

namespace Apps.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        private readonly Generics _gen = new Generics();
        public ActionResult LogIn()
        {
            return SessionConfig.User != null ? RedirectToPlatform() : View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult NewUser()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.LoginError = false;
            System.Web.HttpContext.Current.Session["ValidUpdatedTime"] = true;
            if (!ModelState.IsValid)
                return View(model);
            var pr = new CustomPrincipal(model.Email, model.Password);
            if (pr.Identity != null)
                return RedirectToLocal(returnUrl);
            if (!(bool)System.Web.HttpContext.Current.Session["ValidUpdatedTime"])
                return View(model);
            ViewBag.LoginError = true;
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Home", "Platform");
        }

        [HttpPost]
        public JsonResult LogOff()
        {
            SessionConfig.DestroySession();
            return Json(true);
        }

        public ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult RedirectToPlatform()
        {
            return RedirectToAction("Home", "Platform");
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult CreateUser(User usr)
        {
            usr.SellerCode = string.Concat(SessionConfig.Company.name.Substring(0, 4), usr.Country.Substring(0, 2), usr.UserIcard).ToUpper();
            usr.Password = string.Concat(SessionConfig.Company.name.Replace(" ", ""), "123*");
            usr.UserUc = 0;
            Task.Factory.StartNew(() => _gen.CreateUser(usr));
            return Json(true);
        }

        [HttpPost]
        [CustomAuthorize]
        public JsonResult ChangePassword(string np, string pc)
        {
            return Json(_gen.ChangePassword(np));
        }

        [HttpPost]
        public JsonResult PasswordRecovery(string email)
        {
            if (!string.IsNullOrEmpty(email))
                Task.Factory.StartNew(() => _gen.ResetAccount(email));
            return Json(true);
        }
    }
}