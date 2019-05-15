using Jcr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcr.Business.MockSurvey
{
    public class MSServices
    {
        public static List<ApiMsGetWorkFlowsReturnModel> GetMSWorkFlows()
        {

            var dbEntityContainer = new DBMEdition01Context();
            try
            {

                var result = dbEntityContainer.ApiMsGetWorkFlows().ToList();

                if (result.Count > 0)
                    return result;
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ApiMsGetAllCorpSettingsByUserIdReturnModel> GetAllCorpSettings(int? UserID)
        {

            var dbEntityContainer = new DBMEdition01Context();
            try
            {

                var result = dbEntityContainer.ApiMsGetAllCorpSettingsByUserId(UserID).ToList();

                if (result.Count > 0)
                    return result;
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateMSWorkFlow(int? mockSurveyWorkFlowId, int? userId, string siteIDs)
        {

            var dbEntityContainer = new DBMEdition01Context();
            try
            {

                dbEntityContainer.ApiMsUpdateWorkFlow(mockSurveyWorkFlowId, userId, siteIDs);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
