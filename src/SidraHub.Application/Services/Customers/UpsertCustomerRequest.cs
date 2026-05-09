namespace SidraHub.Application.Services.Customers;

public sealed class UpsertCustomerRequest
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Logo { get; set; }
}
