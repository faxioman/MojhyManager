using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _default : System.Web.UI.Page
{
	protected System.Web.UI.WebControls.Content main;

	protected System.Web.UI.WebControls.Content menu;

    protected void Page_Load(object sender, EventArgs e)
    {
        //registro la classe per Ajax
        AjaxPro.Utility.RegisterTypeForAjax(typeof(_default));
    }
    [AjaxPro.AjaxMethod]
    public string GetServerTime()
    {
        return "Ora server: " + DateTime.Now.ToString();
    }
}
