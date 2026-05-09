using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.Branches;

public sealed class BranchService : IBranchService
{
    private readonly IUnitOfWork _unitOfWork;

    public BranchService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<BranchDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var branches = await _unitOfWork.Repository<Branch>().GetAllAsync(cancellationToken);
        return branches.OrderBy(entity => entity.Id).Select(Map).ToList();
    }

    public async Task<BranchDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id, cancellationToken);
        return branch is null ? null : Map(branch);
    }

    public async Task<BranchDto?> CreateAsync(UpsertBranchRequest request, CancellationToken cancellationToken = default)
    {
        var branch = new Branch
        {
            NameEn = request.NameEn,
            NameAr = request.NameAr,
            AddressEn = request.AddressEn,
            AddressAr = request.AddressAr,
            PhoneNumber = request.PhoneNumber
        };

        await _unitOfWork.Repository<Branch>().AddAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(branch);
    }

    public async Task<bool> UpdateAsync(int id, UpsertBranchRequest request, CancellationToken cancellationToken = default)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id, cancellationToken);
        if (branch is null)
        {
            return false;
        }
        branch.NameEn = request.NameEn;
        branch.NameAr = request.NameAr;
        branch.AddressEn = request.AddressEn;
        branch.AddressAr = request.AddressAr;
        branch.PhoneNumber = request.PhoneNumber;

        _unitOfWork.Repository<Branch>().Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id, cancellationToken);
        if (branch is null)
        {
            return false;
        }

        _unitOfWork.Repository<Branch>().Remove(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static BranchDto Map(Branch entity)
    {
        return new BranchDto(
            entity.Id,
            entity.NameEn,
            entity.NameAr,
            entity.AddressEn,
            entity.AddressAr,
            entity.PhoneNumber);
    }
}
