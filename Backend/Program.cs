using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using System.Text;
using Finn_klone.Backend.Hubs;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// autentifisering med JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

//lager wwwroot-mappa hvis den ikke eksisterer
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "finn-klone",
            ValidAudience = "finn-klone-users",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddAuthorization();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5204); // HTTP
    options.ListenAnyIP(5205, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Henter connectionstring fra appsettings.
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'SqlConnection' not found.");
}

builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(connectionString));


builder.Services.AddControllers();
builder.Services.AddSignalR(); // SignalR for direkte-updates, ligner på onsnapshot
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();  
app.UseCors("AllowFrontend");


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.MapFallbackToFile("index.html");
app.Run();