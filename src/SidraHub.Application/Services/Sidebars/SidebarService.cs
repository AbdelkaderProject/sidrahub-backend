using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.Sidebars;

public sealed class SidebarService : ISidebarService
{
    private readonly IUnitOfWork _unitOfWork;

    public SidebarService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<SidebarDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sidebars = await _unitOfWork.Repository<Sidebar>().GetAllAsync(cancellationToken);
        return sidebars.OrderBy(sidebar => sidebar.Id).Select(Map).ToList();
    }

    public async Task<SidebarDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var sidebar = await _unitOfWork.Repository<Sidebar>().GetByIdAsync(id, cancellationToken);
        return sidebar is null ? null : Map(sidebar);
    }

    public async Task<SidebarDto?> CreateAsync(UpsertSidebarRequest request, CancellationToken cancellationToken = default)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(request.ServiceId, cancellationToken);
        if (service is null)
        {
            return null;
        }

        var sidebar = new Sidebar
        {
            ServiceId = request.ServiceId,
            TitleAr = request.TitleAr,
            TitleEn = request.TitleEn,
            DescriptionAr = request.DescriptionAr,
            DescriptionEn = request.DescriptionEn,
            Image = request.Image
        };

        await _unitOfWork.Repository<Sidebar>().AddAsync(sidebar, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(sidebar);
    }

    public async Task<bool> UpdateAsync(int id, UpsertSidebarRequest request, CancellationToken cancellationToken = default)
    {
        var sidebar = await _unitOfWork.Repository<Sidebar>().GetByIdAsync(id, cancellationToken);
        if (sidebar is null)
        {
            return false;
        }

        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(request.ServiceId, cancellationToken);
        if (service is null)
        {
            return false;
        }

        sidebar.ServiceId = request.ServiceId;
        sidebar.TitleAr = request.TitleAr;
        sidebar.TitleEn = request.TitleEn;
        sidebar.DescriptionAr = request.DescriptionAr;
        sidebar.DescriptionEn = request.DescriptionEn;
        sidebar.Image = request.Image;

        _unitOfWork.Repository<Sidebar>().Update(sidebar);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var sidebar = await _unitOfWork.Repository<Sidebar>().GetByIdAsync(id, cancellationToken);
        if (sidebar is null)
        {
            return false;
        }

        _unitOfWork.Repository<Sidebar>().Remove(sidebar);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static SidebarDto Map(Sidebar sidebar)
    {
        return new SidebarDto(
            sidebar.Id,
            sidebar.ServiceId,
            sidebar.TitleAr,
            sidebar.TitleEn,
            sidebar.DescriptionAr,
            sidebar.DescriptionEn,
            sidebar.Image);
    }
}
