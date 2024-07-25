
namespace BackEnd.Entities
{

    using Newtonsoft.Json;

    public class Notification
    {
        [JsonProperty(PropertyName = "id")]
        public string FeedId { get; set; }

        [JsonProperty(PropertyName = "uploaderUserId")]
        public string UploaderUserId { get; set; }

        [JsonProperty(PropertyName = "likeByUserId")]
        public string LikeByUserId { get; set; }

        [JsonProperty(PropertyName = "commentedByUserId")]
        public string CommentedByUserId { get; set; }

        [JsonProperty(PropertyName = "seenByUser")]
        public bool SeenByUser { get; set; }
    }


}


