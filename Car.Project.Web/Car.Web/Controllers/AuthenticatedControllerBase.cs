using Carsale.Web.Authorization;
using Carsale.Security;
using Carsale.Security.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace Carsale.Web.Controllers
{
    [Authorize]
    public class AuthenticatedControllerBase : Controller
    {

        public AuthenticatedControllerBase()
        {

        }
        public UserDetails CarsaleUser
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                   return ((CarsalePrincial)(HttpContext.User)).UserDetails;
                }
                else if (HttpContext.Items.Contains("User"))
                {
                    return (UserDetails)HttpContext.Items["User"];
                }
                else
                {
                    return null;
                }
            }
        }


        protected override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.User != null && filterContext.HttpContext.User is CarsalePrincial)
            {
                var principal = (CarsalePrincial)filterContext.HttpContext.User;
                
                foreach (var userRole in GetRequiredUserRoles(filterContext))
                    if (principal.IsInRole(userRole.ToString()))
                        return;
            }
            var result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
            };

            filterContext.Result = result;
        }
        private static IEnumerable<UserRole> GetRequiredUserRoles(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var controllerAttributes = filterContext.ActionDescriptor.ControllerDescriptor
                .GetCustomAttributes(typeof(AuthorizeUserRoleAttribute), true);

            var actionAttributes = filterContext.ActionDescriptor
                .GetCustomAttributes(typeof(AuthorizeUserRoleAttribute), true);

            return Enumerable.Concat(controllerAttributes, actionAttributes)
                .Cast<AuthorizeUserRoleAttribute>()
                .SelectMany(a => a.UserRoles)
                .Distinct();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (CarsaleUser != null)
                this.ViewBag.UserRole = CarsaleUser.UserRole.ToString();
        }
        protected override void OnException(ExceptionContext filterContext)
        {         
            var controllerName = string.Empty;
            var actionName = string.Empty;
            try
            {
                controllerName = (string)filterContext.RouteData.Values["controller"];
                actionName = (string)filterContext.RouteData.Values["action"];
            }
            catch
            { }
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.ErrorException(string.Format("Exception was thrown from controller {0} while executing action {1}  ", controllerName, actionName), filterContext.Exception);
            var result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
            };

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            base.OnException(filterContext);

        }
    }
}