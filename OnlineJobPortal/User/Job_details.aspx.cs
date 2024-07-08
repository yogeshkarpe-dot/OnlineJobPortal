using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace OnlineJobPortal.User
{
    public partial class Job_details : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        public string JobTitle = string.Empty;
        DataTable dt;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ShowJobDetails();
                DataBind();
            }
            else
            {
                Response.Redirect("Job_listing.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void ShowJobDetails()
        {

            using (SqlConnection Con = new SqlConnection(CS))
            {
                string query = @"select * from Jobs where JobId = @Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", Request.QueryString["id"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
            }
            DataList1.DataSource = dt;
            DataList1.DataBind();
            JobTitle = dt.Rows[0]["Title"].ToString();
        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ApplyJob")
            {
                if (Session["User"] != null)
                {
                    using (SqlConnection Con = new SqlConnection(CS))
                    {
                        try
                        {
                            string query = @"Insert into AppliedJobs Values (@JobId, @UserId)";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
                            cmd.Parameters.AddWithValue("@UserId", Session["UserId"]);
                            Con.Open();
                            int r = cmd.ExecuteNonQuery();
                            if (r > 0)
                            {
                                lblMsg.Visible = true;
                                lblMsg.Text = "Job Applied Successfully!";
                                lblMsg.CssClass = "alert alert-success";
                                ShowJobDetails();
                            }
                            else
                            {
                                lblMsg.Visible = true;
                                lblMsg.Text = "Cannot apply to this job try after some time!";
                                lblMsg.CssClass = "alert alert-danger";
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write("<script>alert('" + ex.Message + "');</script>");
                        }
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
            if (Session["User"] != null)
            {
                LinkButton btnApplyJob = e.Item.FindControl("lbApplyJob") as LinkButton;
                if (IsApplied())
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

        bool IsApplied()
        {
            DataTable dt1;
            using (SqlConnection Con = new SqlConnection(CS))
            {
                string query = @"select * from AppliedJobs where JobId = @JobId and UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@JobId", Request.QueryString["id"]);
                cmd.Parameters.AddWithValue("@UserId", Session["UserId"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dt1 = new DataTable();
                sda.Fill(dt1);
            }

            if(dt1.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string GetImageUrl(object url)
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