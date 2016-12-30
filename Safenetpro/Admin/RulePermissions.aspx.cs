using AjaxControlToolkit;
using SafenetproAPI.Controllers;
using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Safenetpro
{
    public partial class RulePermissions : System.Web.UI.Page
    {
        HtmlGenericControl mainDivControl = new HtmlGenericControl("div");
        HtmlGenericControl firstChildDivControl = new HtmlGenericControl("div");
        HtmlGenericControl childDivControl = new HtmlGenericControl("div");


        Table htable = new Table();
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["adminUserId"] == null)
                Response.Redirect("Login.aspx");
            else
            {
                BindRuleTypes();
                if (!Page.IsPostBack)
                {
                    BindCustomers();
                    BindTabData(1, 0);
                }
            }
        }

        private void BindRuleTypes()
        {
            RuleTypesController ruleTypes = new RuleTypesController();
            IEnumerable<Rule_Types> rr = ruleTypes.GetRuleTypes();

            foreach (var r in rr)
            {
                TabPanel tbPanel = new TabPanel();
                tbPanel.HeaderText = r.RuleType;

                this.TabContainer1.Controls.Add(tbPanel);
            }
        }

        private void BindTabData(int ruleTypeId, int tabIndex, bool bindMasterGroupActions = true)
        {
            htable.CssClass = "tableTabPanel";
            UniqDisplayNameController uNameController = new UniqDisplayNameController();
            IEnumerable<Uniq_Display_Category_Poco> unDisplay = uNameController.GetUniqDisplayName(ruleTypeId);

            ActionsController actionsController = new ActionsController();
            IEnumerable<Uniq_DisplayName_Actions_Poco> unActions = actionsController.GetActions(ruleTypeId); //ruletype Id

            List<int> sectionList = unDisplay.OrderBy(s => s.Section).Select(u => u.Section).Distinct().ToList();

            foreach (var s in sectionList)
            {
                IEnumerable<Uniq_Display_Category_Poco> unDisplayNames = unDisplay.Where(u => u.Section == s);
                DisplayDataTemp(unDisplayNames, unActions, s, ruleTypeId, tabIndex, bindMasterGroupActions);
            }
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer1.ActiveTabIndex == 0)
            {
                //ddlMasterGroups.SelectedValue = "0";
                BindTabData(1, TabContainer1.ActiveTabIndex); //Rule TypeId
            }
        }
        private void DisplayDataTemp(IEnumerable<Uniq_Display_Category_Poco> unDisplayNames, IEnumerable<Uniq_DisplayName_Actions_Poco> unActions, int sectionid, int ruleTypeId, int tabIndex, bool bindMasterGroupActions)
        {
            mainDivControl = new HtmlGenericControl("div");
            mainDivControl.Attributes.Add("class", "mainDiv");

            childDivControl = new HtmlGenericControl("div");
            //List<int> displayId = new List<int>();
            foreach (var u in unDisplayNames)
            {
                firstChildDivControl = new HtmlGenericControl("div");
                firstChildDivControl.Attributes.Add("class", "Innerdiv");

                childDivControl = new HtmlGenericControl("div");
                childDivControl.Attributes.Add("class", "firstDiv");
                Label lblTemp = new Label();
                lblTemp.ID = u.CategoryId.ToString() + u.DisplayName.ToString();
                lblTemp.Text = u.DisplayName + "  ";
                childDivControl.Controls.Add(lblTemp);

                Image imgTooltip = new Image();
                imgTooltip.ImageUrl = "../img/tooltip_icon.gif";
                imgTooltip.ToolTip = u.CategoryName.Substring(0, u.CategoryName.Length - 2);
                childDivControl.Controls.Add(imgTooltip);

                firstChildDivControl.Controls.Add(childDivControl);

                childDivControl = new HtmlGenericControl("div");
                DropDownList ddlTemp = new DropDownList();

                ddlTemp.Attributes.Add("onChange", "displayActionChanged(" + u.ID + ")");

                ddlTemp.ID = u.ID.ToString();
                ddlTemp.CssClass = "tableDropDown";

                foreach (var a in unActions.Where(a => a.Id == u.ID))
                {
                    ListItem lI = new ListItem();
                    lI.Text = a.ActionName;
                    lI.Value = a.ActionId.ToString();
                    ddlTemp.Items.Add(lI);
                }

                childDivControl.Controls.Add(ddlTemp);

                firstChildDivControl.Controls.Add(childDivControl);

                mainDivControl.Controls.Add(firstChildDivControl);
                //displayId.Add(u.ID);
            }

            Panel panelHead = new Panel();
            panelHead.ID = "pH" + sectionid.ToString();
            panelHead.CssClass = "cpHeader";
            // Add Label inside header panel to display text
            Label lblHead = new Label();
            lblHead.ID = "lblHeader" + sectionid.ToString();
            lblHead.CssClass = "extenderLabel";

            ImageButton imgBtnHead = new ImageButton();
            imgBtnHead.ID = "imgbtn" + sectionid.ToString();
            imgBtnHead.ImageUrl = "../img/expand_blue.jpg";
            imgBtnHead.CssClass = "extenderImage";

            panelHead.Controls.Add(imgBtnHead);
            panelHead.Controls.Add(lblHead);

            //Create Body Panel
            Panel panelBody = new Panel();
            panelBody.ID = "pB" + sectionid.ToString();
            panelBody.CssClass = "cpBody";
            // Add Label inside body Panel to display text
            panelBody.Controls.Add(mainDivControl);

            CollapsiblePanelExtender cpe =
            new CollapsiblePanelExtender();

            cpe.ID = "cpExtender" + sectionid.ToString();
            cpe.TargetControlID = panelBody.ID;
            cpe.ExpandControlID = panelHead.ID;
            cpe.CollapseControlID = panelHead.ID;
            cpe.ScrollContents = false;
            cpe.Collapsed = false;
            cpe.ExpandDirection =
            CollapsiblePanelExpandDirection.Vertical;
            cpe.SuppressPostBack = true;
            cpe.TextLabelID = lblHead.ID;
            cpe.ImageControlID = imgBtnHead.ID;

            cpe.CollapsedText = "Click to Show Content..";
            cpe.ExpandedText = "Click to Hide Content..";

            cpe.ExpandedImage = "../img/collapse_blue.jpg";
            cpe.CollapsedImage = "../img/expand_blue.jpg";

            TabContainer1.Tabs[tabIndex].Controls.Add(panelHead);
            TabContainer1.Tabs[tabIndex].Controls.Add(panelBody);
            TabContainer1.Tabs[tabIndex].Controls.Add(cpe);

            if (bindMasterGroupActions)
                BindMasterGroupActions();
        }

        private void BindMasterGroups(int customerMasterGroup)
        {
            GroupsController ruleTypes = new GroupsController();
            IEnumerable<Group> mgroups = ruleTypes.GetMasterGroups();
            ddlMasterGroups.DataSource = mgroups;
            ddlMasterGroups.DataTextField = "GroupName";
            ddlMasterGroups.DataValueField = "ID";
            ddlMasterGroups.DataBind();

            ddlMasterGroups.SelectedValue = Convert.ToString(customerMasterGroup);
            //ddlMasterGroups.Items.Insert(0, new ListItem() { Text = "Select Master Group", Value = "0", Selected = true });
        }

        private void BindCustomers()
        {
            CustomersController _customersController = new CustomersController();
            IEnumerable<RuleUserProfile_Poco> customers = _customersController.GetCustomers();
            ddlCustomers.DataSource = customers;
            ddlCustomers.DataTextField = "Name";
            ddlCustomers.DataValueField = "Id";
            ddlCustomers.DataBind();

            int customerMasterGroup = _customersController.GetSelectedCustomerMasterGroup(Convert.ToInt32(ddlCustomers.SelectedValue));


            BindMasterGroups(customerMasterGroup);
        }

        protected void ddlMasterGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabData(1, TabContainer1.ActiveTabIndex); //Rule TypeId
            //BindMasterGroupActions();
        }

        public void BindMasterGroupActions()
        {
            GroupsController ruleTypes = new GroupsController();
            IEnumerable<GroupAction_Poco> mgroupActions = ruleTypes.GetGroupActions(1, Convert.ToInt32(ddlMasterGroups.SelectedValue), Convert.ToInt32(ddlCustomers.SelectedValue)); //Rule TypeId
            foreach (var ga in mgroupActions)
            {
                bool selectOption = true;
                if (ga.GroupType == "Master")
                {
                    if (mgroupActions.Where(g => g.DisplayName == ga.DisplayName && g.GroupType != "Master").Count() > 0)
                        selectOption = false;
                }
                if (selectOption)
                {
                    DropDownList ddlG = (DropDownList)TabContainer1.Tabs[TabContainer1.ActiveTabIndex].FindControl(Convert.ToString(ga.ID));
                    if (ddlG != null)
                        ddlG.SelectedValue = Convert.ToString(ga.ActionId);
                }
            }

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "removeAllDisplayedActions();", true);
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabData(1, TabContainer1.ActiveTabIndex, false); //Rule TypeId
            CustomersController _customersController = new CustomersController();
            int customerMasterGroup = _customersController.GetSelectedCustomerMasterGroup(Convert.ToInt32(ddlCustomers.SelectedValue));
            if (customerMasterGroup > 0)
                ddlMasterGroups.SelectedValue = Convert.ToString(customerMasterGroup);

            BindMasterGroupActions();
        }
    }
}