using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;

public interface IMessageRepository
{
    Task SaveChangesAsync();
    Task<Message> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId, Guid senderId);
    Task<List<ResponseMessageDTO>> GetMessagesInConversationAsync(Guid conversationId, int take = 20, DateTime? before = null);
    Task<Message?> GetMessageByIdAsync(Guid messageId);
    Task<Message> UpdateMessageAsync(Guid messageId, string newContent);
    Task<bool> DeleteMessageAsync(Guid messageId);
}

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<Message> SendMessageAsync(RequestMessageDTO messageDto, Guid conversationId, Guid senderId)
    {
        var message = new Message
        {
            Content = messageDto.Content,
            SentAt = messageDto.SentAt,
            ConversationId = conversationId,
            SenderId = senderId
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<List<ResponseMessageDTO>> GetMessagesInConversationAsync(Guid conversationId, int take = 20, DateTime? before = null)
    {
        var query = _context.Messages
            .Where(m => m.ConversationId == conversationId);

        if (before.HasValue)
        {
            query = query.Where(m => m.SentAt < before.Value);
        }

        return await query
            .OrderByDescending(m => m.SentAt)
            .Take(take)
            .Select(m => new ResponseMessageDTO 
            {
                Id = m.Id,
                Content = m.Content,
                SenderId = m.SenderId,
                SentAt = m.SentAt
            })
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(Guid messageId)
    {
        return await _context.Messages.FindAsync(messageId);
    }

    public async Task<Message> UpdateMessageAsync(Guid messageId, string newContent)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null)
            throw new InvalidOperationException("Message not found");

        message.Content = newContent;
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<bool> DeleteMessageAsync(Guid messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null)
            return false;

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return true;
    }
}