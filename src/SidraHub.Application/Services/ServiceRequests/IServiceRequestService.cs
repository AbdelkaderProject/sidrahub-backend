namespace SidraHub.Application.Services.ServiceRequests;

public interface IServiceRequestService
{
    Task<IReadOnlyList<ServiceRequestDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServiceRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceRequestDto?> CreateAsync(CreateServiceRequestRequest request, string? userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
