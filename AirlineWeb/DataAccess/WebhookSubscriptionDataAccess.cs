using System.Data;
using Dapper;
using System.Data.SqlClient;
using AirlineWeb.Models;

namespace AirlineWeb.DataAccess;

public class WebhookSubscriptionDataAccess
{
    private readonly IConfiguration configuration;

    public WebhookSubscriptionDataAccess(IConfiguration config)
    {
        configuration = config;
    }

    public async Task<IEnumerable<WebhookSubscription>> GetAll()
    {
        var sql = "SELECT * FROM dbo.WebhookSubscriptions";

        using IDbConnection connection = new SqlConnection(configuration.GetConnectionString("AirlineConnection"));
        
        return await connection.QueryAsync<WebhookSubscription>(sql);
    }
}
