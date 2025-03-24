using System.Data;
using Dapper;
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

}