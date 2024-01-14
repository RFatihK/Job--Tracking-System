using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Freelancer2.Kullanıcılar
{
    public partial class Job_Details : System.Web.UI.Page
    {
        SqlConnection con; SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        public string jobTitle = string.Empty;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                showJobDetails();
                DataBind();
            }
            else
            {
                Response.Redirect("Job_Listing.aspx");
            }
        }


        private void showJobDetails()
        {
            con = new SqlConnection(str);
            string query = @"Select * from Jobs where JobId = @id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id",Request.QueryString["id"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            DataList1.DataSource = dt;
            DataList1.DataBind();
  
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ApplyJob")
            {
                if (Session["user"] != null)
                {
                    try
                    {
                        con = new SqlConnection(str);
                        string query = @"INSERT INTO AppliedJobs values(@JobId,@UserId)";
                        cmd = new SqlCommand(query, con);                       
                        cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                        cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
                        con.Open();
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Job applied successfull!";
                            lblMsg.CssClass = "alert alert-success"; 
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Cannot apply the job, please try after sometime..!";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<scipt>alert('" + ex.Message + "');</scipt>");
                    }
                    finally
                    {
                        con.Close();
                    }


                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }


        }


        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (Session["user"] != null)
            {
                LinkButton btnApplyJob = new LinkButton();
                btnApplyJob = e.Item.FindControl("lbApplyJob") as LinkButton;
                if (isApplied())
                {
                    btnApplyJob.Enabled = false;
                    btnApplyJob.Text = "Applied";
                }
                else
                {

                    btnApplyJob.Enabled = true;
                    btnApplyJob.Text = "Apply Now";
                }
            }
        }
        bool isApplied()
        {
            con = new SqlConnection(str);
            string query = @"Select * from AppliedJobs where UserId = @UserId and JobId = @JobId";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        protected string GetImageUrl(Object url)
        {
            string url1 = "";
            if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "~/Images/No_image.png";
            }
            else
            {
                url1 = string.Format("~/{0}", url);
                
            }
            return ResolveUrl(url1);
        }


    }

}
    
