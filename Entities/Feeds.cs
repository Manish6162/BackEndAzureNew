
namespace BackEnd.Entities

{
    using Newtonsoft.Json;

    public class Feed
    {
        [JsonProperty(PropertyName = "id")]
        public string FeedId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "currentTime")]
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "uploaderUserId")]
        public string UploaderUserId { get; set; }

        [JsonProperty(PropertyName = "uploaderUserName")]
        public string UploaderUserName{ get; set; }

        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; } // Add UserId property

        [JsonProperty(PropertyName = "profilePicUrl")]
        public string ProfilePicUrl { get; set; } // Add ProfilePicUrl property
    }

}

