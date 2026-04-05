using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.Articles;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var articles = await _articleService.GetAllAsync(cancellationToken);
        return Ok(articles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var article = await _articleService.GetByIdAsync(id, cancellationToken);
        return article is null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertArticleRequest request, CancellationToken cancellationToken)
    {
        var article = await _articleService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertArticleRequest request, CancellationToken cancellationToken)
    {
        var updated = await _articleService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _articleService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
