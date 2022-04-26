using AirlineWeb.DataAccess;
using AirlineWeb.Dtos;
using AirlineWeb.MessageBus;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirlineWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly FlightsDataAccess flightsDataAccess;
    private readonly IMapper mapper;
    private readonly IMessageBusClient messageBusClient;

    public FlightsController(FlightsDataAccess flightsDataAccess, IMapper mapper, IMessageBusClient messageBusClient)
    {
        this.flightsDataAccess = flightsDataAccess;
        this.mapper = mapper;
        this.messageBusClient = messageBusClient;
    }

    [HttpGet("{id}", Name = nameof(GetFlightDetailsById))]
    public async Task<ActionResult<WebhookSubscriptionReadDto>> GetFlightDetailsById(int id)
    {
        var flight = await flightsDataAccess.GetById(id);
        if (flight is null)
            return NotFound();

        return Ok(mapper.Map<FlightDetailReadDto>(flight));
    }

    [HttpPost]
    public async Task<ActionResult<FlightDetailReadDto>> Create(FlightDetailCreateDto flightDetailCreateDto)
    {
        if (flightDetailCreateDto.FlightCode is null)
            return NoContent();
            
        FlightDetail flightDetail = await flightsDataAccess.GetByFlightCode(flightDetailCreateDto.FlightCode);
        if (flightDetail is not null)
            return NoContent();

        flightDetail = mapper.Map<FlightDetail>(flightDetailCreateDto);
        try
        {
            flightDetail.Id = await flightsDataAccess.Create(flightDetail);
            return CreatedAtRoute(nameof(GetFlightDetailsById), new { id = flightDetail.Id }, flightDetail);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update(FlightDetailUpdateDto flightDetailUpdateDto)
    {
        if (flightDetailUpdateDto.FlightCode is null)
            return NoContent();
            
        FlightDetail flightDetail = await flightsDataAccess.GetById(flightDetailUpdateDto.Id);
        if (flightDetail is null)
            return NotFound();

        var oldPrice = flightDetail.Price;

        mapper.Map(flightDetailUpdateDto, flightDetail);

        try
        {
            var succesfulUpdate = await flightsDataAccess.Update(flightDetail);
            if (oldPrice != flightDetail.Price)
            {
                Console.WriteLine("Price change, placing message on bus");
                var message = new NotificationMessageDto
                {
                    WebhookType = "pricechange",
                    OldPrice = oldPrice,
                    NewPrice = flightDetail.Price,
                    FlightCode = flightDetail.FlightCode
                };

                messageBusClient.SendMessage(message);
            }
            else
            {
                Console.WriteLine("No price change");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
