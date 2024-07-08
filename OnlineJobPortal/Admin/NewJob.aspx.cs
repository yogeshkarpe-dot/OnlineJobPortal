using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace OnlineJobPortal.Admin
{
    public partial class NewJob : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }
            Session["title"] = "Add Job";
            if (!IsPostBack)
            {
                FillData();
            }
        }

        private void FillData()
        {
            if(Request.QueryString["id"] != null)
            {
                using (SqlConnection Con = new SqlConnection(CS))
                {
                    string query = "select * from Jobs where JobId = '" + Request.QueryString["id"] + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    Con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            txtJobTitle.Text = sdr["Title"].ToString();
                            txtNoOfPositions.Text = sdr["NoOfPost"].ToString();
                            txtDescription.Text = sdr["Description"].ToString();
                            txtQualification.Text = sdr["Qualification"].ToString();
                            txtExperience.Text = sdr["Experience"].ToString();
                            txtSpecialization.Text = sdr["Specialisation"].ToString();
                            txtLastDate.Text = Convert.ToDateTime(sdr["LastDateToApply"]).ToString("yyyy-MM-dd");
                            txtSalary.Text = sdr["Salary"].ToString();
                            ddlJobType.SelectedValue = sdr["JobType"].ToString();
                            txtCompany.Text = sdr["CompanyName"].ToString();
                            txtWebsite.Text = sdr["Website"].ToString();
                            txtEmail.Text = sdr["Email"].ToString();
                            txtAddress.Text = sdr["Address"].ToString();
                            ddlCountry.SelectedValue = sdr["Country"].ToString();
                            txtState.Text = sdr["State"].ToString();
                            btnAdd.Text = "Update";
                            LinkBack.Visible = true;
                            Session["title"] = "Edit Job";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Job not Found..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    SqlCommand cmd;
                    string Type, ConcatQuery, ImagePath = string.Empty;
                    bool isValidToExecute = false;
                    if(Request.QueryString["id"] != null)
                    {
                        if (fuCompanyLogo.HasFile)
                        {
                            if (Utils.IsValidExtention(fuCompanyLogo.FileName))
                            {
                                ConcatQuery = "CompanyImage = @CompanyImage";
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

                        string query = @"update Jobs set Title=@Title, NoOfPost=@NoOfPost, Description=@Description, Qualification=@Qualification,
                            Experience=@Experience, Specialisation=@Specialisation, LastDateToApply=@LastDateToApply,Salary=@Salary, JobType=@JobType, 
                            CompanyName=@CompanyName," + ConcatQuery+ @"Website=@Website, Email=@Email, Address=@Address, Country=@Country,
                            State=@State where JobId = @id";
                        Type = "Updated";
                        DateTime time = DateTime.Now;
                        cmd = new SqlCommand(query, Con);
                        cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPositions.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                        cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                        cmd.Parameters.AddWithValue("@Specialisation", txtSpecialization.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                        cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                        cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                        cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                        //cmd.Parameters.AddWithValue("@CompanyImage", txtJobTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                        cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", Request.QueryString["id"].ToString());
                        if (fuCompanyLogo.HasFile)
                        {
                            if (Utils.IsValidExtention(fuCompanyLogo.FileName))
                            {
                                Guid obj = Guid.NewGuid();
                                ImagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;
                                fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);
                                cmd.Parameters.AddWithValue("@CompanyImage", ImagePath);
                                isValidToExecute = true;
                            }
                            else
                            {
                                lblMsg.Text = "Please select .jpg, .jpeg, .png file for logo";
                                lblMsg.CssClass = "alert alert-danger";
                            }
                        }
                        else
                        { 
                            isValidToExecute = true;
                        }

                    }
                    else
                    {
                        string query = @"insert into Jobs values (@Title, @NoOfPost, @Description, @Qualification, @Experience, @Specialisation, @LastDateToApply,
                                        @Salary, @JobType, @CompanyName, @CompanyImage, @Website, @Email, @Address, @Country, @State, @CreateDate)";
                        Type = "Saved";
                        DateTime time = DateTime.Now;
                        cmd = new SqlCommand(query, Con);
                        cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPositions.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                        cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                        cmd.Parameters.AddWithValue("@Specialisation", txtSpecialization.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                        cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                        cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                        cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                        //cmd.Parameters.AddWithValue("@CompanyImage", txtJobTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                        cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                        cmd.Parameters.AddWithValue("@CreateDate", time);
                        if (fuCompanyLogo.HasFile)
                        {
                            if (Utils.IsValidExtention(fuCompanyLogo.FileName))
                            {
                                Guid obj = Guid.NewGuid();
                                ImagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;
                                fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);
                                cmd.Parameters.AddWithValue("@CompanyImage", ImagePath);
                                isValidToExecute = true;
                            }
                            else
                            {
                                lblMsg.Text = "Please select .jpg, .jpeg, .png file for logo";
                                lblMsg.CssClass = "alert alert-danger";
                            }
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CompanyImage", ImagePath);
                            isValidToExecute = true;
                        }
                    }
                    if (isValidToExecute)
                    {
                        Con.Open();
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            lblMsg.Text = "Job " + Type + " Successfully..!";
                            lblMsg.CssClass = "alert alert-success";
                            Clear();
                        }
                        else
                        {
                            lblMsg.Text = "Cannot " + Type + " the records please try after sometime..!";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
        }

        private void Clear()
        {
            txtJobTitle.Text = string.Empty;
            txtNoOfPositions.Text = string.Empty;
            txtLastDate.Text = string.Empty;
            txtExperience.Text = string.Empty;
            txtSpecialization.Text = string.Empty;
            txtJobTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtCompany.Text = string.Empty;
            txtQualification.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSalary.Text = string.Empty;
            txtState.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            ddlJobType.ClearSelection();
            ddlCountry.ClearSelection();
        }

        
    }
}