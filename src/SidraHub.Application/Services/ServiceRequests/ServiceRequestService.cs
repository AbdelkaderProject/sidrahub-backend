using Microsoft.EntityFrameworkCore;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;
using SidraHub.Domain.Enums;

namespace SidraHub.Application.Services.ServiceRequests;

public sealed class ServiceRequestService : IServiceRequestService
{
    private readonly IApplicationDbContext _context;

    public ServiceRequestService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ServiceRequestDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ServiceRequests
            .AsNoTracking()
            .Include(request => request.Service)
            .OrderByDescending(request => request.RequestDate)
            .ThenByDescending(request => request.RequestTime)
            .ThenByDescending(request => request.Id)
            .Select(request => Map(request, request.Service))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceRequests
            .AsNoTracking()
            .Include(request => request.Service)
            .Where(request => request.Id == id)
            .Select(request => Map(request, request.Service))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ServiceRequestDto?> CreateAsync(
        CreateServiceRequestRequest request,
        string? userId,
        CancellationToken cancellationToken = default)
    {
        var service = await _context.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(entry => entry.Id == request.ServiceId, cancellationToken);
        if (service is null)
        {
            return null;
        }

        var slot = await _context.ServiceSlots
            .FirstOrDefaultAsync(entry =>
                entry.Id == request.ServiceSlotId
                && entry.ServiceId == request.ServiceId
                && entry.IsAvailable,
                cancellationToken);
        if (slot is null)
        {
            return null;
        }

        var serviceRequest = new ServiceRequest
        {
            Code = GenerateCode(),
            UserId = string.IsNullOrWhiteSpace(userId) ? null : userId,
            ServiceId = service.Id,
            ServiceSlotId = slot.Id,
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone.Trim(),
            Description = request.Description.Trim(),
            RequestDate = slot.Day,
            RequestTime = slot.TimeFrom,
            RequestStatus = ServiceRequestStatus.Submit
        };

        slot.IsAvailable = false;

        await _context.ServiceRequests.AddAsync(serviceRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Map(serviceRequest, service);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = await _context.ServiceRequests
            .Include(entry => entry.ServiceSlot)
            .FirstOrDefaultAsync(entry => entry.Id == id, cancellationToken);
        if (request is null)
        {
            return false;
        }

        request.ServiceSlot.IsAvailable = true;
        _context.ServiceRequests.Remove(request);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static ServiceRequestDto Map(ServiceRequest request, Service service)
    {
        return new ServiceRequestDto(
            request.Id,
            request.Code,
            request.ServiceId,
            service.NameAr,
            service.NameEn,
            request.ServiceSlotId,
            request.CustomerName,
            request.CustomerEmail,
            request.CustomerPhone,
            request.Description,
            request.RequestDate,
            request.RequestTime,
            (int)request.RequestStatus,
            request.RequestStatus.ToString());
    }

    private static string GenerateCode()
    {
        return $"SR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
