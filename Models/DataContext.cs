using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
  public class DataContext : DbContext
  {
    protected readonly IConfiguration Configuration;

    public DataContext() { }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();

    }

    // Publishing to the contect the connection String provided in application.json
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseSqlite($"Data Source=./Database/uniManager.db");
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<TeachClass> TeachClasses { get; set; }
    public DbSet<Dilosi> Dilosis { get; set; }
  }
}
