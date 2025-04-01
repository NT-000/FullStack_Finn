using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finn_klone.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IDbConnection _db;

    public ReviewController(IDbConnection db)
    {
        _db = db;
    }

    //Get

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetReviewsForUser(int userId)
    {
        var query = "SELECT * FROM Reviews WHERE ToUserId = @UserId ORDER BY CreatedAt DESC";
        var reviews = await _db.QueryAsync<Review>(query, new { UserId = userId });
        return Ok(reviews);
    }

    [HttpGet("ad/{adId}")]
    public async Task<IActionResult> GetReviewsForAd(int adId)
    {
        var query = "SELECT * FROM Reviews WHERE AdId = @AdId";
        var reviews = await _db.QueryAsync<Review>(query, new { AdId = adId });
        return Ok(reviews);
    }

    //Post

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
    {
        var fromUserId = int.Parse(User.FindFirst("id").Value);
        if (fromUserId == reviewDto.ToUserId) return BadRequest("du kan ikke vurdere deg selv");

        var query = @"
INSERT INTO Reviews (FromUserId, ToUserId, AdId, Rating, Comment, CreatedAt)
VALUES (@FromUserId, @ToUserId, @AdId, @Rating, @Comment, GETDATE())";

        var rowsAffected = await _db.ExecuteAsync(query, new
        {
            FromUserId = fromUserId,
            reviewDto.ToUserId,
            reviewDto.AdId,
            reviewDto.Rating,
            reviewDto.Comment
        });

        if (rowsAffected > 0) return Ok();
        return StatusCode(500, "kunne ikke lage vurdering.");
    }
}