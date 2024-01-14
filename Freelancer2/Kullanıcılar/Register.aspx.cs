using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Freelancer2.Kullanıcılar
{
    public partial class Register : System.Web.UI.Page
    {

        SqlConnection con;
        SqlCommand cmd;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(str);
                string query = @"Insert Into Kullanıcılar (Username,Password,Name,Adress,Mobile,Email,Country) values (@Username,@Password,@Name,@Adress,@Mobile,@Email,@Country)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());               
                cmd.Parameters.AddWithValue("@Password", txtConfirmPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Adress", txtAdress.Text.Trim());
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);               
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Register successfull!";
                    lblMsg.CssClass = "alert alert-success";
                    clear();

                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Cannot save record right now!,please try after sometime..";
                    lblMsg.CssClass = "alert alert-danger";

                }

            }
            catch (SqlException ex)
            { 
                if(ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtUserName.Text.Trim()+"</b> username already exist,try new one..!";
                    lblMsg.CssClass = "alert alert-danger";

                }
                else
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
        }

        private void clear()
        {
            txtUserName.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtAdress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            ddlCountry.ClearSelection();
        }
    }
}