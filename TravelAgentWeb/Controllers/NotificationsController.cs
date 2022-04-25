using Microsoft.AspNetCore.Mvc;
using TravelAgentWeb.DataAccess;
using TravelAgentWeb.Dtos;

namespace TravelAgentWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly TravelAgentDataAccess travelAgentDataAccess;

    public NotificationsController(TravelAgentDataAccess travelAgentDataAccess)
    {
        this.travelAgentDataAccess = travelAgentDataAccess;
    }

    [HttpPost]
    public async Task<ActionResult> FlightChanged(FlightDetailUpdateDto flightDetailUpdateDto)
    {
        if (flightDetailUpdateDto.Secret is null)
            return NoContent();

        Console.WriteLine($"Webhook received from: {flightDetailUpdateDto.Publisher}");
        var secretModel = await travelAgentDataAccess.GetBySecret(flightDetailUpdateDto.Secret);
        
        if (secretModel is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid secret");
            Console.ResetColor();
            return NoContent();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Valid webhook");
        Console.WriteLine($"Old price: {flightDetailUpdateDto.OldPrice}");
        Console.WriteLine($"New price: {flightDetailUpdateDto.NewPrice}");
        Console.ResetColor();
        return Ok();
    }
}