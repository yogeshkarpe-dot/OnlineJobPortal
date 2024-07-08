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
    public partial class Job_listing : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
        DataTable dt;
        public int jobount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowJobList();
                RBSelectedColorChange();
            }
        }

        private void ShowJobList()
        {
            if (dt == null)
            {
                using (SqlConnection Con = new SqlConnection(CS))
                {
                    string query = @"select JobId, Title, Salary, JobType, CompanyName, CompanyImage, Country, State, CreateDate from Jobs";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                }
            }
            DataList1.DataSource = dt;
            DataList1.DataBind();
            lbljobCount.Text = JobCount(dt.Rows.Count);
        }

        private string JobCount(int count)
        {
            if(count > 1)
            {
                return "Total <b>" + count + "</b> jobs found";
            }
            else if(count == 1)
            {
                return "Only <b>" + count + "</b> job found";
            }
            else
            {
                return "No Jobs found";
            }
        }
        private void RBSelectedColorChange()
        {
            if (RadioButtonList1.SelectedItem.Selected == true)
            {
                RadioButtonList1.SelectedItem.Attributes.Add("class", "selectedradio");
            }

        }


        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCountry.SelectedValue != "0") 
            {

                using (SqlConnection Con = new SqlConnection(CS))
                {
                    string query = @"select JobId, Title, Salary, JobType, CompanyName, CompanyImage, Country, State, CreateDate from Jobs where Country = '" + ddlCountry.SelectedValue + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                    ShowJobList();
                    RBSelectedColorChange();
                }
            }
            else
            {
                ShowJobList();
                RBSelectedColorChange();
            }
            
        }

        protected string GetImageUrl(object url)
        {
            string url1 = "";
            if(string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "~/Images/No_image.png";
            }
            else
            {
                url1 = string.Format("~/{0}", url);
            }
            return ResolveUrl(url1);
        }

        public static string RelativeDate(DateTime theDate)

        {

            Dictionary<long, string> thresholds = new Dictionary<long, string>();

            int minute = 60;

            int hour = 60 * minute;

            int day = 24 * hour;

            thresholds.Add(60, "{0} seconds ago");

            thresholds.Add(minute * 2, "a minute ago");

            thresholds.Add(45 * minute, "{0} minutes ago");

            thresholds.Add(120 * minute, "an hour ago");

            thresholds.Add(day, "{0} hours ago");

            thresholds.Add(day * 2, "yesterday");

            thresholds.Add(day * 30, "{0} days ago");

            thresholds.Add(day * 365, "{0} months ago");

            thresholds.Add(long.MaxValue, "{0} years ago");

            long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;

            foreach (long threshold in thresholds.Keys)

            {

                if (since < threshold)

                {

                    TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));

                    return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());

                }

            }

            return "";

        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string JobType = string.Empty;
            JobType = selectedCheckBox();

            if(JobType != "")
            {
                using(SqlConnection Con = new SqlConnection(CS))
                {
                    string query = @"select JobId, TItle, Salary, JobType, CompanyName, CompanyImage, Country, State, CreateDate from Jobs
                                     where JobType IN (" + JobType + ")";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                    ShowJobList();
                    RBSelectedColorChange();
                }
            }
            else
            {
                ShowJobList();
            }
        }

        private string selectedCheckBox()
        {
            string JobType = string.Empty;
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    JobType += "'" + CheckBoxList1.Items[i].Text + "',";
                }
            }
            return JobType = JobType.TrimEnd(',');
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RadioButtonList1.SelectedValue != "0")
            {
                string PostedDate = string.Empty;
                PostedDate = SelectedRadioButton();
                using (SqlConnection Con = new SqlConnection(CS))
                {
                    string query = @"select JobId, Title, Salary, JobType, CompanyName, CompanyImage, Country, State, CreateDate from Jobs
                                     where Convert(DATE, CreateDate)" + PostedDate + " ";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                    ShowJobList();
                    RBSelectedColorChange();
                }
            }
            else
            {
                ShowJobList();
                RBSelectedColorChange();
            }
        }

        private string SelectedRadioButton()
        {
            string PostedDate = string.Empty;
            DateTime Date = DateTime.Today;

            if(RadioButtonList1.SelectedValue == "1")
            {
                PostedDate = "= Convert(DATE,'" + Date.ToString("yyyy/MM/dd") + "')";
            }

            else if (RadioButtonList1.SelectedValue == "2")
            {
                PostedDate = "between Convert(DATE,'" + DateTime.Now.AddDays(-2).ToString("yyyy/MM/dd") + "') and Convert(DATE, '" + Date.ToString("yyyy/MM/dd") + "')" ;
            }

            else if (RadioButtonList1.SelectedValue == "3")
            {
                PostedDate = "between Convert(DATE,'" + DateTime.Now.AddDays(-3).ToString("yyyy/MM/dd") + "') and Convert(DATE, '" + Date.ToString("yyyy/MM/dd") + "')";
            }

            else if (RadioButtonList1.SelectedValue == "4")
            {
                PostedDate = "between Convert(DATE,'" + DateTime.Now.AddDays(-4).ToString("yyyy/MM/dd") + "') and Convert(DATE, '" + Date.ToString("yyyy/MM/dd") + "')";
            }

            else
            {
                PostedDate = "between Convert(DATE,'" + DateTime.Now.AddDays(-10).ToString("yyyy/MM/dd") + "') and Convert(DATE, '" + Date.ToString("yyyy/MM/dd") + "')";
            }

            return PostedDate;
        }

        protected void lbFilter_Click(object sender, EventArgs e)
        {
            bool IsCondition = false;
            string SubQuery = string.Empty;
            string JobType = string.Empty;
            string PostedDate = string.Empty;
            string query = string.Empty;
            List<string> queryList = new List<string>();

            using (SqlConnection Con = new SqlConnection(CS))
            {
                try
                {
                    if (ddlCountry.SelectedValue != "0")
                    {
                        queryList.Add(" Country = '" + ddlCountry.SelectedValue + "' ");
                        IsCondition = true;
                    }

                    JobType = selectedCheckBox();

                    if (JobType != "")
                    {
                        queryList.Add(" JobType IN (" + JobType + ") ");
                        IsCondition = true;
                    }

                    if (RadioButtonList1.SelectedValue != "0")
                    {
                        PostedDate = SelectedRadioButton();
                        queryList.Add(" Convert(DATE, CreateDate) " + PostedDate);
                        IsCondition = true;
                    }

                    if (IsCondition)
                    {
                        foreach (string a in queryList)
                        {
                            SubQuery += a + " and ";
                        }

                        SubQuery = SubQuery.Remove(SubQuery.LastIndexOf("and"), 3);
                        query = @"select JobId, Title, Salary, JobType, CompanyName, CompanyImage, Country, State, CreateDate from Jobs where " + SubQuery + " ";
                    }
                    else
                    {
                        query = @"select JobId, Title, Salary, JobType, CompanyName, CompanyImage, Country, State, CreateDate from Jobs";
                    }

                    SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                    dt = new DataTable();
                    sda.Fill(dt);
                    ShowJobList();
                    RBSelectedColorChange();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void lbReset_Click(object sender, EventArgs e)
        {
            ddlCountry.ClearSelection();
            CheckBoxList1.ClearSelection();
            RadioButtonList1.SelectedValue = "0";
            RBSelectedColorChange();
            ShowJobList();
        }
    }
}