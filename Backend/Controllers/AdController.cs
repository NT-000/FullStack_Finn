using System.Data;
using System.Globalization;
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

    //Get
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

    [Authorize]
    [HttpGet("bought")]
    public async Task<IActionResult> GetBoughtAds()
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null) return Unauthorized("User not found in token");

        var buyerId = int.Parse(userIdClaim.Value);

        var query = @"SELECT * FROM Ads WHERE BuyerId = @BuyerId AND isSold = 1";
        var boughtAds = await _db.QueryAsync<Ad>(query, new { BuyerId = buyerId });

        foreach (var ad in boughtAds)
        {
            var images = await _db.QueryAsync<AdImage>(
                "SELECT * FROM AdImages WHERE AdId = @AdId", new { AdId = ad.Id });
            ad.Images = images.ToList();
        }

        return Ok(boughtAds);
    }

    //Post


    //Oppretter annonse med bilder
    [HttpPost("create-with-files")]
    public async Task<IActionResult> CreateAdWithFiles([FromForm] CreateAdDto adDto)
    {
        try
        {
            if (adDto == null) return BadRequest("No data provided.");

            //legger til annonsen i Ads-tabellen

            var adQuery = @"
INSERT INTO Ads (Title, Description, Condition, Price, Category, UserId, Latitude, Longitude, LocationName, CreatedAt)
OUTPUT INSERTED.Id
VALUES (@Title, @Description, @Condition, @Price, @Category, @UserId, @Latitude, @Longitude, @LocationName, GETDATE())";


            // må konvertere fra string som fåes fra frontend grunnet formData
            var latitudeParsed = !string.IsNullOrWhiteSpace(adDto.Latitude)
                ? double.Parse(adDto.Latitude, CultureInfo.InvariantCulture)
                : (double?)null;

            var longitudeParsed = !string.IsNullOrWhiteSpace(adDto.Longitude)
                ? double.Parse(adDto.Longitude, CultureInfo.InvariantCulture)
                : (double?)null;

            var adId = await _db.ExecuteScalarAsync<int>(adQuery, new
            {
                adDto.Title,
                adDto.Category,
                adDto.Description,
                adDto.Condition,
                adDto.Price,
                adDto.UserId,
                Latitude = latitudeParsed,
                Longitude = longitudeParsed,
                adDto.LocationName
            });
            if (adDto.Files == null || adDto.Files.Count <= 0)
                return Ok(new { message = "Ad created with imgs", adId });
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            foreach (var file in adDto.Files)
            {
                if (file.Length == 0)
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

            return Ok(new { message = "Ad created with imgs", adId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    //Update

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAd(int id, [FromBody] AdUpdateDto updatedAd)
    {
        try
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized("No user id found in token");

            var userId = int.Parse(userIdClaim.Value);

            var existingAd = await _db.QueryFirstOrDefaultAsync<Ad>(
                "SELECT * FROM Ads WHERE Id = @Id", new { Id = id });

            if (existingAd == null)
                return NotFound("Ad not found.");

            if (existingAd.UserId != userId)
                return Unauthorized("Not authorized to update this ad.");

            var query = @"
        UPDATE Ads SET
            Title = @Title,
            Category = @Category,
            Description = @Description,
            Condition = @Condition,
            Price = @Price
        WHERE Id = @Id";

            await _db.ExecuteAsync(query, new
            {
                Title = updatedAd.Title ?? existingAd.Title,
                Category = updatedAd.Category ?? existingAd.Category,
                Description = updatedAd.Description ?? existingAd.Description,
                Condition = updatedAd.Condition ?? existingAd.Condition,
                Price = updatedAd.Price ?? existingAd.Price,
                Id = id
            });

            return Ok("Ad updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // markere som solgt
    [Authorize]
    [HttpPut("{id}/sold")]
    public async Task<IActionResult> MarkAsSold(int id, [FromBody] MarkAsSoldDto dto)
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null) return Unauthorized("user not found in token");
        var sellerId = int.Parse(userIdClaim.Value);

        var ad = await _db.QueryFirstOrDefaultAsync<Ad>(
            "SELECT * FROM Ads WHERE Id = @Id", new { Id = id });

        if (ad == null) return NotFound("dd not found.");

        if (ad.UserId != sellerId) return Unauthorized("not authorized to update this ad.");
        var query = @"
UPDATE Ads SET
isSold = 1,
BuyerId = @BuyerId
           WHERE Id = @Id";

        var rowsAffected = await _db.ExecuteAsync(query, new { dto.BuyerId, Id = id });
        if (rowsAffected > 0) return Ok("Ad marked as sold");
        return StatusCode(500, "ad not updated.");
    }

    //oppdatere kordinater
    [Authorize]
    [HttpPut("{id}/location")]
    public async Task<IActionResult> UpdateAdLocation(int id, [FromBody] LocationDto updatedLocation)
    {
        var existingAd = await _db.QueryFirstOrDefaultAsync<Ad>(
            "SELECT * FROM Ads WHERE Id = @Id", new { Id = id }
        );
        if (existingAd == null)
            return NotFound("Ad not found.");

        // sjekk at brukeren eier annonsen
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null)
            return Unauthorized("No user id found in token");
        var userId = int.Parse(userIdClaim.Value);
        if (existingAd.UserId != userId)
            return Unauthorized("Not authorized to update location of this ad.");

        var query = "UPDATE Ads SET Latitude = @Lat, Longitude =@Lng, LocationName=@Loc  WHERE Id = @Id";

        var rowsAffected = await _db.ExecuteAsync(query, new
        {
            Lat = updatedLocation.Latitude,
            Lng = updatedLocation.Longitude,
            Loc = updatedLocation.LocationName,
            Id = id
        });
        return rowsAffected > 0 ? Ok("location updated successfully.") : StatusCode(500, "Could not update location.");
    }

    //Delete
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        var query = "DELETE FROM Ads WHERE Id = @Id";
        try
        {
            await _db.ExecuteAsync(query, new { Id = id });
            return Ok("ad deleted");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}