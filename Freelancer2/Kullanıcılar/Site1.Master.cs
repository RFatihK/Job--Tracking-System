using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Freelancer2.Kullanıcılar
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                lblRegisterOrProfile.Text = "Profile";
                lblLoginOrLogout.Text = "Logout";
            }
            else
            {
                lblRegisterOrProfile.Text = "Register";
                lblLoginOrLogout.Text = "Login";
            }
            }
        protected void lblRegisterOrProfile_Click(object sender, EventArgs e)
        {
            if (lblRegisterOrProfile.Text == "Profile")
            {
                Response.Redirect("Profile.aspx");
            }
            else
            {
                Response.Redirect("Register.aspx");
            }
        }
        protected void lblLoginOrLogout_Click(object sender, EventArgs e)
        {
            if (lblLoginOrLogout.Text == "Login")
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                Session.Abandon();
                Response.Redirect("Login.aspx");
            }
        }
    }
}