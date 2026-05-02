using Microsoft.AspNetCore.Mvc;
using SidraHub.Application.Services.CustomerReviews;

namespace SidraHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomerReviewsController : ControllerBase
{
    private readonly ICustomerReviewService _customerReviewService;

    public CustomerReviewsController(ICustomerReviewService customerReviewService)
    {
        _customerReviewService = customerReviewService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var reviews = await _customerReviewService.GetAllAsync(cancellationToken);
        return Ok(reviews);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var review = await _customerReviewService.GetByIdAsync(id, cancellationToken);
        return review is null ? NotFound() : Ok(review);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertCustomerReviewRequest request, CancellationToken cancellationToken)
    {
        var review = await _customerReviewService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertCustomerReviewRequest request, CancellationToken cancellationToken)
    {
        var updated = await _customerReviewService.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _customerReviewService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
