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
    public class GroupAction_Poco
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public int ActionId { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
        public int RuleTypeId { get; set; }
        public int GroupId { get; set; }
    }

    public class GroupsController : ApiController
    {
        public IEnumerable<Group> GetMasterGroups()
        {
            using (var spContext = new netsEntities())
            {
                return spContext.Groups.Where(g => g.GroupType == "Master").ToList();
            }
        }
        public IEnumerable<GroupAction_Poco> GetGroupActions(int ruleTypeId, int groupId, int customerId)
        {
            using (var spContext = new netsEntities())
            {

                string sqlFirstQuery = "";
                string sqlQuery = "";
                //if ((from g in spContext.Groups
                //     join cg in spContext.CustomerGroups on g.ID equals cg.GroupId
                //     where cg.CustomerId == customerId && g.ID == groupId
                //     select cg).Count() <= 0)
                //{
                sqlFirstQuery = "SELECT UC.ID,UC.[DisplayName], AllRules_W_ID.ActionId, Groups.GroupName, Groups.[GroupType], GroupToRule_W_ID.RuleTypeId,Groups.ID GroupId "
                                    + " FROM (Groups INNER JOIN GroupToRule_W_ID ON Groups.ID = GroupToRule_W_ID.GroupId) "
                                    + " INNER JOIN (((UniqDisplayName UC INNER JOIN Display_Category ON UC.ID = Display_Category.DisplayId) "
                                    + " INNER JOIN CategoryToRule ON Display_Category.CategoryId = CategoryToRule.CategoryId) "
                                    + " INNER JOIN AllRules_W_ID ON CategoryToRule.AllRule_ID = AllRules_W_ID.ID) ON GroupToRule_W_ID.AllRule_Id = AllRules_W_ID.ID "
                                    + " GROUP BY UC.[DisplayName], AllRules_W_ID.ActionId, Groups.GroupName, Groups.[GroupType], GroupToRule_W_ID.RuleTypeId,Groups.ID,UC.ID "
                                    + " HAVING (((Groups.ID)=" + groupId + ") AND ((Groups.[GroupType])='master') AND ((GroupToRule_W_ID.RuleTypeId)=" + ruleTypeId + "))";
                //}
                //else
                //{
                sqlQuery = "SELECT UC.ID, UC.DisplayName, AR.ActionId,G.GroupName,G.GroupType, AR.RuleTypeId,CG.GroupId "
                            + " FROM Groups G INNER JOIN (UniqDisplayName UC INNER JOIN (Users C INNER JOIN (CustomerGroups CG INNER JOIN (Display_Category DC  "
                            + " INNER JOIN (CategoryToRule CR INNER JOIN (GroupToRule_W_ID GR INNER JOIN AllRules_W_ID AR ON GR.AllRule_ID = AR.ID) "
                            + " ON CR.AllRule_ID = AR.ID) ON DC.CategoryId = CR.CategoryId) "
                            + " ON CG.GroupID = GR.GroupId) ON C.ID = CG.CustomerID) "
                            + " ON UC.ID = DC.DisplayId) ON G.ID = GR.GroupId "
                            + " GROUP BY UC.ID, UC.DisplayName, AR.ActionId, G.GroupType, AR.RuleTypeId,G.GroupName,CG.GroupId,C.ID "
                            + " HAVING AR.RuleTypeId=" + ruleTypeId + " And C.ID=" + customerId + " And G.GroupType='Regular' "
                            + " Order By DisplayName";
                //}
                List<GroupAction_Poco> sqlFirstResults =spContext.Database.SqlQuery<GroupAction_Poco>(sqlFirstQuery).ToList();

                List<GroupAction_Poco> sqlResults = spContext.Database.SqlQuery<GroupAction_Poco>(sqlQuery).ToList();

                return sqlResults.Union(sqlFirstResults);
            }
        }
    }
}
