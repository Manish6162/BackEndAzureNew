using BackEnd.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly Container _chatsContainer;

        public ChatsController(CosmosDbContext cosmosDbContext)
        {
            _chatsContainer = cosmosDbContext.ChatsContainer;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] Chats chat)
        {
            try
            {
                if (chat == null)
                    return BadRequest("Invalid chat data.");

                // Generate a unique chat ID
                chat.ChatId = Guid.NewGuid().ToString();

                // Add the new chat record to Cosmos DB
                await _chatsContainer.CreateItemAsync(chat);

                // Return success response
                return Ok(new { Message = "Chat created successfully.", ChatId = chat.ChatId });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"An error occurred while creating the chat: {ex.Message}");
            }
        }

        // Add more endpoints as needed
    }
}
