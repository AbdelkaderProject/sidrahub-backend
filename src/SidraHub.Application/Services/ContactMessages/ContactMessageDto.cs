namespace SidraHub.Application.Services.ContactMessages;

public sealed record ContactMessageDto(
    int Id,
    string Name,
    string Email,
    string Phone,
    string Subject,
    string Message,
    bool IsRead,
    DateTime CreatedDateTime);

public sealed record CreateContactMessageRequest(
    string Name,
    string Email,
    string Phone,
    string Subject,
    string Message);
