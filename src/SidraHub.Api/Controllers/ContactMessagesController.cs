using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.ContactMessages;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContactMessagesController : ControllerBase
{
    private readonly IContactMessageService _service;

    public ContactMessagesController(IContactMessageService service)
    {
        _service = service;
    }

    // Admin: get all messages
    [HttpGet]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var messages = await _service.GetAllAsync(cancellationToken);
        return Ok(messages);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var message = await _service.GetByIdAsync(id, cancellationToken);
        return message is null ? NotFound() : Ok(message);
    }

    // Public: submit contact form
    [HttpPost]
    public async Task<IActionResult> Create(CreateContactMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = message.Id }, message);
    }

    // Admin: mark as read
    [HttpPatch("{id:int}/read")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var result = await _service.MarkAsReadAsync(id, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    // Admin: delete
    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
