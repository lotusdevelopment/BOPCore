using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Apps.App_Start
{
    public static class CoreConnections
    {
        public static string Company
        {
            get
            {
                var rt = GetRoutes();
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsLocal"]))
                    return rt.Company.local;
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsProduction"]))
                    return rt.Company.production;
                return rt.Company.test;
            }
            private set { }
        }
        public static string Account
        {
            get
            {
                var rt = GetRoutes();
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsLocal"]))
                    return rt.Account.local;
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsProduction"]))
                    return rt.Account.production;
                return rt.Account.test;
            }
            private set { }
        }
        public static string LotusSales
        {
            get
            {
                var rt = GetRoutes();
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsLocal"]))
                    return rt.LotusSales.local;
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsProduction"]))
                    return rt.LotusSales.production;
                return rt.LotusSales.test;
            }
            private set { }
        }
        public static string Sales
        {
            get
            {
                var rt = GetRoutes();
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsLocal"]))
                    return rt.Sales.local;
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["IsProduction"]))
                    return rt.Sales.production;
                return rt.Sales.test;
            }
            private set { }
        }

        private static CustomRoutes GetRoutes()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var dir = Path.GetDirectoryName(path);
            var pc = File.ReadAllText(dir + "\\customroutes.json");
            var routes = JsonConvert.DeserializeObject<CustomRoutes>(pc);
            return routes;
        }
    }

    public class CustomRoutes
    {
        public CustomRoutesLocal Company { get; set; }
        public CustomRoutesLocal Account { get; set; }
        public CustomRoutesLocal LotusSales { get; set; }
        public CustomRoutesLocal Sales { get; set; }
    }

    public class CustomRoutesLocal
    {
        public string local { get; set; }
        public string production { get; set; }
        public string test { get; set; }
    }
}