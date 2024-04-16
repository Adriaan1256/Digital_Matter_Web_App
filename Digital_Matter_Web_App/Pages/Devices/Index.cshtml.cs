using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace Digital_Matter_Web_App.Pages.Devices
{
    public class IndexModel : PageModel
    {
        public List<DeviceInfo> listDevices = new List<DeviceInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Devices";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DeviceInfo deviceInfo = new DeviceInfo();
                                deviceInfo.id = "" + reader.GetInt32(0);
                                deviceInfo.name = reader.GetString(1);
                                deviceInfo.type = reader.GetString(2);
                                deviceInfo.groupid = "" + reader.GetInt32(3);
                                deviceInfo.firmwareid = "" + reader.GetInt32(4);

                                listDevices.Add(deviceInfo);
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

    public class DeviceInfo
    {
        public String id;
        public String name;
        public String type;
        public String groupid;
        public String firmwareid;
    }
}
