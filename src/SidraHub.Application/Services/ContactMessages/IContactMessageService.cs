namespace SidraHub.Application.Services.ContactMessages;

public interface IContactMessageService
{
    Task<IReadOnlyList<ContactMessageDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ContactMessageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ContactMessageDto> CreateAsync(CreateContactMessageRequest request, CancellationToken cancellationToken = default);
    Task<bool> MarkAsReadAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
