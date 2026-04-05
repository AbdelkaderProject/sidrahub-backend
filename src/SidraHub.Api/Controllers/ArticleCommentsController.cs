using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var comment = await _articleCommentService.GetByIdAsync(id, cancellationToken);
        return comment is null ? NotFound() : Ok(comment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertArticleCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _articleCommentService.CreateAsync(request, cancellationToken);
        return comment is null ? BadRequest("Invalid ArticleId.") : CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertArticleCommentRequest request, CancellationToken cancellationToken)
    {
        var updated = await _articleCommentService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : BadRequest("Entity not found or invalid ArticleId.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _articleCommentService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
