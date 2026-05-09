using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.ServiceRequests;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServiceRequestsController : ControllerBase
{
    private readonly IServiceRequestService _serviceRequestService;

    public ServiceRequestsController(IServiceRequestService serviceRequestService)
    {
        _serviceRequestService = serviceRequestService;
    }

    [HttpGet]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var requests = await _serviceRequestService.GetAllAsync(cancellationToken);
        return Ok(requests);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var request = await _serviceRequestService.GetByIdAsync(id, cancellationToken);
        return request is null ? NotFound() : Ok(request);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(CreateServiceRequestRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var created = await _serviceRequestService.CreateAsync(request, userId, cancellationToken);
        return created is null
            ? BadRequest(new { errorMessage = "The selected service or time slot is no longer available." })
            : CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceRequestService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
