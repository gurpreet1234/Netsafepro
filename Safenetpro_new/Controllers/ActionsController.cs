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
    public class Uniq_DisplayName_Actions_Poco
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }
    public class ActionsController : ApiController
    {
        public IEnumerable<Uniq_DisplayName_Actions_Poco> GetActions(int ruleTypeId)
        {
            using (var spContext = new netsEntities())
            {
                string sqlQuery = "SELECT UC.Id,UC.[DisplayName],Actions.Id ActionId, Actions.[Action] ActionName "
                                + " FROM (UniqDisplayName UC INNER JOIN Display_Category ON UC.ID = Display_Category.DisplayId) "
                                + " INNER JOIN CategoryToRule On Display_Category.RuleTypeId = CategoryToRule.RuleTypeId "
                                + " INNER JOIN AllRules_W_ID ON CategoryToRule.AllRule_ID = AllRules_W_ID.ID "
                                + " INNER JOIN Actions ON Actions.ID = AllRules_W_ID.ActionId "
                                + " AND (Display_Category.CategoryId = CategoryToRule.CategoryId) "
                                + "  Where Actions.RuleTypeId=" + ruleTypeId
                                + " GROUP BY UC.[DisplayName],UC.Id,Actions.Id, Actions.[Action]";

                return spContext.Database.SqlQuery<Uniq_DisplayName_Actions_Poco>(sqlQuery).ToList();
            }
        }
    }
}
