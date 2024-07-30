using Mangoes.Services.OrderAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace Mangoes.Services.OrderAPI.Data
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) 
        {
            
        }

        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }

    }
}
