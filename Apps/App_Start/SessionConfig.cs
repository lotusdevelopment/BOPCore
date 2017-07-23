using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using LotusViewModels.General;
using ViewModels.ILotusAssistance;
using LotusViewModels.Response;
using Apps.Logic;


// ReSharper disable once CheckNamespace
namespace Apps.App_Start
{
    public static class SessionConfig
    {
        private static readonly Generics _gen = new Generics();

        public static User User
        {
            get
            {
                return GetUser();
            }
            private set
            {
            }
        }
        public static Empresas Company
        {
            get
            {
                return GetCompany();
            }
            private set
            {
            }
        }
        public static List<Roles> Roles
        {
            get
            {
                return GetRoles();
            }
            private set
            {
            }
        }
        public static List<LotusViewModels.General.Apps> Apps
        {
            get
            {
                return GetApps();
            }
            private set
            {
            }
        }
        public static LotusViewModels.General.Apps CurrentApp
        {
            get
            {
                return GetCurrentApp();
            }
            private set
            {
            }
        }

        public static void SetSession(LoginResponse userResponse)
        {
            System.Web.HttpContext.Current.Session["CurrentLogin"] = userResponse;
        }

        public static void DestroySession()
        {
            User = null;
            Company = null;
            Roles = null;
            Apps = null;
            CurrentApp = null;
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
        }

        public static void UpdateLastActivity()
        {
            _gen.UpdateLastActivity(User.UserEmail);
        }

        public static Empresas Allow()
        {
            UpdateLastActivity();
            return Company;
        }

        public static bool CheckAccess()
        {
            if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["IsProduction"]))
                return true;
            try
            {
                var controller = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString().ToUpper();
                var action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString().ToUpper();
                if (controller.Equals("ACCOUNT") && action.Equals("LOGIN"))
                    return true;
                if (controller.Equals("PLATFORM") && action.Equals("HOME"))
                    return true;
                var availableMenus = CurrentApp.Menu.Where(x => !string.IsNullOrEmpty(x.Action)
                && !string.IsNullOrEmpty(x.Controller)).ToList();
                return availableMenus.Any(x => x.Action.ToUpper().Equals(action) && x.Controller.ToUpper().Equals(controller));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ChangeApp(int id)
        {
            var app = Apps.FirstOrDefault(x => x.AppId == id);
            if (app == null)
                return;
            System.Web.HttpContext.Current.Session["CurrentApp"] = null;
            System.Web.HttpContext.Current.Session["CurrentApp"] = app;
        }

        private static User GetUser()
        {
            if (System.Web.HttpContext.Current.Session["CurrentLogin"] != null)
            {
                var ad = System.Web.HttpContext.Current.Session["CurrentLogin"] as LoginResponse;
                return ad.User;
            }
            return null;
        }
        private static Empresas GetCompany()
        {
            if (System.Web.HttpContext.Current.Session["CurrentLogin"] != null)
            {
                var ad = System.Web.HttpContext.Current.Session["CurrentLogin"] as LoginResponse;
                return ad.Company;
            }
            return null;
        }
        private static List<Roles> GetRoles()
        {
            if (System.Web.HttpContext.Current.Session["CurrentLogin"] != null)
            {
                var ad = System.Web.HttpContext.Current.Session["CurrentLogin"] as LoginResponse;
                return ad.Roles;
            }
            return null;
        }
        private static List<LotusViewModels.General.Apps> GetApps()
        {
            if (System.Web.HttpContext.Current.Session["CurrentLogin"] != null)
            {
                var ad = System.Web.HttpContext.Current.Session["CurrentLogin"] as LoginResponse;
                if (User.UserLanguage != "en") foreach (var app in ad.Apps) foreach (var menu in app.Menu) menu.Name = _gen.TranslateText(menu.Name, User.UserLanguage, null);
                return ad.Apps;
            }
            return null;
        }
        private static LotusViewModels.General.Apps GetCurrentApp()
        {
            if (System.Web.HttpContext.Current.Session["CurrentLogin"] != null)
            {
                if (Apps.Count > 0)
                {
                    if (System.Web.HttpContext.Current.Session["CurrentApp"] != null)
                        return System.Web.HttpContext.Current.Session["CurrentApp"] as LotusViewModels.General.Apps;
                    System.Web.HttpContext.Current.Session["CurrentApp"] = Apps.FirstOrDefault();
                    return System.Web.HttpContext.Current.Session["CurrentApp"] as LotusViewModels.General.Apps;
                }
                System.Web.HttpContext.Current.Session["CurrentApp"] = null;
                return null;
            }
            return null;
        }
    }
}