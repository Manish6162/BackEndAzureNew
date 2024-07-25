using Microsoft.AspNetCore.Mvc;
using BackEnd.Entities;
using System;
using System.Collections.Generic;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        // POST: api/Likes
        [HttpPost("{feedId}/like")]


        // Simulated method to fetch feed from data source
        private Feed FetchFeedFromDataSource(string feedId)
        {
            // You would typically fetch feed from a database or another data source
            // Here, for demonstration, I'm just returning a mock feed
            return new Feed
            {
                FeedId = feedId,
                Likes = 0, // Initialize likes counts
            };
        }
    }
}
