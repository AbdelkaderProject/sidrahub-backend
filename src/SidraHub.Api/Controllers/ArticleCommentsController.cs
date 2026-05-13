using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SidraHub.Application.Services.ArticleComments;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ArticleCommentsController : ControllerBase
{
    private readonly IArticleCommentService _articleCommentService;

    public ArticleCommentsController(IArticleCommentService articleCommentService)
    {
        _articleCommentService = articleCommentService;
    }

    // Admin: all comments with status
    [HttpGet]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var comments = await _articleCommentService.GetAllAsync(cancellationToken);
        return Ok(comments);
    }

    // Public: approved comments only
    [HttpGet("article/{articleId:int}")]
    public async Task<IActionResult> GetByArticleId(int articleId, CancellationToken cancellationToken)
    {
        var comments = await _articleCommentService.GetByArticleIdAsync(articleId, cancellationToken);
        return Ok(comments);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var comment = await _articleCommentService.GetByIdAsync(id, cancellationToken);
        return comment is null ? NotFound() : Ok(comment);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(UpsertArticleCommentRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var comment = await _articleCommentService.CreateAsync(request, userId, userName, cancellationToken);
        return comment is null ? BadRequest("Invalid ArticleId.") : CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertArticleCommentRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var updated = await _articleCommentService.UpdateAsync(id, request, userId, cancellationToken);
        return updated ? NoContent() : Forbid();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var deleted = await _articleCommentService.DeleteAsync(id, userId, cancellationToken);
        return deleted ? NoContent() : Forbid();
    }

    // Admin actions
    [HttpDelete("admin/{id:int}")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> AdminDelete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _articleCommentService.AdminDeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
    {
        var result = await _articleCommentService.ApproveAsync(id, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/reject")]
    [Authorize(Roles = IdentityRoles.Admin)]
    public async Task<IActionResult> Reject(int id, CancellationToken cancellationToken)
    {
        var result = await _articleCommentService.RejectAsync(id, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
