using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finn_klone.Controllers;

[Route("api/messages")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IDbConnection _db;

    public MessageController(IDbConnection db)
    {
        _db = db;
    }

    [HttpGet("conversation")]
    public async Task<IActionResult> GetConversation(int userId1, int userId2)
    {
        var query = @"
            SELECT * FROM Messages
            WHERE (SenderUserId = @userId1 AND ReceiverUserId = @userId2)
            OR (SenderUserId = @userId2 AND ReceiverUserId = @userId1)
            ORDER BY Timestamp ASC
        ";
        var messages = await _db.QueryAsync<Message>(query, new { userId1, userId2 });
        return Ok(messages);
    }

    [HttpGet("interested-users/{adId}")]
    public async Task<IActionResult> GetInterestedUsers(int adId)
    {
        var query = @"
    SELECT DISTINCT u.Id AS id, u.Name AS name
    FROM Messages m
    JOIN Users u ON u.Id = m.SenderUserId
    WHERE m.AdId = @AdId";

        var interestedUsers = await _db.QueryAsync(query, new { AdId = adId });
        return Ok(interestedUsers);
    }
}