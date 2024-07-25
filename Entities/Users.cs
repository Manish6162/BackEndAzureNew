using Newtonsoft.Json;

namespace BackEnd.Entities
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("profilePicUrl")]
        public string ProfilePicUrl { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty("browserInfo")]
        public string BrowserInfo { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
