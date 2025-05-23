using System.Data;
using Dapper;
using Finn_klone.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Finn_klone.Backend.Controllers;

[Authorize]
[Route("api/favorites")]
public class FavoriteController : ControllerBase
{
    private readonly IDbConnection _db;

    public FavoriteController(IConfiguration config)
    {
        _db = new SqlConnection(config.GetConnectionString("SqlConnection"));
    }

    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        try
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var query =
                @"SELECT a.*
   FROM Ads a
    JOIN Favorites f ON a.Id = f.AdId
        WHERE f.UserId = @userId";
            var ads = await _db.QueryAsync<Ad>(query, new { UserId = userId });

            foreach (var ad in ads)
            {
                var images = await _db.QueryAsync<AdImage>(
                    "SELECT * FROM AdImages WHERE AdId = @AdId", new { AdId = ad.Id });
                ad.Images = images.ToList();
            }

            return Ok(ads);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{adId}")]
    public async Task<IActionResult> AddFavorites(int adId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var query = "INSERT INTO Favorites (AdId, UserId) VALUES (@adId, @userId)";
            await _db.ExecuteAsync(query, new { AdId = adId, UserId = userId });
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{adId}")]
    public async Task<IActionResult> RemoveFavorites(int adId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var query = "DELETE FROM Favorites WHERE UserId = @UserId AND AdId = @AdId";
            await _db.ExecuteAsync(query, new { UserId = userId, AdId = adId });
            return Ok("removed from favorites");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}