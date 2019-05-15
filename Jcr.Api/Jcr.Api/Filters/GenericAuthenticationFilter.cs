using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jcr.Api.Helpers;

namespace Jcr.Api.Filters
{
    /// <summary>
    /// Generic basic Authentication filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class GenericAuthenticationFilter : AuthorizationFilterAttribute
    {
      
        /// <summary>
        /// Public default Constructor
        /// </summary>
        public GenericAuthenticationFilter()
        {
        }

        private readonly bool _isActive = true;

        /// <summary>
        /// parameter isActive explicitly enables/disables this filter.
        /// </summary>
        /// <param name="isActive"></param>
        public GenericAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// Checks basic authentication request
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!_isActive) return;
            var identity = FetchAuthHeader(filterContext);
            if (identity == null)
            {
                ChallengeAuthRequest(filterContext, "");
                return;
            }
            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;
            string invalidMsg;
            if (!OnAuthorizeUser(identity,  filterContext, out invalidMsg))
            {
                ChallengeAuthRequest(filterContext, invalidMsg);
                return;
            }
            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Virtual method.Can be overriden with the custom Authorization.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual bool OnAuthorizeUser(BasicAuthenticationIdentity identity, HttpActionContext filterContext, out string invalidMsg)
        {
            invalidMsg = string.Empty;

            if (string.IsNullOrEmpty(identity.Name) || string.IsNullOrEmpty(identity.Password))
                return false;

            return true;
        }

        /// <summary>
        /// Checks for autrhorization header in the request and parses it, creates user credentials and returns as BasicAuthenticationIdentity
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string authHeaderValue = null;
            var authRequest = filterContext.Request.Headers.Authorization;
            if (authRequest != null && !String.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Basic")
                authHeaderValue = authRequest.Parameter;
            if (string.IsNullOrEmpty(authHeaderValue))
                return null;
            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
            var credentials = authHeaderValue.Split(':');

            var isGuestUser = false;
            if (authHeaderValue.Contains("guestuser"))
                isGuestUser = true;
            //Encoding password to match with Database password
            // var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials[1]);
            // password =  System.Convert.ToBase64String(credentials[1]);
            var password = string.Empty;
            if (isGuestUser)
                password = credentials[1].Trim();
            else
                password = CryptHelpers.Encrypt(credentials[1].Trim(), "jcr");

            int? subscriptionTypeId=null;
            int tempInt = 0;
            if(credentials.Length>2 && int.TryParse(credentials[2].Trim(), out tempInt))
                subscriptionTypeId = Convert.ToInt32(credentials[2].Trim());
            string errorMessage=string.Empty;

            return credentials.Length < 2 ? null : new BasicAuthenticationIdentity(credentials[0], password, subscriptionTypeId, errorMessage, isGuestUser);
        }


        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="filterContext"></param>
        private static void ChallengeAuthRequest(HttpActionContext filterContext, string errorMessage)
        {
            var dnsHost = filterContext.Request.RequestUri.DnsSafeHost;
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            filterContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));           
            filterContext.Response.Headers.Add("UnauthorizedMessage", errorMessage);
        }
    }
}