using AirlineSendAgent.App;
using AirlineSendAgent.Client;
using AirlineWeb.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirlineSendAgent;

public class Program
{
    private static void Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddSingleton<IAppHost, AppHost>();
                services.AddSingleton<IWebhookClient, WebhookClient>();
                services.AddTransient<WebhookSubscriptionDataAccess, WebhookSubscriptionDataAccess>();
                services.AddHttpClient("localhost").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                });;
            }).Build();

        var appService = host.Services.GetService<IAppHost>();
        if (appService is not null)
            appService.Run();
    }
}