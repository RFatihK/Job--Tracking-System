using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Freelancer2.Admin
{
    public partial class UserList : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlConnection con;
        DataTable dt;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["admin"] == null)
            {

                Response.Redirect("../User/Login.aspx");
            }
            if (!IsPostBack)
            {
                ShowUsers();
            }
        }
        private void ShowUsers()
        {
            string query = string.Empty;
            con = new SqlConnection(str);
            query = @"SELECT ROW_NUMBER() OVER (ORDER BY UserId) AS [Sr.No], UserId, Name, Email, Mobile, Country FROM Kullanıcılar";
            cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int userId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                con = new SqlConnection(str);
                cmd = new SqlCommand("Delete from Kullanıcılar where UserId=@id", con);
                cmd.Parameters.AddWithValue("id", userId);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    lblMsg.Text = "userId deleted succesfully!";
                    lblMsg.CssClass = "alert alert-success";
                }
                else
                {
                    lblMsg.Text = "Cannot delete this record !";
                    lblMsg.CssClass = "alert alert-danger";
                }
                con.Close();
                GridView1.EditIndex = -1;
                ShowUsers();
            }
            catch (Exception ex)
            {
                con.Close();
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }

        protected void GridView1_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowUsers();
        }
    }
}