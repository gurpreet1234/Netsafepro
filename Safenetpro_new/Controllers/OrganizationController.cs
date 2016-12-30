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
    public class OrganizationController : ApiController
    {
        public IEnumerable<Organization> GetOrganizations()
        {
            using (var spContext = new netsEntities())
            {
                return spContext.Organizations.ToList();
            }
        }
    }
}
