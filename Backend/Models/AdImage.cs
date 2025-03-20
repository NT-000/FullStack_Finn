namespace Finn_klone;

public class AdImage
{
    public int Id { get; set; }
    public int AdId { get; set; } // foreign key
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}