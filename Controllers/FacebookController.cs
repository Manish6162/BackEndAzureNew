using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public FacebookController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("import")]
        public async Task<IActionResult> ImportFromFacebook(string accessToken)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://graph.facebook.com/me?fields=name,email,picture&access_token={accessToken}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var userData = JObject.Parse(jsonResponse);

                    // Create an instance of your entity class
                    var importedUser = new ImportedUser
                    {
                        Name = userData["name"].Value<string>(),
                        Email = userData["email"].Value<string>(),
                        ProfilePictureUrl = userData["picture"]["data"]["url"].Value<string>()
                        // Add more properties as needed
                    };

                    // Process the imported user data, you may save it to the database or perform other operations
                    // For demonstration, returning the imported user data
                    return Ok(importedUser);
                }
                else
                {
                    return BadRequest("Failed to import user data from Facebook.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex}");

                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }

    public class ImportedUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        // Add more properties as needed
    }
}
