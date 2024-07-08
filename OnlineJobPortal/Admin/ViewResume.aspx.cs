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
    public partial class ViewResume : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }

            if (!IsPostBack)
            {
                ShowAppliedJob();
            }
        }

        private void ShowAppliedJob()
        {
            string query = string.Empty;
            using (SqlConnection Con = new SqlConnection(CS))
            {
                query = @"select Row_Number() over(Order by (select 1)) 
                        as [Sr.No], aj.AppliedJobId, j.CompanyName, aj.JobId, j.Title, u.Mobile, u.Name, u.Email, u.Resume
                        from AppliedJobs aj
                        inner join [User] u on aj.UserId = u.UserId
                        inner join Jobs j on aj.JobId = j.JobId";

                SqlCommand cmd = new SqlCommand(query, Con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    sda.Fill(dt);

                    // Debug: Check the content of the DataTable
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            Console.WriteLine($"{column.ColumnName}: {row[column]}");
                        }
                    }

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("FormatException: " + ex.Message);
                    // Log or handle the exception as needed
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowAppliedJob();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    GridViewRow row = GridView1.Rows[e.RowIndex];
                    int AppliedJobId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                    SqlCommand cmd = new SqlCommand("delete from AppliedJobs where AppliedJobId = @id", Con);
                    cmd.Parameters.AddWithValue("@id", AppliedJobId);
                    Con.Open();
                    int r = cmd.ExecuteNonQuery();

                    if (r > 0)
                    {
                        lblMsg.Text = "Resume deletd successfully";
                        lblMsg.CssClass = "alert alert-success";
                    }
                    else
                    {
                        lblMsg.Text = "Cannot delete this record..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                    GridView1.EditIndex = -1;
                    ShowAppliedJob();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "select$" + e.Row.RowIndex);
            e.Row.ToolTip = "click to view job details";
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(GridViewRow row in GridView1.Rows)
            {
                if(row.RowIndex == GridView1.SelectedIndex)
                {
                    HiddenField JobId = (HiddenField)row.FindControl("hdnJobId");
                    Response.Redirect("JobList.aspx?id=" + JobId.Value);
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row";
                }
            }
        }
    }
}