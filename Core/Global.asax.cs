using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace Core
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
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
    }
}