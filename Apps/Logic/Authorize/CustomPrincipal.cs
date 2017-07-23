using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Apps.App_Start;
using Apps.Resources;
using Apps.Models;
using LotusViewModels.General;
using LotusViewModels.Request;
using LotusViewModels.Response;
using Newtonsoft.Json;

namespace Apps.Logic.Authorize
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly CoreConnect _core = new CoreConnect();

        private User Account;
        public IIdentity Identity { get; set; }

        public CustomPrincipal(string username, string password)
        {
            this.Account = LoginUser(username, password);
            if (this.Account != null)
                this.Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return true;
        }

        private User LoginUser(string user, string password)
        {
            var ip = CoreConnections.Account + "LoginSystem";
            var login = new LoginRequest
            {
                User = user,
                Password = password
            };
            var suc = _core.GetResponse(ip, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(login)));
            if (string.IsNullOrEmpty(suc))
                return null;
            var srl = JsonConvert.DeserializeObject<LoginResponse>(suc);
            if (!srl.Valid)
                return null;
            if (DateTime.Now.AddMinutes(-30) <= Convert.ToDateTime(srl.User.LastActivity))
            {
                //Para que tome el tema de los 30 mins para el login
                HttpContext.Current.Session["ValidUpdatedTime"] = true;
                //return null;
            }
            SetSession(srl);
            return srl.User;
        }

        private void SetSession(LoginResponse user)
        {
            SessionConfig.SetSession(user);
        }
    }
}