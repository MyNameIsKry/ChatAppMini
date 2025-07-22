using ChatAppMini.Data;

public interface IMessageRepository
{
    Task SaveChangesAsync();
    Task<Message> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId);
}

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<Message> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId)
    {
        var message = new Message
        {
            Content = messageDto.Content,
            SentAt = messageDto.SentAt,
            ConversationId = conversationId,
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }
}