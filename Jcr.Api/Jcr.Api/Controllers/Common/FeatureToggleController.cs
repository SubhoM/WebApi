using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jcr.Api.Controllers.Common
{
    public class FeatureToggleController : ApiController
    {
        // GET: api/FeatureToggle
        public bool Get()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["NewFeatureToggle"]);
        }


    }
}
