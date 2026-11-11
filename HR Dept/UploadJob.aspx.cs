using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web.UI.WebControls;

namespace HR_Dept
{
    public partial class UploadJob : System.Web.UI.Page
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
                LoadEducationLevels();
                LoadSkills();
            }
        }

     
        private void LoadEducationLevels()
        {
            try
            {
                ddlEducationLevel.Items.Clear();
                ddlEducationLevel.Items.Add(new ListItem("-- Select Level --", ""));
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT education_level_id, level_name FROM education_levels ORDER BY education_level_id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlEducationLevel.Items.Add(new ListItem(
                                reader["level_name"].ToString(),
                                reader["education_level_id"].ToString()
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblFeedback.Text = "Error loading education levels: " + ex.Message;
                lblFeedback.ForeColor = System.Drawing.Color.Red;
            }
        }

        
        private void LoadSkills()
        {
            try
            {
                ddlSkills.Items.Clear();
                ddlSkills.Items.Add(new ListItem("-- Select Skill --", ""));
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT skill_id, skill_name FROM skills ORDER BY skill_name";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                   
                            while (reader.Read())
                            {
                                ddlSkills.Items.Add(new ListItem(
                                    reader["skill_name"].ToString(),
                                    reader["skill_id"].ToString()
                                ));
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                lblFeedback.Text = "Error loading skills: " + ex.Message;
                lblFeedback.ForeColor = System.Drawing.Color.Red;
            }
        }

       
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJobTitle.Text))
            {
                lblFeedback.Text = "Please enter a job title.";
                lblFeedback.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (ddlEducationLevel.SelectedValue == "")
            {
                lblFeedback.Text = "Please select an education level.";
                lblFeedback.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string insertJob = @"INSERT INTO jobs 
                                        (job_title, department, education_level_id, education_keywords, experience_keywords) 
                                    VALUES 
                                        (@jobTitle, @department, @educationLevelId, @educationKeywords, @experience_keywords);
                                    SELECT LAST_INSERT_ID();";

                        int newJobId;

                     
                        using (MySqlCommand cmd = new MySqlCommand(insertJob, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@jobTitle", txtJobTitle.Text.Trim());
                            cmd.Parameters.AddWithValue("@department", txtDept.Text.Trim());
                            cmd.Parameters.AddWithValue("@educationLevelId", ddlEducationLevel.SelectedValue);
                            cmd.Parameters.AddWithValue("@educationKeywords", txtEducationField.Text.Trim());
                            cmd.Parameters.AddWithValue("@experience_keywords", txtExperienceKeywords.Text.Trim());


                            newJobId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (ListItem item in lstSelectedSkills.Items)
                        {
                            string insertSkill = @"INSERT INTO job_skills (job_id, skill_id) VALUES (@jobId, @skillId)";

                           
                            using (MySqlCommand cmd = new MySqlCommand(insertSkill, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@jobId", newJobId);
                                cmd.Parameters.AddWithValue("@skillId", item.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }

                       
                        transaction.Commit();

                        lblFeedback.Text = "Job posted successfully!";
                        lblFeedback.ForeColor = System.Drawing.Color.Green;

                       
                        txtJobTitle.Text = "";
                        txtDept.Text = "";
                        txtEducationField.Text = "";
                        txtExperienceKeywords.Text = "";
                        ddlEducationLevel.SelectedIndex = 0;
                        lstSelectedSkills.Items.Clear();
                        if (ddlSkills.Items.Count > 0) ddlSkills.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                       
                        transaction.Rollback();
                        lblFeedback.Text = "Error posting job: " + ex.Message;
                        lblFeedback.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        protected void btnAddSkill_Click(object sender, EventArgs e)
        {
            if (ddlSkills.SelectedIndex == -1)
                return;

            foreach (ListItem item in lstSelectedSkills.Items)
            {
                if (item.Value == ddlSkills.SelectedValue)
                    return; 
            }

            lstSelectedSkills.Items.Add(
                new ListItem(
                    ddlSkills.SelectedItem.Text,
                    ddlSkills.SelectedValue
                )
            );
        }
        protected void btnRemoveSkill_Click(object sender, EventArgs e)
        {
            if (lstSelectedSkills.SelectedIndex >= 0)
                lstSelectedSkills.Items.RemoveAt(lstSelectedSkills.SelectedIndex);
        }
        protected void btnNewSkill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewSkill.Text))
                return;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string check = "SELECT skill_id FROM skills WHERE LOWER(skill_name)=@name";

                using (MySqlCommand cmd = new MySqlCommand(check, connection))
                {
                    cmd.Parameters.AddWithValue("@name", txtNewSkill.Text.Trim());

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        string insert =
                            "INSERT INTO skills(skill_name) VALUES(@name)";

                        using (MySqlCommand insertCmd =
                            new MySqlCommand(insert, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@name",
                                txtNewSkill.Text.Trim());

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            ddlSkills.Items.Clear();
            LoadSkills();

            ListItem newItem = ddlSkills.Items.FindByText(txtNewSkill.Text.Trim());

            if (newItem != null)
            {
                ddlSkills.ClearSelection();
                newItem.Selected = true;
            }

            txtNewSkill.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminBoard.aspx");
        }
    }
}
