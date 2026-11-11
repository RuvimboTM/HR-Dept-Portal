using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Mysqlx.Datatypes.Scalar.Types;

namespace HR_Dept
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            {
                if (Page.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                    string email = txtEmail.Text.Trim();
                    string password = txtPassword.Text.Trim();
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        
                        connection.Open();

                   
                        string sql = "SELECT adminId, email FROM admin WHERE email = @email AND password = @password";

                        using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@password", password);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    
                                    Session["AdminId"] = reader["adminId"].ToString();
                                    Session["AdminEmail"] = reader["email"].ToString();
                                    Session["UserRole"] = "Admin";

                                    Response.Redirect("AdminBoard.aspx");
                                }
                                else
                                {
                                    lblFeedback.Text = "Invalid admin credentials!";
                                    lblFeedback.ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            txtPassword.Text = "";

        }
    }
}


        