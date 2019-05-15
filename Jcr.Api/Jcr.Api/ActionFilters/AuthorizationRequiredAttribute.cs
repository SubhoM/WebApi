using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jcr.Business;

namespace Jcr.Api.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";
        private const string UserID = "UserID";

        private int SiteID = 0;
        private int TracerCustomID = 0;

        public string ParameterName = string.Empty;

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
                TokenServices tokenService = new TokenServices();
            if (filterContext.Request.Headers.Contains(Token) && filterContext.Request.Headers.Contains(UserID))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();
                var userId = filterContext.Request.Headers.GetValues(UserID).First();
                
                // Validate Token
                if (tokenService.ValidateToken(tokenValue,userId))
                {
                    if (string.IsNullOrEmpty(ParameterName) == false)
                    {
                        var actionArgs = (int)filterContext.ActionArguments[this.ParameterName];

                        if (string.Equals(ParameterName, "siteid", System.StringComparison.CurrentCultureIgnoreCase))
                            SiteID = actionArgs;
                        else if (string.Equals(ParameterName, "tracercustomid", System.StringComparison.CurrentCultureIgnoreCase)
                            || string.Equals(ParameterName, "tracerid", System.StringComparison.CurrentCultureIgnoreCase))
                            TracerCustomID = actionArgs;
                    }

                    var isValid = SiteID == 0 && TracerCustomID == 0;

                    var userID = 0;
                    int.TryParse(userId, out userID);

                    //Valid Stuff
                    if (SiteID > 0)
                    {
                        isValid = tokenService.ValidateUserSiteAccess(userID, SiteID);
                    }
                    else if (TracerCustomID > 0)
                    {
                        isValid = tokenService.ValidateUserTracerAccess(userID, TracerCustomID);
                    }


                    if(isValid == false)
                        filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                }
                else
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);

        }
    }


    
    }
