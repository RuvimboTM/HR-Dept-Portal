using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;

namespace HR_Dept
{
    public partial class UpdateJob : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        int jobId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            if (Request.QueryString["job_id"] == null)
            {
                Response.Redirect("AdminBoard.aspx");
            }

            jobId = Convert.ToInt32(Request.QueryString["job_id"]);

            if (!IsPostBack)
            {
                LoadEducationLevels();
                LoadSkills();
                LoadJob();  
            }
        }

        private void LoadEducationLevels()
        {
            try
            {
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
                lblMessage.Text = "Error loading education levels: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void LoadSkills()
        {
            ddlSkills.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT skill_id, skill_name FROM skills ORDER BY skill_name";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ddlSkills.Items.Add(
                            new ListItem(
                                reader["skill_name"].ToString(),
                                reader["skill_id"].ToString()
                            ));
                    }
                }
            }
        }

        private void LoadJob()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                
                    string sql = "SELECT job_title, department, education_level_id, education_keywords, experience_keywords FROM jobs WHERE job_id = @job_id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@job_id", jobId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtJobTitle.Text = reader["job_title"].ToString();
                                txtDepartment.Text = reader["department"].ToString();
                                txtEducationField.Text = reader["education_keywords"].ToString();
                                txtExperienceKeywords.Text = reader["experience_keywords"].ToString();



                                string levelId = reader["education_level_id"].ToString();
                                ListItem selectedLevel = ddlEducationLevel.Items.FindByValue(levelId);
                                if (selectedLevel != null)
                                    selectedLevel.Selected = true;
                            }
                            else
                            {
                              
                                Response.Redirect("AdminBoard.aspx");
                            }
                        }
                    }

                    string skillsSql = @"
SELECT s.skill_id, s.skill_name
FROM job_skills js
INNER JOIN skills s
ON js.skill_id = s.skill_id
WHERE js.job_id=@job_id";

                    using (MySqlCommand cmd = new MySqlCommand(skillsSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@job_id", jobId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            lstSelectedSkills.Items.Clear();

                            while (reader.Read())
                            {
                                lstSelectedSkills.Items.Add(
                                    new ListItem(
                                        reader["skill_name"].ToString(),
                                        reader["skill_id"].ToString()
                                    ));
                            }
                        }
                    }

                 
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading job: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJobTitle.Text))
            {
                lblMessage.Text = "Please enter a job title.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (ddlEducationLevel.SelectedValue == "")
            {
                lblMessage.Text = "Please select an education level.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 1: Update the job details
                    string sql = @"UPDATE jobs 
                                   SET job_title = @title, 
                                       department = @dept,
                                       education_level_id = @educationLevelId,
                                       education_keywords = @educationField,
experience_keywords=@experienceKeywords
                                   WHERE job_id = @job_id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@title", txtJobTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@dept", txtDepartment.Text.Trim());
                        cmd.Parameters.AddWithValue("@educationLevelId", ddlEducationLevel.SelectedValue);
                        cmd.Parameters.AddWithValue("@educationField", txtEducationField.Text.Trim());
                        cmd.Parameters.AddWithValue("@experienceKeywords", txtExperienceKeywords.Text.Trim());

                        cmd.Parameters.AddWithValue("@job_id", jobId);
                        cmd.ExecuteNonQuery();
                    }

                    //  Delete old skills for this job, then re-insert the new selection
                    string deleteSkills = "DELETE FROM job_skills WHERE job_id = @job_id";

                    using (MySqlCommand cmd = new MySqlCommand(deleteSkills, connection))
                    {
                        cmd.Parameters.AddWithValue("@job_id", jobId);
                        cmd.ExecuteNonQuery();
                    }

                    foreach (ListItem item in lstSelectedSkills.Items)
                    {
                        string insertSkill = @"INSERT INTO job_skills(job_id,skill_id)
                           VALUES(@jobId,@skillId)";

                        using (MySqlCommand cmd = new MySqlCommand(insertSkill, connection))
                        {
                            cmd.Parameters.AddWithValue("@jobId", jobId);
                            cmd.Parameters.AddWithValue("@skillId", item.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    lblMessage.Text = "Job updated successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating job: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Delete skills first (foreign key constraint)
        //            string deleteSkills = "DELETE FROM job_skills WHERE job_id = @job_id";
        //            using (MySqlCommand cmd = new MySqlCommand(deleteSkills, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@job_id", jobId);
        //                cmd.ExecuteNonQuery();
        //            }

        //            // Then delete the job
        //            string deleteJob = "DELETE FROM jobs WHERE job_id = @job_id";
        //            using (MySqlCommand cmd = new MySqlCommand(deleteJob, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@job_id", jobId);
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        Response.Redirect("AdminBoard.aspx");
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = "Error deleting job: " + ex.Message;
        //        lblMessage.ForeColor = System.Drawing.Color.Red;
        //    }
        //}
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    int applicationCount;
                    string countSql = "SELECT COUNT(*) FROM applications WHERE job_id = @job_id";
                    using (MySqlCommand cmd = new MySqlCommand(countSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@job_id", jobId);
                        applicationCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                   
                    if (applicationCount > 0)
                    {
                        lblMessage.Text = "This job has " + applicationCount +
                            " application(s) and cannot be deleted.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                  
                    using (MySqlTransaction tx = connection.BeginTransaction())
                    {
                        try
                        {
                            string deleteSkills = "DELETE FROM job_skills WHERE job_id = @job_id";
                            using (MySqlCommand cmd = new MySqlCommand(deleteSkills, connection, tx))
                            {
                                cmd.Parameters.AddWithValue("@job_id", jobId);
                                cmd.ExecuteNonQuery();
                            }

                            string deleteJob = "DELETE FROM jobs WHERE job_id = @job_id";
                            using (MySqlCommand cmd = new MySqlCommand(deleteJob, connection, tx))
                            {
                                cmd.Parameters.AddWithValue("@job_id", jobId);
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                }

                Response.Redirect("AdminBoard.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting job: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void btnAddSkill_Click(object sender, EventArgs e)
        {
            if (ddlSkills.SelectedItem == null)
                return;

            bool exists = false;

            foreach (ListItem item in lstSelectedSkills.Items)
            {
                if (item.Value == ddlSkills.SelectedValue)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                lstSelectedSkills.Items.Add(
                    new ListItem(
                        ddlSkills.SelectedItem.Text,
                        ddlSkills.SelectedValue));
            }
        }
        protected void btnRemoveSkill_Click(object sender, EventArgs e)
        {
            for (int i = lstSelectedSkills.Items.Count - 1; i >= 0; i--)
            {
                if (lstSelectedSkills.Items[i].Selected)
                    lstSelectedSkills.Items.RemoveAt(i);
            }
        }
        protected void btnAddNewSkill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewSkill.Text))
                return;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO skills(skill_name) VALUES(@skill)";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@skill", txtNewSkill.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                int id;

                using (MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection))
                {
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                ddlSkills.Items.Add(
                    new ListItem(txtNewSkill.Text.Trim(), id.ToString()));

                lstSelectedSkills.Items.Add(
                    new ListItem(txtNewSkill.Text.Trim(), id.ToString()));
            }

            txtNewSkill.Text = "";
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminBoard.aspx");
        }
    }
}
