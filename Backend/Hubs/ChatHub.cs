using System.Data;
using Dapper;
using Microsoft.AspNetCore.SignalR;

namespace Finn_klone.Backend.Hubs;

public class ChatHub : Hub
{
    private readonly IDbConnection _db;

    public ChatHub(IDbConnection db)
    {
        _db = db;
    }

    public async Task SendMessage(int senderUserId, int receiverUserId, string message, int? adId)
    {
        var query = @"
    INSERT INTO Messages (SenderUserId, ReceiverUserId, Content, Timestamp, AdId) 
    VALUES (@SenderUserId, @ReceiverUserId, @Content, GETDATE(), @AdId)";

        await _db.ExecuteAsync(query, new
        {
            SenderUserId = senderUserId,
            ReceiverUserId = receiverUserId,
            Content = message,
            AdId = adId
        });
        // sende via SignalR til både avsender og mottaker
        await Clients.Groups(senderUserId.ToString(), receiverUserId.ToString())
            .SendAsync("ReceiveMessage", senderUserId, receiverUserId, message, adId);  
    }

    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User.FindFirst("id").Value;
        if (userIdClaim != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userIdClaim);
        }
        await base.OnConnectedAsync();
    }
}