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
    public partial class Contact : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
                using (SqlConnection Con = new SqlConnection(CS))
                {
                    try
                    {
                        string Query = @"Insert into Contact values (@Name, @Email, @Subject, @Message)";
                        SqlCommand Cmd = new SqlCommand(Query, Con);
                        Cmd.Parameters.AddWithValue("@Name", name.Value.Trim());
                        Cmd.Parameters.AddWithValue("@Email", email.Value.Trim());
                        Cmd.Parameters.AddWithValue("@Subject", subject.Value.Trim());
                        Cmd.Parameters.AddWithValue("@Message", message.Value.Trim());
                        Con.Open();
                        int r = Cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Thanks for reaching out, will loop into your query.";
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
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('" + ex.Message + "')</script>");
                    }

                }
            

            
        }

        private void Clear()
        {
            name.Value = string.Empty;
            email.Value = string.Empty;
            subject.Value = string.Empty;
            message.Value = string.Empty;
        }
    }
}