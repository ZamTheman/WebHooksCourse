namespace AirlineWeb.Models
{
    public class WebhookSubscription
    {
        public int Id { get; set; }
        public string WebhookUri { get; set; } = "";
        public string Secret { get; set; } = "";
        public string WebhookType { get; set; } = "";
        public string WebhookPublisher { get; set; } = "";
    }
}
