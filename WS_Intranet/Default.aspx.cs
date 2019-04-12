using System;
using System.Linq;

namespace WS_Intranet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Swagger", false);

            //if (Request.Cookies["Modo"] != null)
            //{
            //    var modo = Request.Cookies["Modo"].Value.ToString();
            //    if (modo == "TributarioOnline")
            //    {
            //        Response.Redirect("http://www.google.com");
            //    }
            //}
        }
    }
}