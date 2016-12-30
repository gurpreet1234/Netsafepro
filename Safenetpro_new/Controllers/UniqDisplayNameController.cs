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
    public class Uniq_Display_Category_Poco
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public int CategoryId { get; set; }
        public int Section { get; set; }
        public int DisplayId { get; set; }
        public bool Display { get; set; }
        public int RuelTypeId { get; set; }
        public string CategoryName { get; set; }
    }

    public class UniqDisplayNameController : ApiController
    {
        public IEnumerable<Uniq_Display_Category_Poco> GetUniqDisplayName(int ruleTypeId)
        {
            using (var spContext = new netsEntities())
            {
                string sqlQuery = "Select U.*,Section,DisplayId,Display,DC.RuleTypeId, "
                            + " (Select CategoryName + ', ' AS 'data()' from Category C Inner Join Display_Category DCC on C.CategoryId=DCC.CategoryId "
                                        + " Where DCC.DisplayId=U.ID And DCC.RuleTypeId=1 Group By CategoryName FOR XML PATH('')) CategoryName, "
                            + " (Select Top 1.C.ID from Category C Inner Join Display_Category DCC on C.CategoryId=DCC.CategoryId  "
                                        + " Where DCC.DisplayId=U.ID And DCC.RuleTypeId=1) CategoryId "
                            + " from UniqDisplayName U "
                            + " Inner Join Display_Category DC on U.ID=DC.DisplayId "
                            + " Inner Join Category C on C.ID=DC.CategoryId "
                            + " Where Display=1 And DC.RuletypeId=" + ruleTypeId
                            + " Group By U.Id,U.[DisplayName],Section,DisplayId,Display,DC.RuleTypeId "
                            + " Order By CategoryId";

                return spContext.Database.SqlQuery<Uniq_Display_Category_Poco>(sqlQuery).ToList();
            }
        }
    }
}
