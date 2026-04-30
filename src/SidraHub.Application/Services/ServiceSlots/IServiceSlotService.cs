namespace SidraHub.Application.Services.ServiceSlots;

public interface IServiceSlotService
{
    Task<IReadOnlyList<ServiceSlotDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServiceSlotDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceSlotDto?> CreateAsync(UpsertServiceSlotRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertServiceSlotRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
