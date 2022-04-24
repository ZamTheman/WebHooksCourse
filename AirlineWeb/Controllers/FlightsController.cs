using AirlineWeb.DataAccess;
using AirlineWeb.Dtos;
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
    private readonly ILogger<WebhookSubscriptionController> logger;

    public FlightsController(FlightsDataAccess flightsDataAccess, IMapper mapper, ILogger<WebhookSubscriptionController> logger)
    {
        this.flightsDataAccess = flightsDataAccess;
        this.mapper = mapper;
        this.logger = logger;
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
    public async Task<ActionResult<FlightDetailReadDto>> Update(FlightDetailUpdateDto flightDetailUpdateDto)
    {
        if (flightDetailUpdateDto.FlightCode is null)
            return NoContent();
            
        FlightDetail flightDetail = await flightsDataAccess.GetById(flightDetailUpdateDto.Id);
        if (flightDetail is null)
            return NotFound();

        mapper.Map(flightDetailUpdateDto, flightDetail);
        try
        {
            var succesfulUpdate = await flightsDataAccess.Update(flightDetail);
            if (succesfulUpdate)
                return CreatedAtRoute(nameof(GetFlightDetailsById), new { id = flightDetail.Id }, flightDetail);

            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}