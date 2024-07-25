using System;
using Newtonsoft.Json;

namespace BackEnd.Entities
{
    public class Chats
    {
        [JsonProperty(PropertyName = "id")]
        public string ChatId { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "senderUserId")]
        public string SenderUserId { get; set; }

        [JsonProperty(PropertyName = "receiverUserId")]
        public string ReceiverUserId { get; set; }

        [JsonProperty(PropertyName = "currentTime")]
        public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
