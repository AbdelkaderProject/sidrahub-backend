namespace SidraHub.Application.Services.ServiceRequests;

public sealed class CreateServiceRequestRequest
{
    public int ServiceId { get; set; }
    public int ServiceSlotId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
