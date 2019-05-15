
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jcr.Api.Filters;
//using AttributeRouting.Web.Http;
using System.Configuration;
using Jcr.Business;
using Jcr.Data;
using Jcr.Api.Helpers;

namespace Jcr.Api.Controllers
{
    [ApiAuthenticationFilter]
    [RoutePrefix("Authenticate")]
    public class AuthenticateController : ApiController
    {

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>
       
        [VersionedRoute("get/token", 1)]
        [HttpPost]
        public HttpResponseMessage Authenticate()
        {
          
            HttpResponseMessage response = null;

            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {                    
                    response = GetAuthToken(basicAuthenticationIdentity);
                }            

            }

            return response;
        }

        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private HttpResponseMessage GetAuthToken(BasicAuthenticationIdentity basicAuthenticationIdentity)
        {
            var userId = basicAuthenticationIdentity.UserId;

            TokenServices tokenService = new TokenServices();
            var token = tokenService.GenerateToken(userId);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token", token.AuthToken);
            response.Headers.Add("UserId", userId.ToString());
            response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
            return response;
        }


    }
}
