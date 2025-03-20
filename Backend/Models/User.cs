namespace Finn_klone;

public class User
{
   public int Id { get; set; }
   public string? Name { get; set; }
   public string? Email { get; set; }
   public string PasswordHash { get; set; }
   public int Rating { get; set; }
   public bool IsAdmin { get; set; }
   public string? ProfileImageUrl { get; set; }
   public List<Ad> Ads { get; set; }
}