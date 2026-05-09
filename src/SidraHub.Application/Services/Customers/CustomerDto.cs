namespace SidraHub.Application.Services.Customers;

public sealed record CustomerDto(
    int Id,
    string NameAr,
    string NameEn,
    string? Logo);
