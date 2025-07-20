using Microsoft.EntityFrameworkCore;

namespace ChatAppMini.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<ConversationUser> ConversationUsers => Set<ConversationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConversationUser>()
            .HasKey(cu => new { cu.UserId, cu.ConversationId });

        // Quan hệ: ConversationUser → User
        modelBuilder.Entity<ConversationUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.Conversations)
            .HasForeignKey(cu => cu.UserId);

        // Quan hệ: ConversationUser → Conversation
        modelBuilder.Entity<ConversationUser>()
            .HasOne(cu => cu.Conversation)
            .WithMany(c => c.Participants)
            .HasForeignKey(cu => cu.ConversationId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany() // không cần navigation ngược nếu không dùng
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict); // tránh xóa User thì xóa hết Message
    }
}
