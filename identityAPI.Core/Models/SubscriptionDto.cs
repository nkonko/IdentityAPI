namespace identityAPI.Core.Models
{
    public class SubscriptionDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SubscriptionCreateDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
    }

    public class SubscriptionUpdateDto
    {
        public string Plan { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
    }

    public class PaymentWebhookDto
    {
        public string Event { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
    }
}
