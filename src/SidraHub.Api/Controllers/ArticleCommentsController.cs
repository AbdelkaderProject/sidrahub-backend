using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SidraHub.Application.Services.ArticleComments;

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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var comments = await _articleCommentService.GetAllAsync(cancellationToken);
        return Ok(comments);
    }

    [HttpGet("article/{articleId:int}")]
    public async Task<IActionResult> GetByArticleId(int articleId, CancellationToken cancellationToken)
    {
        var comments = await _articleCommentService.GetByArticleIdAsync(articleId, cancellationToken);
        return Ok(comments);
    }

    [HttpGet("{id:int}")]
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
}
