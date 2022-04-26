using AirlineSendAgent.Dtos;

namespace AirlineSendAgent.Client;

public interface IWebhookClient
{
    Task SendWebHookNotification(FlightDetailChangePayloadDto flightDetailChangePayloadDto);
}