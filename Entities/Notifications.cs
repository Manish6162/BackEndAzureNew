using System;
using Newtonsoft.Json;

namespace BackEnd.Entities
{
    public class Notifications
    {
        [JsonProperty(PropertyName = "id")]
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "feedId")]
        public string FeedId { get; set; }

        [JsonProperty(PropertyName = "uploaderUserId")]
        public string UploaderUserId { get; set; }

        [JsonProperty(PropertyName = "likedByUserId")]
        public string LikedByUserId { get; set; }

        [JsonProperty(PropertyName = "commentedByUserId")]
        public string CommentedByUserId { get; set; }

        [JsonProperty(PropertyName = "currentTime")]
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "isSeen")]
        public bool IsSeen { get; set; } 
    }
}
