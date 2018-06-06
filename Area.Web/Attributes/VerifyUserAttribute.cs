using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Attributes
{
    public class VerifyUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLogged = context.HttpContext.Session["UserID"];
            if (isLogged == null)
            {
                context.Result = new RedirectResult(string.Format("/User/Login", context.HttpContext.Request.Url.AbsolutePath));
            }
        }
    }
}