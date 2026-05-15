using Microsoft.EntityFrameworkCore;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;
using SidraHub.Domain.Enums;

namespace SidraHub.Application.Services.ServiceRequests;

public sealed class ServiceRequestService : IServiceRequestService
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public ServiceRequestService(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<IReadOnlyList<ServiceRequestDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var requests = await _context.ServiceRequests
            .AsNoTracking()
            .OrderByDescending(request => request.RequestDate)
            .ThenByDescending(request => request.RequestTime)
            .ThenByDescending(request => request.Id)
            .Select(request => new ServiceRequestQueryResult(
                request.Id,
                EF.Property<string?>(request, nameof(ServiceRequest.Code)) ?? string.Empty,
                request.ServiceId,
                request.Service != null ? EF.Property<string?>(request.Service, nameof(Service.NameAr)) ?? string.Empty : string.Empty,
                request.Service != null ? EF.Property<string?>(request.Service, nameof(Service.NameEn)) ?? string.Empty : string.Empty,
                request.ServiceSlotId,
                EF.Property<string?>(request, nameof(ServiceRequest.CustomerName)) ?? string.Empty,
                EF.Property<string?>(request, nameof(ServiceRequest.CustomerEmail)) ?? string.Empty,
                EF.Property<string?>(request, nameof(ServiceRequest.CustomerPhone)) ?? string.Empty,
                EF.Property<string?>(request, nameof(ServiceRequest.Description)) ?? string.Empty,
                EF.Property<DateOnly?>(request, nameof(ServiceRequest.RequestDate)) ?? default,
                EF.Property<TimeOnly?>(request, nameof(ServiceRequest.RequestTime)) ?? default,
                EF.Property<int?>(request, nameof(ServiceRequest.RequestStatus)) ?? (int)ServiceRequestStatus.Submit))
            .ToListAsync(cancellationToken);

        return requests.Select(Map).ToList();
    }

    public async Task<ServiceRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var request = await _context.ServiceRequests
            .AsNoTracking()
            .Where(request => request.Id == id)
            .Select(request => new ServiceRequestQueryResult(
                request.Id,
                EF.Property<string?>(request, nameof(ServiceRequest.Code)) ?? string.Empty,
                request.ServiceId,
                request.Service != null ? EF.Property<string?>(request.Service, nameof(Service.NameAr)) ?? string.Empty : string.Empty,
                request.Service != null ? EF.Property<string?>(request.Service, nameof(Service.NameEn)) ?? string.Empty : string.Empty,
                request.ServiceSlotId,
                EF.Property<string?>(request, nameof(ServiceRequest.CustomerName)) ?? string.Empty,
                EF.Property<string?>(request, nameof(ServiceRequest.CustomerEmail)) ?? string.Empty,
                EF.Property<string?>(request, nameof(ServiceRequest.CustomerPhone)) ?? string.Empty,
                EF.Property<string?>(request, nameof(ServiceRequest.Description)) ?? string.Empty,
                EF.Property<DateOnly?>(request, nameof(ServiceRequest.RequestDate)) ?? default,
                EF.Property<TimeOnly?>(request, nameof(ServiceRequest.RequestTime)) ?? default,
                EF.Property<int?>(request, nameof(ServiceRequest.RequestStatus)) ?? (int)ServiceRequestStatus.Submit))
            .FirstOrDefaultAsync(cancellationToken);

        return request is null ? null : Map(request);
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

        // Notify admins
        await _notificationService.NotifyAdminsAsync(
            "📅 حجز استشارة جديد",
            $"العميل {request.CustomerName} حجز استشارة في خدمة \"{service.NameAr}\" بتاريخ {slot.Day:yyyy-MM-dd} الساعة {slot.TimeFrom:HH:mm}",
            cancellationToken);

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

    private static ServiceRequestDto Map(ServiceRequestQueryResult request)
    {
        var requestStatus = Enum.IsDefined(typeof(ServiceRequestStatus), request.RequestStatus)
            ? (ServiceRequestStatus)request.RequestStatus
            : ServiceRequestStatus.Submit;

        return new ServiceRequestDto(
            request.Id,
            request.Code,
            request.ServiceId,
            request.ServiceNameAr,
            request.ServiceNameEn,
            request.ServiceSlotId,
            request.CustomerName,
            request.CustomerEmail,
            request.CustomerPhone,
            request.Description,
            request.RequestDate,
            request.RequestTime,
            request.RequestStatus,
            requestStatus.ToString());
    }

    private sealed record ServiceRequestQueryResult(
        int Id,
        string Code,
        int ServiceId,
        string ServiceNameAr,
        string ServiceNameEn,
        int ServiceSlotId,
        string CustomerName,
        string CustomerEmail,
        string CustomerPhone,
        string Description,
        DateOnly RequestDate,
        TimeOnly RequestTime,
        int RequestStatus);

    private static string GenerateCode()
    {
        return $"SR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
