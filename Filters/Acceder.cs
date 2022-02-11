using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Licorera.Filters
{
    public class Acceder : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Si session es nulo , entonces retorne al Login
            var usuario = HttpContext.Current.Session["Usuario"];

            var rol = HttpContext.Current.Session["Rol"];



            if (usuario == null || rol.ToString() != "1" )
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }


            base.OnActionExecuting(filterContext);
        }
    }
}