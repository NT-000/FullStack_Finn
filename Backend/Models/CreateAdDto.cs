namespace Finn_klone.Backend.Models;

public class CreateAdDto
{
    public string? Title { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Condition { get; set; }
    public decimal Price { get; set; }
    public int UserId { get; set; }

    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    public string? LocationName { get; set; }
    public List<IFormFile>? Files { get; set; }
}