namespace SidraHub.Application.Services.AboutPageContents;

public interface IAboutPageContentService
{
    Task<IReadOnlyList<AboutPageContentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AboutPageContentDto?> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task<AboutPageContentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AboutPageContentDto> CreateAsync(UpsertAboutPageContentRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpsertAboutPageContentRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
