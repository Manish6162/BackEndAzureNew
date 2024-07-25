using System;
using Newtonsoft.Json;

namespace BackEnd.Entities
{
    public class Comments
    {
        [JsonProperty(PropertyName = "id")]
        public string CommentId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "commentorUserId")]
        public string commentorUserId { get; set; }

        [JsonProperty(PropertyName = "currentTime")]
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
