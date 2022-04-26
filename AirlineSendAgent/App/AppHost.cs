using System.Text;
using System.Text.Json;
using AirlineSendAgent.Client;
using AirlineSendAgent.Dtos;
using AirlineWeb.DataAccess;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AirlineSendAgent.App;

public class AppHost : IAppHost
{
    private readonly WebhookSubscriptionDataAccess webhookSubscriptionDataAccess;
    private readonly IWebhookClient webhookClient;

    public AppHost(WebhookSubscriptionDataAccess webhookSubscriptionDataAccess, IWebhookClient webhookClient)
    {
        this.webhookSubscriptionDataAccess = webhookSubscriptionDataAccess;
        this.webhookClient = webhookClient;
    }
    public void Run()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", Port= 5672 };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        var queueName = channel.QueueDeclare();
        channel.QueueBind(queue: queueName, exchange: "trigger", routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        Console.WriteLine("Listening on the message bus...");

        consumer.Received += async (ModuleHandle, ea) =>
        {
            Console.WriteLine("Event triggered");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            var message = JsonSerializer.Deserialize<NotificationMessageDto>(notificationMessage);
            if (message is null || message.WebhookType is null)
            {
                Console.WriteLine("Failed message");
                return;
            }

            var webhookToSend = new FlightDetailChangePayloadDto()
            {
                WebhookType = message.WebhookType,
                WebhookURI = string.Empty,
                Secret = string.Empty,
                Publisher = string.Empty,
                OldPrice = message.OldPrice,
                NewPrice = message.NewPrice,
                FlightCode = message.FlightCode
            };

            var subscribers = await webhookSubscriptionDataAccess.GetByType(message.WebhookType);
            Console.WriteLine($"Nr subscribers: {subscribers.Count()}");
            foreach (var sub in subscribers)
            {
                webhookToSend.WebhookURI = sub.WebhookUri;
                webhookToSend.Secret = sub.Secret;
                webhookToSend.Publisher = sub.WebhookPublisher;

                try
                {
                    await webhookClient.SendWebHookNotification(webhookToSend);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send message to: { webhookToSend.WebhookURI }, {ex.Message}");
                }
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.ReadLine();
    }
}