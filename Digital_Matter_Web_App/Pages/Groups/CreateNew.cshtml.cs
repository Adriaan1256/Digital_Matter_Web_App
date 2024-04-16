using Digital_Matter_Web_App.Pages.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Groups
{
    public class CreateNewModel : PageModel
    {
        public GroupInfo groupInfo = new GroupInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
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
                    String sql = "INSERT INTO Groups " +
                                 "(Group_Name, Parent_Group_ID) VALUES" +
                                 "(@name, @parentid)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", groupInfo.name);
                        command.Parameters.AddWithValue("@parentid", parentIdValue);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            groupInfo.name = "";
            groupInfo.parentid = "";
            successMessage = "New Group Added Correctly";

            Response.Redirect("/Groups/Base");
        }

    }
}

