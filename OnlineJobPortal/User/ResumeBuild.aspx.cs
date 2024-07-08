using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace OnlineJobPortal.User
{
    public partial class ResumeBuild : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                if(Request.QueryString["id"] != null)
                {
                    ShowUserInfo();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void ShowUserInfo()
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    string query = "select * from [User] where UserId=@UserId";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);

                    Con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            txtUserName.Text = sdr["Username"].ToString();
                            txtFullName.Text = sdr["Name"].ToString();
                            txtEmail.Text = sdr["Email"].ToString();
                            txtMobile.Text = sdr["Mobile"].ToString();
                            txtTenth.Text = sdr["TenthGrade"].ToString();
                            txtTwelth.Text = sdr["TwelthGrade"].ToString();
                            txtGraduation.Text = sdr["GraduationGrade"].ToString();
                            txtPostGraduation.Text = sdr["PostGraduationGrade"].ToString();
                            txtPHD.Text = sdr["PHD"].ToString();
                            txtWork.Text = sdr["WorksOn"].ToString();
                            txtExperience.Text = sdr["Experience"].ToString();
                            txtAddress.Text = sdr["Address"].ToString();
                            ddlCountry.SelectedValue = sdr["Country"].ToString();
                        }
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "User not found!";
                        lblMessage.CssClass = "alert alert-danger";
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                string ConcatQuery = string.Empty ; string FilePath = string.Empty;
                bool IsValid = false;
                try
                {
                    if (Request.QueryString["id"] != null)
                    {
                        if (fuResume.HasFile)
                        {
                            if (Utils.IsValidExtentionResume(fuResume.FileName))
                            {
                                ConcatQuery = "Resume=@Resume,";
                                IsValid = true;
                            }
                            else
                            {
                                ConcatQuery = string.Empty;
                            }
                        }
                        else
                        {
                            ConcatQuery = string.Empty;
                        }
                        string query = @"Update [User] set Username=@Username, Name=@Name, Email=@Email, Mobile=@Mobile, TenthGrade=@TenthGrade, TwelthGrade=@TwelthGrade,
                                    GraduationGrade=@GraduationGrade, PostGraduationGrade=@PostGraduationGrade, PHD=@PHD, WorksOn=@WorksOn, Experience=@Experience,"
                                    + ConcatQuery + @"Address=@Address, Country=@Country where UserId=@UserId";

                        SqlCommand cmd = new SqlCommand(query, Con);
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenthGrade", txtTenth.Text.Trim());
                        cmd.Parameters.AddWithValue("@TwelthGrade", txtTwelth.Text.Trim());
                        cmd.Parameters.AddWithValue("@GraduationGrade", txtGraduation.Text.Trim());
                        cmd.Parameters.AddWithValue("@PostGraduationGrade", txtPostGraduation.Text.Trim());
                        cmd.Parameters.AddWithValue("@PHD", txtPHD.Text.Trim());
                        cmd.Parameters.AddWithValue("@WorksOn", txtWork.Text.Trim());
                        cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                        cmd.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);

                        if (fuResume.HasFile)
                        {
                            if (Utils.IsValidExtentionResume(fuResume.FileName))
                            {
                                Guid obj = Guid.NewGuid();
                                FilePath = "Resumes/" + obj.ToString() + fuResume.FileName;
                                fuResume.PostedFile.SaveAs(Server.MapPath("~/Resumes/" + obj.ToString() + fuResume.FileName));
                                cmd.Parameters.AddWithValue("@Resume", FilePath);
                                IsValid = true;
                            }
                            else
                            {
                                ConcatQuery = string.Empty;
                                lblMessage.Visible = true;
                                lblMessage.Text = "Please Select .doc, .docx or .pdf file for resume!";
                                lblMessage.CssClass = "alert alert-danger";
                            }
                        }
                        else
                        {
                            IsValid = true;
                        }

                        if (IsValid)
                        {
                            Con.Open();
                            int r = cmd.ExecuteNonQuery();

                            if(r > 0)
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Resume details updated successfully..!";
                                lblMessage.CssClass = "alert alert-success";
                            }
                            else
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Cannot update the records, please try after some time..!";
                                lblMessage.CssClass = "alert alert-danger";
                            }
                        }
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Cannot update the records, please try <b>Relogin</b>!";
                        lblMessage.CssClass = "alert alert-danger";
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "<b>" + txtUserName.Text.Trim() + "</b>" + " Username already exist, try new one..!";
                        lblMessage.CssClass = "alert alert-danger";
                    }
                    else
                    {
                        Response.Write("<script>alert('" + ex.Message + "')</script>");
                    }
                }
                catch(Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
                    
            
        }
    }
}