using Microsoft.EntityFrameworkCore;

namespace BooksApp.Data;

public partial class BookAppStoreContext : DbContext
{
    public BookAppStoreContext()
    {
    }

    public BookAppStoreContext(DbContextOptions<BookAppStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}
