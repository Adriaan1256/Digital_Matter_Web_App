using Digital_Matter_Web_App.Pages.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Firmware
{
    public class EditModel : PageModel
    {
        public FirmwareInfo firmwareInfo = new FirmwareInfo();
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
                    String sql = "SELECT * FROM Firmware WHERE Firmware_ID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                firmwareInfo.id = reader.IsDBNull(0) ? null : reader.GetInt32(0).ToString();
                                firmwareInfo.version = reader.IsDBNull(0) ? null : reader.GetString(1);
                                firmwareInfo.releasedate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                                firmwareInfo.description = reader.IsDBNull(0) ? null : reader.GetString(3);
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
            firmwareInfo.id = Request.Form["id"];
            firmwareInfo.version = Request.Form["version"];
            string releaseDateStr = Request.Form["releasedate"];
            firmwareInfo.description = Request.Form["description"];

            if (string.IsNullOrEmpty(firmwareInfo.version) || string.IsNullOrEmpty(releaseDateStr) || string.IsNullOrEmpty(firmwareInfo.description))
            {
                errorMessage = "All the fields are required";
                return;
            }

            if (!DateTime.TryParse(releaseDateStr, out DateTime releasedate))
            {
                errorMessage = "Invalid release date format.";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Digital_Matter_Devices;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Firmware " +
                                 "SET Version = @version, Release_Date  = @releasedate, Description = @description " +
                                 "WHERE Firmware_ID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@version", firmwareInfo.version);
                        command.Parameters.AddWithValue("@releasedate", releasedate);
                        command.Parameters.AddWithValue("@description", firmwareInfo.description);
                        command.Parameters.AddWithValue("@id", firmwareInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Firmware/Base");
        }
    }
}