using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Firmware
{
    public class BaseModel : PageModel
    {
        public List<FirmwareInfo> listFirmware = new List<FirmwareInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Firmware";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FirmwareInfo firmwareInfo = new FirmwareInfo();
                                firmwareInfo.id = reader.IsDBNull(0) ? null : reader.GetInt32(0).ToString();
                                firmwareInfo.version = reader.IsDBNull(0) ? null : reader.GetString(1);
                                firmwareInfo.releasedate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                                firmwareInfo.description = reader.IsDBNull(0) ? null : reader.GetString(3);
                                listFirmware.Add(firmwareInfo);
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

    public class FirmwareInfo
    {
        public String id;
        public String version;
        public DateTime releasedate;
        public String FormattedDate => releasedate.ToString("yyyy-MM-dd");
        public String description;
    }
}
