using Apps.App_Start;
using Apps.Logic.Authorize;
using LotusViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Controllers
{
    public class DashBoardController : Controller
    {
        
        public ActionResult Seller()
        {
            return View();
        }

      
    }
}