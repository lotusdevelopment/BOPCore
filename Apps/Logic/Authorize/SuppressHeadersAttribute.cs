using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Apps.Logic.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SuppressHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }
    }
}