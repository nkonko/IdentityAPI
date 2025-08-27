using identityAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using identityAPI.Infrastructure.Services;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetSubscriptions()
            => Ok(await _subscriptionService.GetSubscriptionsAsync());

        [HttpPost]
        public async Task<ActionResult<SubscriptionDto>> CreateSubscription([FromBody] SubscriptionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sub = await _subscriptionService.CreateSubscriptionAsync(dto);
            return CreatedAtAction(nameof(GetSubscriptions), new { id = sub.Id }, sub);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(string id, [FromBody] SubscriptionUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _subscriptionService.UpdateSubscriptionAsync(id, dto);
            return result ? NoContent() : NotFound();
        }
    }
}
