using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.ServiceCategories;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ServiceCategoriesController : ControllerBase
{
    private readonly IServiceCategoryService _serviceCategoryService;

    public ServiceCategoriesController(IServiceCategoryService serviceCategoryService)
    {
        _serviceCategoryService = serviceCategoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _serviceCategoryService.GetAllAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _serviceCategoryService.GetByIdAsync(id, cancellationToken);
        return category is null ? NotFound() : Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertServiceCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _serviceCategoryService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertServiceCategoryRequest request, CancellationToken cancellationToken)
    {
        var updated = await _serviceCategoryService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceCategoryService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
