using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apps.Controllers
{
    public class HelpDeskController : Controller
    {
        // GET: HelpDesk
        public ActionResult Home()
        {
            return View();
        }
    }
}