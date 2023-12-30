using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StoreThoiTrang63136126.Models;
using StoreThoiTrang63136126.Session;

namespace StoreThoiTrang63136126.Areas.Admin.Controllers
{
    public class Base63136126Controller : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session[ConstaintUser.ADMIN_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Controller = "Login63136126",
                    Action = "Index",
                    Area = "Admin"
                }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
