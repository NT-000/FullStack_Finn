namespace Finn_klone.Backend.Models;

public class RegisterDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string? ProfileImageUrl { get; set; }
}