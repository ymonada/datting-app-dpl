using WebSocket.Domain.Entity;

namespace WebSocket.Entity;

public class Message
{
    public int Id { get; set; }
    public int MatchId { get; set; } // Зовнішній ключ до Match
    public int SenderId { get; set; } // ID відправника (User1 або User2)
    public string Content { get; set; } // Текст повідомлення
    public DateTime SentAt { get; set; } // Час відправлення
    public bool IsRead { get; set; } // Чи прочитано

    // Навігаційні властивості
    public Match Match { get; set; }
    public User Sender { get; set; }
}