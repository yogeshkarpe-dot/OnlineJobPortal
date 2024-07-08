using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace OnlineJobPortal.User
{
    public partial class Profile : System.Web.UI.Page
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
                ShowUserProfile();
            }
        }

        private void ShowUserProfile()
        {
            using(SqlConnection Con = new SqlConnection(CS))
            {
                string query = "select UserId, Username, Name, Address, Mobile, Email, Country, Resume from [User] where Username=@Username";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Username", Session["User"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dlProfile.DataSource = dt;
                    dlProfile.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('please do login again with your new username')</script>");
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void dlProfile_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "EditUserProfile")
            {
                Response.Redirect("ResumeBuild.aspx?id=" + e.CommandArgument.ToString());
            }
        }
    }
}