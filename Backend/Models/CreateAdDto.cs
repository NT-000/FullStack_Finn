namespace Finn_klone;

public class CreateAdDto
{
    public string? Title { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Condition { get; set; }
    public decimal Price { get; set; }
    public int UserId { get; set; }
    
    public List<IFormFile> Files { get; set; }
}