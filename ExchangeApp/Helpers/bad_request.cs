using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ExchangeApp.Helpers
{
    public class BadRequestFilter : ActionFilterAttribute, IActionFilter
{
    void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
    {
        // TODO: Add your acction filter's tasks here
        if (filterContext.ActionParameters["id"] == null)
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        
        base.OnActionExecuting(filterContext);
    }
}
}