using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalNomadApp.Endpoints
{
    public class GetNomadById
    {

        [FunctionName("GetNomadById")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/GetNomad")] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();


            var responseMessage = new ResponseModel
            {
                Id = "2",
                Name = "TestUserName",
                Description = "TestUserDescription",
                Url = "https://localhost:8080//test",
                Type = "Test",
            };

            return new OkObjectResult(responseMessage);
        }
    }
}
