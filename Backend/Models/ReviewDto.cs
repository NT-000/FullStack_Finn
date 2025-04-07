namespace Finn_klone.Backend.Models;

public class ReviewDto
{
    public int ToUserId { get; set; }
    public int AdId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}