using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Services.ServiceSlots;

public sealed class ServiceSlotService : IServiceSlotService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceSlotService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ServiceSlotDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await MarkExpiredSlotsUnavailableAsync(cancellationToken);

        var slots = await _unitOfWork.Repository<ServiceSlot>().GetAllAsync(cancellationToken);
        return slots
            .OrderBy(slot => slot.Day)
            .ThenBy(slot => slot.TimeFrom)
            .Select(Map)
            .ToList();
    }

    public async Task<ServiceSlotDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await MarkExpiredSlotsUnavailableAsync(cancellationToken);

        var slot = await _unitOfWork.Repository<ServiceSlot>().GetByIdAsync(id, cancellationToken);
        return slot is null ? null : Map(slot);
    }

    public async Task<ServiceSlotDto?> CreateAsync(UpsertServiceSlotRequest request, CancellationToken cancellationToken = default)
    {
        if (!IsTimeRangeValid(request))
        {
            return null;
        }

        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(request.ServiceId, cancellationToken);
        if (service is null)
        {
            return null;
        }

        var slot = new ServiceSlot
        {
            ServiceId = request.ServiceId,
            Day = request.Day,
            TimeFrom = request.TimeFrom,
            TimeTo = request.TimeTo,
            IsAvailable = request.IsAvailable && !IsSlotExpired(request.Day, request.TimeTo)
        };

        await _unitOfWork.Repository<ServiceSlot>().AddAsync(slot, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(slot);
    }

    public async Task<bool> UpdateAsync(int id, UpsertServiceSlotRequest request, CancellationToken cancellationToken = default)
    {
        if (!IsTimeRangeValid(request))
        {
            return false;
        }

        var slot = await _unitOfWork.Repository<ServiceSlot>().GetByIdAsync(id, cancellationToken);
        if (slot is null)
        {
            return false;
        }

        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(request.ServiceId, cancellationToken);
        if (service is null)
        {
            return false;
        }

        slot.ServiceId = request.ServiceId;
        slot.Day = request.Day;
        slot.TimeFrom = request.TimeFrom;
        slot.TimeTo = request.TimeTo;
        slot.IsAvailable = request.IsAvailable && !IsSlotExpired(request.Day, request.TimeTo);

        _unitOfWork.Repository<ServiceSlot>().Update(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var slot = await _unitOfWork.Repository<ServiceSlot>().GetByIdAsync(id, cancellationToken);
        if (slot is null)
        {
            return false;
        }

        _unitOfWork.Repository<ServiceSlot>().Remove(slot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task MarkExpiredSlotsUnavailableAsync(CancellationToken cancellationToken)
    {
        var availableSlots = await _unitOfWork.Repository<ServiceSlot>()
            .FindAsync(slot => slot.IsAvailable, cancellationToken);

        var hasChanges = false;
        foreach (var slot in availableSlots)
        {
            if (!IsSlotExpired(slot.Day, slot.TimeTo))
            {
                continue;
            }

            slot.IsAvailable = false;
            _unitOfWork.Repository<ServiceSlot>().Update(slot);
            hasChanges = true;
        }

        if (hasChanges)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    private static bool IsTimeRangeValid(UpsertServiceSlotRequest request)
    {
        return request.TimeTo > request.TimeFrom;
    }

    private static bool IsSlotExpired(DateOnly day, TimeOnly timeTo)
    {
        var slotDateTime = day.ToDateTime(timeTo);
        return slotDateTime <= DateTime.Now;
    }

    private static ServiceSlotDto Map(ServiceSlot slot)
    {
        return new ServiceSlotDto(
            slot.Id,
            slot.ServiceId,
            slot.Day,
            slot.TimeFrom,
            slot.TimeTo,
            slot.IsAvailable);
    }
}
