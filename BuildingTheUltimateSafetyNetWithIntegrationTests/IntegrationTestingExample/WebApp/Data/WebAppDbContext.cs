using Microsoft.EntityFrameworkCore;

namespace WebApp.Data;

public class WebAppDbContext(DbContextOptions<WebAppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}