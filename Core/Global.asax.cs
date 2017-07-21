using Core.InnerLogic;
using System;
using System.IO;
using System.Runtime.Caching;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace Core
{
    public class Global : HttpApplication
    {
        private readonly Tem _tem = new Tem();
        private readonly Funcional _fun = new Funcional();
        private readonly LocationsServices _lcn = new LocationsServices();

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            _lcn.SaveProcessLog("","---Start Update Services---");
            TimServices();
            FuncionalServices();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            EnableCrossDmainAjaxCall();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.Add(new ServiceRoute("UpStream", new WebServiceHostFactory(), typeof(UpStream)));
        }

        private void EnableCrossDmainAjaxCall()
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods",
                              "GET, POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers",
                              "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age",
                              "1728000");
                HttpContext.Current.Response.End();
            }
        }

        private void TimServices()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(30);
            var timer = new System.Threading.Timer((ex) =>
            {
                _tem.CheckAndSaveTemDirections(_lcn.GetPaths(1));
            }, null, startTimeSpan, periodTimeSpan);
        }

        private void FuncionalServices()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(30);
            var timer = new System.Threading.Timer((ex) =>
            {
                _fun.CheckAndSaveFuncionalDirections(_lcn.GetPaths(2));
            }, null, startTimeSpan, periodTimeSpan);
        }
    }
}