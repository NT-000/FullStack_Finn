namespace Finn_klone;

public class Ad
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Condition { get; set; }
    public decimal Price { get; set; }
    public int UserId { get; set; } // foreign key

    public DateTime CreatedAt { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }
    
    public string? LocationName { get; set; }


    public List<AdImage> Images { get; set; } = new(); // bilder til annonsene
}