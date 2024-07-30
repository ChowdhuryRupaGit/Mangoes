using Mangoes.Services.EmailAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace Mangoes.Services.EmailAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) 
        {
            
        }

        public DbSet<EmailLogger> EmailLogger { get; set; }

    }
}
