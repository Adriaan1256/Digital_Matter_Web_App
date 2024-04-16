using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Devices
{
    public class EditModel : PageModel
    {
        public DeviceInfo deviceInfo = new DeviceInfo();
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
                    String sql = "SELECT * FROM Devices WHERE Device_ID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                deviceInfo.id = "" + reader.GetInt32(0);
                                deviceInfo.name = reader.GetString(1);
                                deviceInfo.type = reader.GetString(2);
                                deviceInfo.groupid = "" + reader.GetInt32(3);
                                deviceInfo.firmwareid = "" + reader.GetInt32(4);

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
            deviceInfo.id = Request.Form["id"];
            deviceInfo.name = Request.Form["name"];
            deviceInfo.type = Request.Form["devicetype"];
            deviceInfo.groupid = Request.Form["groupid"];
            deviceInfo.firmwareid = Request.Form["firmwareid"];

            if (deviceInfo.name.Length == 0 || deviceInfo.type.Length == 0 || deviceInfo.groupid.Length == 0 ||
                deviceInfo.firmwareid.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Devices " +
                                 "SET Device_Name = @name, Device_Type  = @type, Group_ID = @groupid, Firmware_ID = @firmwareid " +
                                 "WHERE Device_ID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", deviceInfo.name);
                        command.Parameters.AddWithValue("@type", deviceInfo.type);
                        command.Parameters.AddWithValue("@groupid", deviceInfo.groupid);
                        command.Parameters.AddWithValue("@firmwareid", deviceInfo.firmwareid);
                        command.Parameters.AddWithValue("@id", deviceInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Devices/Index");
        }
    }
}
