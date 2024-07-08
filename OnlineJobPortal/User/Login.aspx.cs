using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace OnlineJobPortal.User
{
    public partial class Login : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        string username, password = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    if(ddlLoginType.SelectedValue == "Admin")
                    {
                        username = ConfigurationManager.AppSettings["username"];
                        password = ConfigurationManager.AppSettings["password"];
                        if(username == txtUserName.Text.Trim() && password == txtPassword.Text.Trim())
                        {
                            Session["admin"] = username;
                            Response.Redirect("../Admin/Dashboard.aspx", false);
                        }
                        else
                        {
                            ShowErrorMsg("Admin");
                        }
                    }
                    else
                    {
                        string Query = @"select * from [User] where UserName=@UserName and Password=@Password";
                        SqlCommand Cmd = new SqlCommand(Query, Con);
                        Cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                        Cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                        
                        Con.Open();
                        SqlDataReader sdr = Cmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            Session["User"] = sdr["UserName"].ToString();
                            Session["UserId"] = sdr["UserId"].ToString();
                            Response.Redirect("../User/Default.aspx", false);
                        }
                        else
                        {
                            ShowErrorMsg("User");
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "')</script>");
                }
            }
        }

        private void ShowErrorMsg(string UserType)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "<b>" + UserType + "</b>" + " Credentials are incorrect..!";
            lblMessage.CssClass = "alert alert-danger";
        }
    }
}