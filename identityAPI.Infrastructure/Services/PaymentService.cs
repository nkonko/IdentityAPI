using identityAPI.Core.Models;

namespace identityAPI.Infrastructure.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessWebhookAsync(PaymentWebhookDto dto);
    }

    public class PaymentService : IPaymentService
    {
        public async Task<bool> ProcessWebhookAsync(PaymentWebhookDto dto)
        {
            // Placeholder: implement webhook logic
            return true;
        }
    }
}
