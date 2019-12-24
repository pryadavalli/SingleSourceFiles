using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceToken.Helpers
{
    public class AuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public virtual async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //if (!context.HttpContext.Request.IsHttps)
            //    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string accessToken = authHeader?.Split(" ").Length > 0 ? authHeader?.Split(" ")[1] : null;
                string resultToken = TokenManager.ValidateToken(accessToken);

                if(resultToken == null)
                    context.Result = new UnauthorizedResult();

                return;
            }
            // Return authentication type (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = "Bearer";

            if (!IsProtectedAction(context))
            {
                return;
            }

            // Return unauthorized
            context.Result = new UnauthorizedResult();
        }
        private bool IsProtectedAction(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return false;

            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
            var actionMethodInfo = controllerActionDescriptor.MethodInfo;

            var authorizeAttribute = controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
            if (authorizeAttribute != null)
                return true;

            authorizeAttribute = actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>();
            if (authorizeAttribute != null)
                return true;

            return false;
        }
        private bool IsUserAuthenticated(AuthorizationFilterContext context)
        {
            return context.HttpContext.User.Identity.IsAuthenticated;
        }

        //private string GetActionId(AuthorizationFilterContext context)
        //{
        //    var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        //    var area = controllerActionDescriptor.ControllerTypeInfo.
        //                         GetCustomAttribute<AreaAttribute>()?.RouteValue;
        //    var controller = controllerActionDescriptor.ControllerName;
        //    var action = controllerActionDescriptor.ActionName;

        //    return $"{area}:{controller}:{action}";
        //}

    }
}
