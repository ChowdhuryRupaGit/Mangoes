using Mangoes.Services.CartAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace Mangoes.Services.CartAPI.Data
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) 
        {
            
        }

        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeader { get; set; }

    }
}
