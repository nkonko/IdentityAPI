using identityAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using identityAPI.Infrastructure.Services;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] PaymentWebhookDto dto)
        {
            var result = await _paymentService.ProcessWebhookAsync(dto);
            return result ? Ok() : BadRequest();
        }
    }
}
