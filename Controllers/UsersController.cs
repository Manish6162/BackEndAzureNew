using Azure.Storage.Blobs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections;
using System.Reflection;
using System.Resources;
using User = BackEnd.Entities.User;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Container _usersContainer;
        private readonly BlobServiceClient _blobServiceClient;
        private static readonly string[] ProfilePicUrls =
        {
            "https://mediastorageasmr.blob.core.windows.net/mediacontainer/default-profile-pic1.jpg",
            "https://mediastorageasmr.blob.core.windows.net/mediacontainer/default-profile-pic2.jpg",
            "https://mediastorageasmr.blob.core.windows.net/mediacontainer/default-profile-pic3.jpg"
        };

        public UsersController(CosmosDbContext cosmosDbContext, BlobServiceClient blobServiceClient)
        {
            _usersContainer = cosmosDbContext.UsersContainer;
            _blobServiceClient = blobServiceClient;
        }

        [HttpGet("toTestWebApiDeployment")]
        public IActionResult toTestWebApiDeployment()
        {
            return Ok();
        }

        [HttpPost("checkOrGenerateUser")]
        public async Task<IActionResult> CheckOrGenerateUser([FromBody] UserMetadata metadata)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.ip = @ip AND c.userAgent = @userAgent")
                    .WithParameter("@ip", metadata.Ip)
                    .WithParameter("@userAgent", metadata.UserAgent);

                var iterator = _usersContainer.GetItemQueryIterator<User>(query);
                if (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    var existingUser = response.FirstOrDefault();
                    if (existingUser != null)
                    {
                        return Ok(existingUser);
                    }
                }

                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = GenerateUsername(),
                    ProfilePicUrl = GetRandomProfilePicUrl(),
                    Ip = metadata.Ip,
                    UserAgent = metadata.UserAgent,
                    BrowserInfo = metadata.BrowserInfo,
                    Location = metadata.Location
                };

                await _usersContainer.CreateItemAsync(newUser);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateUsername()
        {
            string adjective = GetRandomResource("Strings_Adjectives");
            string noun = GetRandomResource("Strings_Nouns");
            string number = GenerateRandomNumber();

            return $"{adjective}_{noun}_{number}";
        }

        private string GetRandomResource(string resourceName)
        {
            try
            {
                string resourceKey = $"BackEnd.Resources.{resourceName}.resources";
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceKey))
                {
                    if (stream == null) throw new Exception("Resource not found.");
                    using (var reader = new ResourceReader(stream))
                    {
                        var resources = reader.OfType<DictionaryEntry>().ToList();
                        int randomIndex = new Random().Next(0, resources.Count);

                        return resources[randomIndex].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GenerateRandomNumber()
        {
            return new Random().Next(100, 1000).ToString();
        }

        private string GetRandomProfilePicUrl()
        {
            Random random = new Random();
            int index = random.Next(ProfilePicUrls.Length);
            return ProfilePicUrls[index];
        }
    }

    public class UserMetadata
    {
        public string Ip { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string BrowserInfo { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
