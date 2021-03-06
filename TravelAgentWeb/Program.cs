using TravelAgentWeb.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        RegisterDI(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void RegisterDI(IServiceCollection services)
    {
        // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // services.AddTransient<WebhookSubscriptionDataAccess, WebhookSubscriptionDataAccess>();
        services.AddTransient<TravelAgentDataAccess, TravelAgentDataAccess>();
    }
}