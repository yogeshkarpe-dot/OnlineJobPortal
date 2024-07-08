using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace OnlineJobPortal.Admin
{
    public partial class UserList : System.Web.UI.Page
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
                ShowUsers();
            }
        }

        private void ShowUsers()
        {
            string query = string.Empty;
            using (SqlConnection Con = new SqlConnection(CS))
            {
                query = @"select Row_Number() over(Order by (select 1)) as [Sr.No], UserId, Name, Email, Mobile, Country from [User]";

                SqlCommand cmd = new SqlCommand(query, Con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    GridViewRow row = GridView1.Rows[e.RowIndex];
                    int UserId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                    SqlCommand cmd = new SqlCommand("delete from [User] where UserId = @id", Con);
                    cmd.Parameters.AddWithValue("@id", UserId);
                    Con.Open();
                    int r = cmd.ExecuteNonQuery();

                    if (r > 0)
                    {
                        lblMsg.Text = "User deletd successfully";
                        lblMsg.CssClass = "alert alert-success";
                    }
                    else
                    {
                        lblMsg.Text = "Cannot delete this User..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                    GridView1.EditIndex = -1;
                    ShowUsers();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowUsers();
        }

    }
}