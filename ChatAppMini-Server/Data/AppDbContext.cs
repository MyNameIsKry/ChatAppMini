using Microsoft.EntityFrameworkCore;
using ChatAppMini.Models;

namespace ChatAppMini.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) {}
    public DbSet<User> Users => Set<User>();
}
