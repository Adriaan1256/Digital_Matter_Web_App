using Digital_Matter_Web_App.Pages.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Digital_Matter_Web_App.Pages.Firmware
{
    public class CreateNewModel : PageModel
    {
        public FirmwareInfo firmwareInfo = new FirmwareInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            firmwareInfo.version = Request.Form["version"];
            string releaseDateStr = Request.Form["releasedate"];
            firmwareInfo.description= Request.Form["description"];

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
                    String sql = "INSERT INTO Firmware " +
                                 "(Version, Release_Date, Description) VALUES" +
                                 "(@version, @releasedate, @description)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@version", firmwareInfo.version);
                        command.Parameters.AddWithValue("@releasedate", releasedate);
                        command.Parameters.AddWithValue("@description", firmwareInfo.description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            firmwareInfo.version = "";
            releaseDateStr = "";
            firmwareInfo.description = "";
            successMessage = "New Firmware Added Correctly";

            Response.Redirect("/Firmware/Base");
        }

    }
}
