using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ExchangeApp.Helpers
{
    public class NotFoundFilter : ActionFilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is ArgumentNullException)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}