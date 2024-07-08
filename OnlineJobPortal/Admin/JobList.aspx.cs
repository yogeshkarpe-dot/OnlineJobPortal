using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace OnlineJobPortal.Admin
{
    public partial class JobList : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public Color ColorTra { get; private set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["Admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }

            if (!IsPostBack)
            {
                ShowJob();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowJob();
        }

        private void ShowJob()
        {
            string query = string.Empty;
            using(SqlConnection Con = new SqlConnection(CS))
            {
                query = @"select Row_Number() over(Order by (select 1)) as [Sr.No], JobId, Title, NoOfPost, Qualification, Experience, LastDateToApply, CompanyName, Country, State, CreateDate from Jobs";

                SqlCommand cmd = new SqlCommand(query, Con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

            if (Request.QueryString["id"] != null)
            {
                linkBack.Visible = true;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowJob();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    GridViewRow row = GridView1.Rows[e.RowIndex];
                    int JobId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                    SqlCommand cmd = new SqlCommand("delete from Jobs where JobId = @id", Con);
                    cmd.Parameters.AddWithValue("@id", JobId);
                    Con.Open();
                    int r = cmd.ExecuteNonQuery();

                    if(r > 0)
                    {
                        lblMsg.Text = "Job deletd successfully";
                        lblMsg.CssClass = "alert alert-success";
                    }
                    else
                    {
                        lblMsg.Text = "Cannot delete this record..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                    GridView1.EditIndex = -1;
                    ShowJob();
                }
                catch(Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditJob")
            {
                Response.Redirect("NewJob.aspx?id=" + e.CommandArgument.ToString());
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.ID = e.Row.RowIndex.ToString();
                if (Request.QueryString["id"] != null)
                {
                    int JobId = Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Values[0]);
                    if (JobId == Convert.ToInt32(Request.QueryString["id"]))
                    {
                        e.Row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    }
                }
            }
        }
    }
}