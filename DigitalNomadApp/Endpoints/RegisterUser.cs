using DigitalNomadApp.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DigitalNomadApp.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace DigitalNomadApp.Endpoints
{
    public class RegisterUser
    {


        [FunctionName("RegisterNomad")]
        public static async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/register")] HttpRequest req,
         ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<UserDetails>(requestBody);

            if (user == null)
            {
                return new BadRequestObjectResult("Invalid user data");
            }

            try
            {
               
                var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "INSERT INTO [userdetails] (id, firstname, lastname, email, password, phonenumber) VALUES (@id, @firstname, @lastname, @email,CONVERT(VARBINARY(MAX), @password), @phonenumber); SELECT SCOPE_IDENTITY();";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", Guid.NewGuid());
                        command.Parameters.AddWithValue("@firstname", user.firstName);
                        command.Parameters.AddWithValue("@lastname", user.lastName);
                        command.Parameters.AddWithValue("@email", user.email);
                        command.Parameters.AddWithValue("@password", EncryptionHelper.Encrypt(user.password));
                        command.Parameters.AddWithValue("@phonenumber", user.phoneNumber);

                        var userId = await command.ExecuteScalarAsync();
                       

                        return new OkObjectResult("user successfully created");
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while registering the user");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
