using System.Data;
using Dapper;
using Finn_klone.Backend.Models;
using Finn_klone.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finn_klone.Backend.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IDbConnection _db;

    public UserController(IDbConnection db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // Bruker JWT-token for å finne id'en til innlogget bruker.
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null) return Unauthorized("Restricted access to page"); // 401-status code

            var query = "SELECT * FROM Users WHERE Id = @Id";
            var user = await _db.QueryFirstOrDefaultAsync<User>(query, new { Id = userId });

            if (user == null) return NotFound("User not found.");

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Henter èn bruker basert på ID, brukes for visning av andres profiler.
    [Authorize]
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";
            var user = await _db.QueryFirstOrDefaultAsync<User>(query, new { Id = id });

            if (user == null)
                return NotFound("user not found.");
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Henter alle brukere
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var query = "SELECT * FROM Users";
            var users = await _db.QueryAsync<User>(query);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateRating()
    {
        try
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null) return Unauthorized("Restricted access to page");
            var query = "UPDATE Users SET Rating = @Rating WHERE Id = @Id";
            var user = await _db.QueryFirstOrDefaultAsync<User>(query, new { Id = userId });
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    // Oppretter en ny bruker
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto newUser, IFormFile file)
    {
        try
        {
            var existingUser =
                await _db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email",
                    new { newUser.Email });

            if (existingUser != null) return BadRequest("Email already exists.");

            string? profileImageUrl = null;

            if (file.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/profileImages");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                profileImageUrl = $"https://localhost:5205/uploads/profileImages/{fileName}";
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            var result = await _db.ExecuteAsync(
                "INSERT INTO Users(Name, Email, PasswordHash, ProfileImageUrl) VALUES (@Name, @Email, @PasswordHash, @ProfileImageUrl)",
                new { newUser.Name, newUser.Email, PasswordHash = hashedPassword, ProfileImageUrl = profileImageUrl });

            if (result > 0) return Ok("User registered successfully.");
            return BadRequest("Something went wrong.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // login

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginUser)
    {
        try
        {
            var user = await _db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email",
                new { loginUser.Email });

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
                return BadRequest("Invalid credentials.");

            var jwt = new Jwt(_config);
            var token = jwt.GenerateToken(user.Id, user.Email);

            // Sett JWT som cookie med HttpOnly og Secure
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true, // forhindrer JavaScript-tilgang
                Secure = true, // sendes kun over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(100)
            });
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // slette og fjerne cookie ved utlogging
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Ok(new { message = "Logged out." });
    }
}