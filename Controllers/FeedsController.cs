using Azure.Storage.Blobs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly Container _feedsContainer;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "mediastorage";

        public FeedsController(CosmosDbContext cosmosDbContext, BlobServiceClient blobServiceClient)
        {
            _feedsContainer = cosmosDbContext.FeedsContainer;
            _blobServiceClient = blobServiceClient;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFeedWithBlob([FromForm] IFormFile file, [FromForm] string userId, [FromForm] string profilePicUrl)
        {
            try
            {
                // Check if the request has a file
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                // Create a new feed object
                var feed = new Feed
                {
                    FeedId = Guid.NewGuid().ToString(),
                    UserId = userId, // Set the user ID
                    ProfilePicUrl = profilePicUrl, // Set the profile picture URL
                    CurrentTime = DateTime.UtcNow,
                    Likes = 0,
                    Comments = 0,
                    Url = string.Empty // Set the URL to empty for now
                };

                // Generate a unique file name for blob storage
                string fileName = $"{feed.FeedId}_{file.FileName}";

                // Get the Blob container client
                BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);

                // Get the Blob client for the new file
                BlobClient blob = container.GetBlobClient(fileName);

                // Upload the file to Blob storage
                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream);
                }

                // Set the feed URL to the Blob URI
                feed.Url = blob.Uri.ToString();

                // Add the new feed record to Cosmos DB
                await _feedsContainer.CreateItemAsync(feed);

                // Return success response
                return Ok(new { Message = "Feed created successfully.", FeedId = feed.FeedId });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"An error occurred while creating the feed: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFeeds()
        {
            try
            {
                var queryDefinition = new QueryDefinition("SELECT * FROM c ORDER BY c.currentTime DESC");
                var feedIterator = _feedsContainer.GetItemQueryIterator<Feed>(queryDefinition);
                var orderedFeeds = new List<Feed>();

                while (feedIterator.HasMoreResults)
                {
                    var feedResponse = await feedIterator.ReadNextAsync();
                    orderedFeeds.AddRange(feedResponse.ToList());
                }

                return Ok(new
                {
                    OrderedFeeds = orderedFeeds
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}
