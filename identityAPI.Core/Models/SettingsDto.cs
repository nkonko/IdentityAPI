namespace identityAPI.Core.Models
{
    public class SettingsDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string SupportEmail { get; set; } = string.Empty;
    }

    public class SettingsUpdateDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string SupportEmail { get; set; } = string.Empty;
    }
}
