namespace BackEnd.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ProfilePicUrl { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string BrowserInfo { get; set; }
        public string OperatingSystem { get; set; }
        public string DeviceId { get; set; } // Added DeviceId
    }
}
