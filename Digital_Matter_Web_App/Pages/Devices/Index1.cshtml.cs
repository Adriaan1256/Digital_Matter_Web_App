using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Devices
{
    public class Index1Model : PageModel
    {
        public DeviceInfo deviceInfo = new DeviceInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {
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
                    String sql = "INSERT INTO Devices " +
                                 "(Device_Name, Device_Type, Group_ID, Firmware_ID) VALUES" +
                                 "(@name, @type, @groupid, @firmwareid)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", deviceInfo.name);
                        command.Parameters.AddWithValue("@type", deviceInfo.type);
                        command.Parameters.AddWithValue("@groupid", deviceInfo.groupid);
                        command.Parameters.AddWithValue("@firmwareid", deviceInfo.firmwareid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return;
            }


            deviceInfo.name = "";
            deviceInfo.type = "";
            deviceInfo.groupid = "";
            deviceInfo.firmwareid = "";
            successMessage = "New Device Added Correctly";

            Response.Redirect("/Devices/Index");
        }

    }
}
