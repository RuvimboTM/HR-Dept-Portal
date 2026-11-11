using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace HR_Dept
{
    public partial class AdminBoard : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadJobs();
            }
        }

        private void LoadJobs()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // JOIN jobs with education_levels to get the level name
                    // Also pull skills as a comma-separated string using GROUP_CONCAT
                    string sql = @"
                        SELECT 
                            j.job_id,
                            j.job_title,
                            j.department,
                            el.level_name,
                            j.education_keywords,
                            IFNULL(GROUP_CONCAT(s.skill_name ORDER BY s.skill_name SEPARATOR ', '), 'No skills listed') AS skills_display
                        FROM jobs j
                        LEFT JOIN education_levels el ON j.education_level_id = el.education_level_id
                        LEFT JOIN job_skills js ON j.job_id = js.job_id
                        LEFT JOIN skills s ON js.skill_id = s.skill_id
                        GROUP BY j.job_id, j.job_title, j.department, el.level_name, j.education_keywords
                        ORDER BY j.job_id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            rptJobs.DataSource = dt;
                            rptJobs.DataBind();
                            lblNoJobs.Visible = false;
                        }
                        else
                        {
                            lblNoJobs.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblNoJobs.Visible = true;
                lblNoJobs.Text = "Error loading jobs: " + ex.Message;
                ClientScript.RegisterStartupScript(GetType(), "Error", $"alert('Error loading jobs: {ex.Message}');", true);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Response.Redirect("UploadJob.aspx");
        }
    }
}
