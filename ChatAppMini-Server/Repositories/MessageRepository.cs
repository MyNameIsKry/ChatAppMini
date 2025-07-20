using ChatAppMini.Data;

public interface IMessageRepository
{
    Task SaveChangesAsync();
}

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}