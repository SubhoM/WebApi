using System.Threading;
using System.Web.Http.Controllers;
using Jcr.Business;
using System.Web.Http.Filters;

namespace Jcr.Api.Filters
{
    /// <summary>
    /// Custom Authentication Filter Extending basic Authentication
    /// </summary>
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(BasicAuthenticationIdentity identity, HttpActionContext actionContext, out string invalidMsg)
        {
            UserServices userService = new UserServices();
            //  var provider = actionContext.ControllerContext.Configuration
            //     .DependencyResolver.GetService(typeof(IUserServices)) as IUserServices;
            // if (provider != null)
            // {

            var userId = 0;
            if (identity.IsGuestUser)
                userId = userService.AuthenticateGuest(identity.Name, identity.Password, out invalidMsg);
            else
                userId = userService.Authenticate(identity.Name, identity.Password, identity.SubscriptionTypeId, out invalidMsg);
            var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (invalidMsg.Length == 0)
            {

                if (basicAuthenticationIdentity != null)
                    basicAuthenticationIdentity.UserId = userId;
                return true;
            }
            else
            {
                
                basicAuthenticationIdentity.ErrorMessage = invalidMsg;
                return false;
            }
            
       
           
        }
    }
}