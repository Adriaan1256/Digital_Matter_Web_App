using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Groups
{
    public class BaseModel : PageModel
    {
        public List<GroupInfo> listGroups = new List<GroupInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Groups";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GroupInfo groupInfo = new GroupInfo();
                                groupInfo.id = reader.IsDBNull(0) ? null : reader.GetInt32(0).ToString();
                                groupInfo.name = reader.IsDBNull(1) ? null : reader.GetString(1);
                                groupInfo.parentid = reader.IsDBNull(2) ? null : reader.GetInt32(2).ToString();

                                listGroups.Add(groupInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class GroupInfo
    {
        public String id;
        public String name;
        public String parentid;
    }
}

