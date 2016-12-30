using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SafenetproAPI.Controllers
{
    public class RuleTypesController : ApiController
    {
        public IEnumerable<Rule_Types> GetRuleTypes()
        {
            using (var spContext = new netsEntities())
            {
                return spContext.Rule_Types.ToList();
            }
        }
    }
}
