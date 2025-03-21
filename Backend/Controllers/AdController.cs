using System.Data;
using Dapper;
using Finn_klone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/ads")]
[ApiController]
public class AdController : ControllerBase
{
    private readonly IDbConnection _db;

    public AdController(IDbConnection db)
    {
        _db = db;
    }

    // Henter alle annonser med brukerinfo // Sjekke ut dictionary, minske antall kall
    [HttpGet]
    public async Task<IActionResult> GetAds()
    {
        var query = @"
        SELECT a.*, u.Name, u.Email, u.Rating
        FROM Ads a
        INNER JOIN Users u ON u.Id = a.UserId";

        var ads = await _db.QueryAsync<Ad>(query);
        foreach (var ad in ads)
        {
            var images = await _db.QueryAsync<AdImage>(
                "SELECT * FROM AdImages WHERE AdId = @AdId", new { AdId = ad.Id });
            ad.Images = images.ToList();
        }

        return Ok(ads);
    }

    // Henter en annonse basert på ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAd(int id)
    {
        var query = "SELECT * FROM Ads WHERE Id = @Id";
        var ad = await _db.QueryFirstOrDefaultAsync<Ad>(query, new { Id = id });

        if (ad == null)
            return NotFound("ad is null.");

        var queryImages = "SELECT * FROM AdImages WHERE AdId = @AdId";
        var images = await _db.QueryAsync<AdImage>(queryImages, new { AdId = id });
        ad.Images = images.ToList();
        return Ok(ad);
    }

    // Oppretter ny annonse
    [HttpPost]
    public async Task<IActionResult> CreateAd([FromBody] Ad ad)
    {
        if (ad == null)
            return BadRequest("Fill in required fields.");

        var query = @"
        INSERT INTO Ads (Title, Description, Condition, Price, Category, UserId, CreatedAt)
        VALUES (@Title, @Description, @Condition, @Price, @Category, @UserId, GETDATE())";

        var rowsAffected = await _db.ExecuteAsync(query, ad);

        if (rowsAffected > 0)
            return Ok("Ad created!");

        return StatusCode(500, "Could not create ad.");
    }

    //Oppretter annonse med bilder
    [HttpPost("create-with-files")]
    public async Task<IActionResult> CreateAdWithFiles([FromForm] CreateAdDto adDto)
    {
        try
        {
            if (adDto == null) return BadRequest("No data provided.");

            //legger til annonsen i Ads-tabellen
            var adQuery = @"
INSERT INTO Ads (Title, Description, [Condition], Price, Category, UserId, CreatedAt)
OUTPUT INSERTED.Id
VALUES (@Title, @Description, @Condition, @Price, @Category, @UserId, GETDATE())";
            var adId = await _db.ExecuteScalarAsync<int>(adQuery, new
            {
                adDto.Title,
                adDto.Category,
                adDto.Description,
                adDto.Condition,
                adDto.Price,
                adDto.UserId
            });
            if (adDto.Files.Count > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                foreach (var file in adDto.Files)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    var fileName = $"{Guid.NewGuid()}_{file.FileName}"; // sikrer unik sti til filen
                    var filePath = Path.Combine(uploadFolder, fileName);

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); // lagrer fila på server
                    }

                    var imageUrl = $"https://localhost:5205/uploads/{fileName}";

                    var queryImages =
                        "INSERT INTO AdImages (AdId, ImageUrl) VALUES (@AdId, @ImageUrl)"; // Oppretter rad i AdImages som binder AdId til ImageUrl
                    await _db.ExecuteAsync(queryImages, new { AdId = adId, ImageUrl = imageUrl });
                }
            }

            return Ok(new { message = "Ad created with imgs", adId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($",{ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAd(int id, [FromBody] Ad updatedAd)
    {
        if (updatedAd == null) return BadRequest("No data provided.");
        var query = @"
UPDATE Ads SET Title =@Title, Category=@Category Description=@Description, Condition=@Condition, Price=@Price WHERE Id = @Id ";
        
        await _db.ExecuteAsync(query, new
        {
            updatedAd.Title,
            updatedAd.Category,
            updatedAd.Description,
            updatedAd.Condition,
            updatedAd.Price,
            Id = id,
        });
        return Ok("Ad updated");
    }
    //delete
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        var query = "DELETE FROM Ads WHERE Id = @Id";
        await _db.ExecuteAsync(query, new { Id = id });
        return Ok("ad deleted");
    }
}