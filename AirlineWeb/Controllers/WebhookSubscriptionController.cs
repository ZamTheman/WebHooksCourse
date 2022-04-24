using AirlineWeb.DataAccess;
using AirlineWeb.Dtos;
using AirlineWeb.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirlineWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebhookSubscriptionController : ControllerBase
{
    private readonly WebhookSubscriptionDataAccess webhookSubscriptionDataAccess;
    private readonly IMapper mapper;
    private readonly ILogger<WebhookSubscriptionController> logger;

    public WebhookSubscriptionController(WebhookSubscriptionDataAccess webhookSubscriptionDataAccess, IMapper mapper, ILogger<WebhookSubscriptionController> logger)
    {
        this.webhookSubscriptionDataAccess = webhookSubscriptionDataAccess;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet("{secret}", Name = nameof(GetSubscriptionBySecret))]
    public async Task<ActionResult<WebhookSubscriptionReadDto>> GetSubscriptionBySecret(string secret)
    {
        var subscription = await webhookSubscriptionDataAccess.GetBySecret(secret);
        if (subscription is null)
            return NotFound();

        return Ok(mapper.Map<WebhookSubscriptionReadDto>(subscription));
    }

    [HttpPost]
    public async Task<ActionResult<WebhookSubscriptionReadDto>> CreateSubscription(WebhookSubscriptionCreateDto webhookSubscriptionCreateDto)
    {
        if (webhookSubscriptionCreateDto.WebhookUri is null)
            return NoContent();
            
        WebhookSubscription subscription = await webhookSubscriptionDataAccess.GetByUrl(webhookSubscriptionCreateDto.WebhookUri);
        if (subscription is not null)
            return NoContent();

        subscription = mapper.Map<WebhookSubscription>(webhookSubscriptionCreateDto);
        subscription.Secret = Guid.NewGuid().ToString();
        subscription.WebhookPublisher = "SAS";
        try
        {
            subscription.Id = await webhookSubscriptionDataAccess.Create(subscription);
            return CreatedAtRoute(nameof(GetSubscriptionBySecret), new { secret = subscription.Secret }, subscription);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}