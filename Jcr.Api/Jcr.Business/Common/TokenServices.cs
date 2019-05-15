using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jcr.Data;
using System.Configuration;

namespace Jcr.Business
{
    public class TokenServices
    {
        SiteServices siteServices = new SiteServices();
        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Token GenerateToken(int userId)
        {

            var db = new DBAMPContext();
            Token token;
            string AuthToken;
            DateTime issuedOn;
            DateTime expiredOn;

            bool isValidateToken = false;
            token = db.Tokens.Where(t => t.UserId == userId).FirstOrDefault<Token>();

            if (token != null)
            {
                isValidateToken = ValidateToken(token.AuthToken, userId.ToString());
            }


            if (!isValidateToken)
            {
                AuthToken = Guid.NewGuid().ToString();
                issuedOn = DateTime.Now;
                expiredOn = DateTime.Now.AddSeconds(
                                                  Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                var tokendomain = new Token
                {
                    UserId = userId,
                    AuthToken = AuthToken,
                    IssuedOn = issuedOn,
                    ExpiresOn = expiredOn
                };
                int rtn = db.ApiTokenInsert(userId, AuthToken, issuedOn, expiredOn);
            }

            else
            {
                issuedOn = token.IssuedOn;
                expiredOn = token.ExpiresOn;
                AuthToken = token.AuthToken;
            }

            var tokenModel = new Token()
            {
                UserId = userId,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                AuthToken = AuthToken
            };

            return tokenModel;



        }

        public bool ValidateUserTracerAccess(int userId, int tracerCustomID)
        {
            var siteID = siteServices.GetSiteIDByTracerCustomID(tracerCustomID);

            return ValidateUserSiteAccess(userId, siteID);
        }

        public bool ValidateUserSiteAccess(int userId, int siteId)
        {
            var sites = SiteServices.GetUserSites(userId).Where(m=> m.SiteID == siteId).ToList();
            
            return (sites != null && sites.Count > 0);
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenValue, string userId)
        {
            Token token;
            var db = new DBAMPContext();
            int Id = Convert.ToInt32(userId);
            token = db.Tokens.Where(t => t.AuthToken == tokenValue && t.UserId == Id).FirstOrDefault<Token>();

            if (token != null)
            {
                if (!(DateTime.Now > token.ExpiresOn))  //if validated token, extend Expiry Time
                {
                    token.ExpiresOn = token.ExpiresOn.AddSeconds(
                                            Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

                    int rtn = db.ApiTokenUpdate(token.UserId, token.AuthToken, token.ExpiresOn);

                    return true;
                }
                else  //If expired token, delete it from DB
                {
                    //int rtn = db.ApiTokenDelete(token.UserId, token.AuthToken);
                    KillToken(token);
                    return false;
                }
            }
            return false;
        }


        public void KillToken(Token token)
        {
            ExceptionLogServices exceptionLog = new ExceptionLogServices();
            using (var db = new DBAMPContext())
            {


                try
                {
                    int rtn = db.ApiTokenDelete(token.UserId, token.AuthToken);
                }
                catch (Exception ex)
                {

                    string sqlParam = "ApiTokenDelete(" + token.UserId + "," + token.AuthToken + ")";
                    string methodName = "JCRAPI/Business/TokenService/KillToken";
                    exceptionLog.ExceptionLogInsert(ex.Message.ToString(), "", methodName, token.UserId, null, sqlParam, string.Empty);

                }

            }


        }
        public bool CheckUserToken(int userId)
        {
            Token token;
            var db = new DBAMPContext();
            int Id = Convert.ToInt32(userId);
            token = db.Tokens.Where(t => t.UserId == Id).FirstOrDefault<Token>();
            bool rtn = false;

            if (token != null)
            {
                if (!(DateTime.Now > token.ExpiresOn))  //if validated token, extend Expiry Time
                {
                    rtn = true;
                }
                else  //If expired token, delete it from DB
                {
                    //int rtn = db.ApiTokenDelete(token.UserId, token.AuthToken);
                    KillToken(token);
                    rtn = false;
                }
            }
            return rtn;
        }

        
    }
}
