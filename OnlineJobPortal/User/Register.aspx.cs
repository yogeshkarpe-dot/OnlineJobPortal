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
    public partial class Register : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            using(SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    string Query = @"Insert into [User] (UserName, Password, Name, Address, Mobile, Email, Country) values (@UserName, @Password, @Name, @Address, @Mobile, @Email, @Country)";
                    SqlCommand Cmd = new SqlCommand(Query, Con);
                    Cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                    Cmd.Parameters.AddWithValue("@Password", txtConfirmPassword.Text.Trim());
                    Cmd.Parameters.AddWithValue("@Name", txtFullName.Text.Trim());
                    Cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    Cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    Cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    Cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    Con.Open();
                    int r = Cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Registered Successfully.";
                        lblMessage.CssClass = "alert alert-success";
                        Clear();
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Cannot Save record rigth now, plz try after sometime..!";
                        lblMessage.CssClass = "alert alert-danger";
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "<b>"+txtUserName.Text.Trim()+"</b>" + " Username already exist, try new one..!";
                        lblMessage.CssClass = "alert alert-danger";
                    }
                    else
                    {
                        Response.Write("<script>alert('" + ex.Message + "')</script>");
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
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            ddlCountry.ClearSelection();
        }
    }
}