namespace SidraHub.Application.Services.CompanyProfiles;

public interface ICompanyProfileService
{
    Task<IReadOnlyList<CompanyProfileDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CompanyProfileDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CompanyProfileDto> CreateAsync(UpsertCompanyProfileRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertCompanyProfileRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
