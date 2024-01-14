using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Freelancer2.Kullanıcılar
{
    public partial class ResumeBuild : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader sdr;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        string query;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    showUserInfo();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void showUserInfo()
        {
            try
            {
                con = new SqlConnection(str);
                string query = "Select * from Kullanıcılar where UserId=@UserId";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);
                con.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        txtUserName.Text = sdr["UserName"].ToString();
                        txtFullName.Text = sdr["Name"].ToString();
                        txtEmail.Text = sdr["Email"].ToString();
                        txtMobile.Text = sdr["Mobile"].ToString();
                        txtTenth.Text = sdr["TenthGrade"].ToString();
                        txtTwelfth.Text = sdr["TwelfthGrade"].ToString();
                        txtGraduation.Text = sdr["GraduationGrade"].ToString();
                        txtPostGraduation.Text = sdr["PostGraduationGrade"].ToString();
                        txtPhd.Text = sdr["Phd"].ToString();
                        txtWork.Text = sdr["WorksOn"].ToString();
                        txtExperience.Text = sdr["Experience"].ToString();
                        txtAdress.Text = sdr["Adress"].ToString();
                        ddlCountry.SelectedValue = sdr["Country"].ToString();
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "User not found!";
                    lblMsg.CssClass = "alert alert-danger";

                }
            }
            catch (Exception ex)
            {

                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
            finally
            {
                con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    string concatQuery = "", filePath = string.Empty;
                    //bool isValidToExecute = false;
                    bool isValid = false;
                    con = new SqlConnection(str);
                    if (fuResume.HasFile)
                    {
                        if (Utils.isValidExtension4Resume(fuResume.FileName))
                        {
                            concatQuery = "Resume=@Resume,";
                            isValid = true;
                        }
                        else
                        {
                            //concatQuery = string.Empty;
                            lblMsg.Visible=true;
                            lblMsg.Text = "Please select! .doc,.docx,.pdf file! for resume";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        concatQuery = string.Empty;
                    }
                    query = @"Update Kullanıcılar set Username=@Username,Name=@Name,Email=@Email,Mobile=@Mobile,TenthGrade=@TenthGrade,TwelfthGrade=@TwelfthGrade,GraduationGrade=@GraduationGrade,
                             PostGraduationGrade=@PostGraduationGrade,Phd=@Phd,WorksOn=@WorksOn,Experience=@Experience," + concatQuery + @"Adress=@Adress,Country=@Country where UserId=@UserId";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenthGrade", txtTenth.Text.Trim());
                    cmd.Parameters.AddWithValue("@TwelfthGrade", txtTwelfth.Text.Trim());
                    cmd.Parameters.AddWithValue("@GraduationGrade", txtGraduation.Text.Trim());
                    cmd.Parameters.AddWithValue("@PostGraduationGrade", txtPostGraduation.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phd", txtPhd.Text.Trim());
                    cmd.Parameters.AddWithValue("@WorksOn", txtWork.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@Adress", txtAdress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);
                    if (isValid)
                    {
                        Guid obj = Guid.NewGuid();
                        filePath = "Resumes/" + obj.ToString() + fuResume.FileName;
                        fuResume.PostedFile.SaveAs(Server.MapPath("~/Resumes/") + obj.ToString() + fuResume.FileName);
                        cmd.Parameters.AddWithValue("@Resume", filePath);
                        //isValidToExecute = true;
                    }
                    else
                    {
                        isValid = true;
                    }
                    if (isValid)
                    {
                        con.Open();
                        int r = cmd.ExecuteNonQuery();
                        if(r>0)
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Resume  details! updated successful!";
                            lblMsg.CssClass = "alert alert-success";
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Cannot updae the records ,please try after sometime..!";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Cannot update the record,please try <b>Relogin</b>!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch(SqlException ex)
            {
                if(ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtUserName.Text.Trim() + "</b> username already exist,try new one..!";
                    lblMsg.CssClass = "alert alert-danger";

                }
                else
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }

            }catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
        }
    }
}