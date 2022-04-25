namespace TravelAgentWeb.Models;

public class WebhookSecret
{
    public int Id { get; set; }
    public string? Secret { get; set; }
    public string? Publisher { get; set; }
}