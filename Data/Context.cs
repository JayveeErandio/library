using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) {}

    public DbSet<Member> Members => Set<Member>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Borrowing> Borrowings => Set<Borrowing>();
}