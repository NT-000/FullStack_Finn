namespace Finn_klone;

public class Message
{
    public int Id { get; set; }
    public int SenderUserId { get; set; }
    public int ReceiverUserId { get; set; }
    public int AdId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}