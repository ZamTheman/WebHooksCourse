using System.Net.Http.Headers;
using System.Text.Json;
using AirlineSendAgent.Dtos;

namespace AirlineSendAgent.Client;

public class WebhookClient : IWebhookClient
{
    private readonly IHttpClientFactory httpClientFactory;

    public WebhookClient(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task SendWebHookNotification(FlightDetailChangePayloadDto flightDetailChangePayloadDto)
    {
        var httpClient = httpClientFactory.CreateClient("localhost");
        var request = new HttpRequestMessage(HttpMethod.Post, flightDetailChangePayloadDto.WebhookURI);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var payload = JsonSerializer.Serialize(flightDetailChangePayloadDto);
        request.Content = new StringContent(payload);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        try
        {
            Console.WriteLine($"Trying to send: {payload}");
            using var response = await httpClient.SendAsync(request);
            Console.WriteLine("Successfuly sent");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unsuccessful: " + ex.Message);           
        }
    }
}