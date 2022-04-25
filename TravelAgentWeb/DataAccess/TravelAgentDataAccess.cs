using System.Data;
using Dapper;
using System.Data.SqlClient;
using TravelAgentWeb.Models;

namespace TravelAgentWeb.DataAccess;

public class TravelAgentDataAccess
{
    private const string connectionString = "Server=localhost,1433;Database=TravelAgentWebDB;User Id=sa;Password=pa55w0rd!";

    public Task<IEnumerable<WebhookSecret>> GetAll()
    {
        var sql = "SELECT * FROM dbo.SubscriptionSecrets";

        using IDbConnection connection = new SqlConnection(connectionString);
   
        return connection.QueryAsync<WebhookSecret>(sql);
    }

    public async Task<WebhookSecret> GetById(int id)
    {
        var sql = "SELECT * FROM dbo.SubscriptionSecrets WHERE Id = @Id";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<WebhookSecret>(sql, new { id });
    }

    public async Task<WebhookSecret> GetByPublisher(string publisher)
    {
        var sql = "SELECT * FROM dbo.SubscriptionSecrets WHERE Publisher = @Publisher";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<WebhookSecret>(sql, new { publisher });
    }

    public async Task<WebhookSecret> GetBySecret(string secret)
    {
        var sql = "SELECT * FROM dbo.SubscriptionSecrets WHERE [Secret] = @Secret";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<WebhookSecret>(sql, new { secret });
    }

    public async Task<int> Create(WebhookSecret webhookSecret)
    {
        var sql = @"
            INSERT INTO
            dbo.SubscriptionSecretsSubscriptionSecrets
            ([Secret], Publisher)
            OUTPUT INSERTED.Id
            VALUES
            (@Secret, @Publisher)";
            
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QuerySingleAsync<int>(sql, new {
            webhookSecret.Secret,
            webhookSecret.Publisher
        });
    }
}
