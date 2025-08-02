using ChatAppMini.Data;
using ChatAppMini.DTOs.Conversation;
using Microsoft.EntityFrameworkCore;

public interface IConversationRepository
{
    Task SaveChangesAsync();
    Task<ResponseConversationDTO?> GetConversationAsync(Guid id);
    Task<ResponseConversationDTO> CreateConversationAsync(RequestConversationDTO requestConversation);
    Task<bool> IsConversationExistsAsync(Guid conversationId);
    Task<List<ResponseConversationDTO>> GetUserConversationsAsync(Guid userId);
    Task AddParticipantAsync(Guid conversationId, Guid participantId);
    Task RemoveParticipantAsync(Guid conversationId, Guid participantId);
    Task DeleteConversationAsync(Guid conversationId);
}

public class ConversationRepository : IConversationRepository
{
    private readonly AppDbContext _context;

    public ConversationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<bool> IsConversationExistsAsync(Guid conversationId) =>
        await _context.Conversations.AnyAsync(c => c.Id == conversationId);

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

    public async Task<ResponseConversationDTO> CreateConversationAsync(RequestConversationDTO requestConversation)
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid()
        };

        var participants = requestConversation.Participants.Select(p => new ConversationUser
        {
            ConversationId = conversation.Id,
            UserId = p.UserId
        }).ToList();

        conversation.Participants = participants;

        await _context.Conversations.AddAsync(conversation);
        await _context.ConversationUsers.AddRangeAsync(participants);
        await SaveChangesAsync();

        return new ResponseConversationDTO
        {
            Id = conversation.Id,
            Messages = new List<ResponseMessageDTO>(),
            Participants = participants.Select(p => new ResponseParticipantDTO
            {
                UserId = p.UserId,
                Username = _context.Users.Find(p.UserId)?.Name ?? "Unknown"
            }).ToList()
        };
    }

    public async Task<List<ResponseConversationDTO>> GetUserConversationsAsync(Guid userId) =>
        await _context.Conversations
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .Select(c => new ResponseConversationDTO
            {
                Id = c.Id,
                Messages = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Take(1)
                    .Select(msg => new ResponseMessageDTO
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
            .ToListAsync();

    public async Task AddParticipantAsync(Guid conversationId, Guid participantId)
    {
        var conversationUser = new ConversationUser
        {
            ConversationId = conversationId,
            UserId = participantId
        };

        await _context.ConversationUsers.AddAsync(conversationUser);
        await SaveChangesAsync();
    }

    public async Task RemoveParticipantAsync(Guid conversationId, Guid participantId)
    {
        var participant = await _context.ConversationUsers
            .FirstOrDefaultAsync(cu => cu.ConversationId == conversationId && cu.UserId == participantId);

        if (participant != null)
        {
            _context.ConversationUsers.Remove(participant);
            await SaveChangesAsync();
        }
    }

    public async Task DeleteConversationAsync(Guid conversationId)
    {
        var conversation = await _context.Conversations
            .Include(c => c.Messages)
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation != null)
        {
            _context.Messages.RemoveRange(conversation.Messages);
            _context.ConversationUsers.RemoveRange(conversation.Participants);
            _context.Conversations.Remove(conversation);
            await SaveChangesAsync();
        }
    }
}