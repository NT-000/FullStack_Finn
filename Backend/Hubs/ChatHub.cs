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

    public async Task SendMessage(int senderUserId, int receiverUserId, string message)
    {
        var query = "INSERT INTO Messages (SenderUserId, ReceiverUserId, Content, Timestamp) VALUES (@sId, @rId, @txt, GETDATE())";
        await _db.ExecuteAsync(query, new { sId = senderUserId, rId = receiverUserId, txt = message });
        
        // sende via SignalR til spesifikk mottaker
        await Clients.Group(receiverUserId.ToString()).SendAsync("ReceiveMessage", senderUserId, receiverUserId, message);
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