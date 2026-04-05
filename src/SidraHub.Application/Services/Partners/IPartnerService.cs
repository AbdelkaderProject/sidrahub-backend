namespace SidraHub.Application.Services.Partners;

public interface IPartnerService
{
    Task<IReadOnlyList<PartnerDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PartnerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PartnerDto?> CreateAsync(UpsertPartnerRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertPartnerRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
