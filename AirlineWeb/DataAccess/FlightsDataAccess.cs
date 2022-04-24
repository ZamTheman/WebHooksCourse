using System.Data;
using Dapper;
using System.Data.SqlClient;
using AirlineWeb.Models;

namespace AirlineWeb.DataAccess;

public class FlightsDataAccess
{
    private const string connectionString = "Server=localhost,1433;Database=AirlineWebDB;User Id=sa;Password=pa55w0rd!";

    public Task<IEnumerable<FlightDetail>> GetAll()
    {
        var sql = "SELECT * FROM dbo.FlightDetails";

        using IDbConnection connection = new SqlConnection(connectionString);
   
        return connection.QueryAsync<FlightDetail>(sql);
    }

    public async Task<FlightDetail> GetById(int id)
    {
        var sql = "SELECT * FROM dbo.FlightDetails WHERE Id = @id";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<FlightDetail>(sql, new { id });
    }

    public async Task<FlightDetail> GetByFlightCode(string flightCode)
    {
        var sql = "SELECT * FROM dbo.FlightDetails WHERE FlightCode = @FlightCode";
        
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QueryFirstOrDefaultAsync<FlightDetail>(sql, new { flightCode });
    }

    public async Task<int> Create(FlightDetail flightDetail)
    {
        var sql = @"
            INSERT INTO
            dbo.FlightDetails
            (FlightCode, Price)
            OUTPUT INSERTED.Id
            VALUES
            (@FlightCode, @Price)";
            
        using IDbConnection connection = new SqlConnection(connectionString);

        return await connection.QuerySingleAsync<int>(sql, new {
            flightDetail.FlightCode,
            flightDetail.Price
        });
    }

    public async Task<bool> Update(FlightDetail flightDetail)
    {
        var sql = @"
            UPDATE FlightDetails
            SET FlightCode = @FlightCode, Price = @Price
            WHERE Id = @Id
        ";

        using IDbConnection connection = new SqlConnection(connectionString);

        var affectedRowCount = await connection.ExecuteAsync(sql, new {
            flightDetail.Id,
            flightDetail.FlightCode,
            flightDetail.Price
        });

        return affectedRowCount == 1;
    }
}
