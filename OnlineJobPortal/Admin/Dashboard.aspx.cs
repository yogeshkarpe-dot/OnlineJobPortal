using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace OnlineJobPortal.Admin
{
    public partial class Dashboard : System.Web.UI.Page
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
                Users();
                Jobs();
                AppliedJobs();
                ContactCount();
            }
        }

        private void ContactCount()
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from Contact", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Session["Contact"] = dt.Rows[0][0];
                }
                else
                {
                    Session["Contact"] = 0;
                }
            }
        }

        private void AppliedJobs()
        {
            using(SqlConnection Con = new SqlConnection(CS))
            {
                SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from AppliedJobs", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    Session["AppliedJobs"] = dt.Rows[0][0];
                }
                else
                {
                    Session["AppliedJobs"] = 0;
                }
            }
        }

        private void Jobs()
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from Jobs", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Session["Jobs"] = dt.Rows[0][0];
                }
                else
                {
                    Session["Jobs"] = 0;
                }
            }
        }

        private void Users()
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                SqlDataAdapter sda = new SqlDataAdapter("select Count(*) from [User]", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Session["Users"] = dt.Rows[0][0];
                }
                else
                {
                    Session["Users"] = 0;
                }
            }
        }
    }
}