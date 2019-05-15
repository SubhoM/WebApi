using System.Security.Principal;

namespace Jcr.Api.Filters
{
    /// <summary>
    /// Basic Authentication identity
    /// </summary>
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        /// <summary>
        /// Get/Set for password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Get/Set for UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Get/Set for UserId
        /// </summary>
        public int UserId { get; set; }

        public int? SubscriptionTypeId { get; set; }

        public bool IsGuestUser { get; set; }

        public string ErrorMessage{ get; set; }

        /// <summary>
        /// Basic Authentication Identity Constructor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public BasicAuthenticationIdentity(string userName, string password, int? subscriptionTypeId, string errorMessage, bool isGuestUser = false)
            : base(userName, "Basic")
        {
            Password = password;
            UserName = userName;
            SubscriptionTypeId = subscriptionTypeId;
            ErrorMessage = errorMessage;
            IsGuestUser = isGuestUser;
        }
    }
}