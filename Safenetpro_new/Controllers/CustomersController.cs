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
    public class DisplayNameUserActions
    {
        public int ID { get; set; }
        public int ActionID { get; set; }
    }

    public class RuleUserProfile_Poco
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CustomersController : ApiController
    {
        [Route("~/api/CustomersList")]
        public IEnumerable<RuleUserProfile_Poco> GetCustomers()
        {
            using (var spContext = new netsEntities())
            {
                return spContext.UserProfiles.Select(u => new RuleUserProfile_Poco { Id = u.UserId, Name = u.FirstName + " " + u.LastName }).ToList();
            }
        }
        public int GetSelectedCustomerMasterGroup(int customerId)
        {
            using (var spContext = new netsEntities())
            {
                string sqlQuery = "Select GroupId from CustomerGroups Where CustomerId=" + customerId +
                                " And GroupId In(Select ID from Groups Where GroupType='Master')";

                return spContext.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
            }
        }
        public IEnumerable<CustomerGroup> GetCustomerGroups(int customerId)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.CustomerGroups.Where(g => g.CustomerId == customerId).ToList();
            }
        }

        [Route("~/api/Customer/{CustomerId}/RuleTypes/{ruleTypeId}/DisplayNameActions")]
        public HttpResponseMessage PostActions(int CustomerId, int ruleTypeId, IEnumerable<DisplayNameUserActions> displayNameUserActions)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    foreach (var da in displayNameUserActions)
                    {
                        string sqlQuery = "SELECT top 1.G.ID "
                                    + " FROM (((Groups G INNER JOIN GroupToRule_W_ID GTR ON G.ID = GTR.GroupId) "
                                    + " INNER JOIN CategoryToRule CTR ON GTR.AllRule_ID = CTR.AllRule_ID) "
                                    + " INNER JOIN AllRules_W_ID AR ON (GTR.AllRule_ID = AR.ID) AND (CTR.AllRule_ID = AR.ID)) "
                                    + " INNER JOIN (UniqDisplayName UC INNER JOIN Display_Category DC ON UC.ID = DC.DisplayId) "
                                    + " ON CTR.CategoryId = DC.CategoryId "
                                    + " GROUP BY G.GroupName,G.ID, G.GroupType, GTR.RuleTypeId, AR.ActionId,UC.ID, UC.[DisplayName] "
                                    + " HAVING GTR.RuleTypeId=" + ruleTypeId + " AND AR.ActionId=" + da.ActionID + " AND UC.ID=" + da.ID + " And GroupType='Regular'";

                        int groupId = spContext.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
                        CustomerGroup CG = spContext.CustomerGroups.Where(c => c.GroupId == groupId && c.CustomerId == CustomerId).FirstOrDefault();

                        if (CG != null)
                            spContext.CustomerGroups.Remove(CG);

                        string nQuery = "SELECT CG.GroupId "
                                        + " FROM Groups G INNER JOIN (UniqDisplayName UC INNER JOIN (Users C INNER JOIN (CustomerGroups CG INNER JOIN (Display_Category DC  "
                                        + " INNER JOIN (CategoryToRule CR INNER JOIN (GroupToRule_W_ID GR INNER JOIN AllRules_W_ID AR ON GR.AllRule_ID = AR.ID) "
                                        + " ON CR.AllRule_ID = AR.ID) ON DC.CategoryId = CR.CategoryId) "
                                        + " ON CG.GroupID = GR.GroupId) ON C.ID = CG.CustomerID) "
                                        + " ON UC.ID = DC.DisplayId) ON G.ID = GR.GroupId "
                                        + " GROUP BY UC.ID, UC.DisplayName, AR.ActionId, G.GroupType, AR.RuleTypeId,G.GroupName,CG.GroupId,C.ID "
                                        + " HAVING AR.RuleTypeId=" + ruleTypeId + " And C.ID=" + CustomerId + " And UC.ID=" + da.ID + " AND G.GroupType='Regular' "
                                        + " Order By DisplayName";
                        int ngroupId = spContext.Database.SqlQuery<int>(nQuery).FirstOrDefault();

                        CustomerGroup CGR = spContext.CustomerGroups.Where(c => c.GroupId == ngroupId && c.CustomerId == CustomerId).FirstOrDefault();

                        if (CGR != null)
                            spContext.CustomerGroups.Remove(CGR);

                        if (groupId > 0)
                        {
                            CustomerGroup CGA = new CustomerGroup();
                            CGA.GroupId = groupId;
                            CGA.CustomerId = CustomerId;
                            spContext.CustomerGroups.Add(CGA);
                        }
                    }
                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                var response = this.Request.CreateResponse(HttpStatusCode.Created, displayNameUserActions);
                return response;
            }
        }

    }
}
