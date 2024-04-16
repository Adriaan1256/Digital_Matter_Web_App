using Digital_Matter_Web_App.Pages.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Groups
{
    public class EditModel : PageModel
    {
        public GroupInfo groupInfo = new GroupInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Groups WHERE Group_ID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                groupInfo.id = reader.IsDBNull(0) ? null : reader.GetInt32(0).ToString();
                                groupInfo.name = reader.IsDBNull(1) ? null : reader.GetString(1);
                                groupInfo.parentid = reader.IsDBNull(2) ? null : reader.GetInt32(2).ToString();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            groupInfo.id = Request.Form["id"];
            groupInfo.name = Request.Form["name"];
            string parentIdStr = Request.Form["parentid"];

            object parentIdValue = string.IsNullOrEmpty(parentIdStr) ? DBNull.Value : (object)parentIdStr;


            if (groupInfo.name.Length == 0)
            {
                errorMessage = "Name is required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Groups " +
                                 "SET Group_Name = @name, Parent_Group_ID  = @parentid " +
                                 "WHERE Group_ID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", groupInfo.name);
                        command.Parameters.AddWithValue("@parentid", parentIdValue);
                        command.Parameters.AddWithValue("@id", groupInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Groups/Base");
        }
    }
}