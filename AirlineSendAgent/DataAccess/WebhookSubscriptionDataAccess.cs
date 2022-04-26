using System.Data;
using Dapper;
using System.Data.SqlClient;
using AirlineSendAgent.Models;

namespace AirlineWeb.DataAccess;

public class WebhookSubscriptionDataAccess
{
    private const string connectionString = "Server=localhost,1433;Database=AirlineWebDB;User Id=sa;Password=pa55w0rd!";

    public Task<IEnumerable<WebhookSubscription>> GetAll()
    {
        var sql = "SELECT * FROM dbo.WebhookSubscriptions";

        using IDbConnection connection = new SqlConnection(connectionString);
   
        return connection.QueryAsync<WebhookSubscription>(sql);
    }

    public async Task<WebhookSubscription> GetByUrl(string webhookUri)
    {
        var sql = "SELECT * FROM dbo.WebhookSubscriptions WHERE WebhookUri = @webhookUri";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<WebhookSubscription>(sql, new { webhookUri });
    }

    public async Task<WebhookSubscription> GetBySecret(string secret)
    {
       var sql = "SELECT * FROM dbo.WebhookSubscriptions WHERE Secret = @secret";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<WebhookSubscription>(sql, new { secret });
    }

    public async Task<IEnumerable<WebhookSubscription>> GetByType(string type)
    {
        var sql = "SELECT * FROM dbo.WebhookSubscriptions WHERE WebhookType = @type";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryAsync<WebhookSubscription>(sql, new { type });
    }

    public async Task<int> Create(WebhookSubscription webhookSubscription)
    {
        var sql = @"
            INSERT INTO
            dbo.WebhookSubscriptions
            (WebhookUri, [Secret], WebhookType, WebhookPublisher)
            OUTPUT INSERTED.Id
            VALUES
            (@WebhookUri, @secret, @WebhookType, @WebhookPublisher)";
            
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QuerySingleAsync<int>(sql, new {
            webhookSubscription.WebhookUri,
            webhookSubscription.Secret,
            webhookSubscription.WebhookType,
            webhookSubscription.WebhookPublisher
        });
    }
}
