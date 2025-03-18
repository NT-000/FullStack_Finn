
using Finn_klone.Backend.Services;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace Finn_klone.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IDbConnection _db;
    private readonly IConfiguration _config;
    
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
        var userId = User.FindFirst("id")?.Value;
        var email = User.FindFirst("email")?.Value;
        if (userId == null) return Unauthorized("Restricted access to page"); // 401-status code

        string query = "SELECT Id, Name, Email, Rating FROM Users WHERE Id = @Id";
        var user = await _db.QueryFirstOrDefaultAsync<User>(query, new { Id = userId });

        if (user == null) return NotFound("User not found.");

        return Ok(user);
    }

    // Henter èn bruker basert på ID, brukes for visning av andres profiler, tatt bort email fra query.
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        
        string query = "SELECT Id, Name, Rating FROM Users WHERE Id = @Id";
        var user = await _db.QueryFirstOrDefaultAsync<User>(query, new { Id = id });

        if (user == null)
            return NotFound("user not found.");

        return Ok(user);
    }
    // Henter alle brukere
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        string query = "SELECT Id, Name, Email, Rating FROM Users";
        var users = await _db.QueryAsync<User>(query);
        return Ok(users);
    }



    // Oppretter en ny bruker
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
    {
        var existingUser = await _db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { newUser.Email });
    
        if (existingUser != null)
        { 
            return BadRequest("Email already exists.");
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
        var result = await _db.ExecuteAsync(
            "INSERT INTO Users(Name, Email, PasswordHash) VALUES (@Name, @Email, @PasswordHash)",
            new { newUser.Name, newUser.Email, PasswordHash = hashedPassword });
        if (result > 0)
        {
            return Ok("User registered successfully.");
        }
        return BadRequest("Something went wrong.");
    }
    
    // login

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginUser)
    {
        var user = await _db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { loginUser.Email });

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
        {
            return BadRequest("Invalid credentials.");
        }

        var jwt = new Jwt(_config);
        var token = jwt.GenerateToken(user.Id, user.Email);
        
        // Sett JWT som cookie med HttpOnly og Secure
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,      // forhindrer JavaScript-tilgang
            Secure = true,        // sendes kun over HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });
        return Ok(new { token });
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
