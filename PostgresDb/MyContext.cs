using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace PostgresDb;

public class MyContext : DbContext
{
    public const string TableName = "Users";
    public const string Schema = "UserService";

    public MyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.Entity<User>(UserConfigure);
    }

    private void UserConfigure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableName);
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.FirstName);
        builder.HasIndex(x => x.LastName);
    }
}
