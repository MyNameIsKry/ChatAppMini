using ChatAppMini.Data;
using ChatAppMini.DTOs.Conversation;
using Microsoft.EntityFrameworkCore;

public interface IConversationRepository
{
    Task SaveChangesAsync();
    Task<ResponseConversationDTO?> GetConversationAsync(Guid id);
}

public class ConversationRepository : IConversationRepository
{
    private readonly AppDbContext _context;

    public ConversationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<ResponseConversationDTO?> GetConversationAsync(Guid id) =>
        await _context.Conversations
            .Where(c => c.Id == id)
            .Select(c => new ResponseConversationDTO
            {
                Id = c.Id,
                Messages = c.Messages.Select(msg => new ResponseMessageDTO
                {
                    Id = msg.Id,
                    Content = msg.Content,
                    SenderId = msg.SenderId,
                    SentAt = msg.SentAt
                }).ToList(),
                Participants = c.Participants.Select(u => new ResponseParticipantDTO
                {
                    UserId = u.UserId,
                    Username = u.User.Name,
                }).ToList()
            })
            .FirstOrDefaultAsync(); 
    
}