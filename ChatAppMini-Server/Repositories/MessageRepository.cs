using ChatAppMini.Data;
using Microsoft.EntityFrameworkCore;

public interface IMessageRepository
{
    Task SaveChangesAsync();
    Task<Message> SaveUsersMessages(RequestMessageDTO msg);
    Task<List<Message>> GetUsersMessages(Guid userId, Guid withUser);
}

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<Message> SaveUsersMessages(RequestMessageDTO msg)
    {
        Message newMessage = new Message
        {
            SenderId = msg.SenderId,
            ReceiverId = msg.ReceiverId,
            Content = msg.Content,
            SentAt = msg.SentAt
        };

        await _context.Messages.AddAsync(newMessage);

        return newMessage;
    }

    public async Task<List<Message>> GetUsersMessages(Guid userId, Guid withUser)
    {
        var message = await  _context.Messages
            .Where(m =>
                (m.SenderId == userId && m.ReceiverId == withUser) ||
                (m.SenderId == withUser && m.ReceiverId == userId)
            )
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return message;
    }
}