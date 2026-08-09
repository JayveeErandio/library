using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) {}

    public DbSet<Member> Accounts => Set<Member>();

    public DbSet<Book> Genres => Set<Book>();

    public DbSet<Borrowing> Choices => Set<Borrowing>();
}