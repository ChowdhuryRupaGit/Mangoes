using Mangoes.Services.CouponAPIS.Model;
using Microsoft.EntityFrameworkCore;

namespace Mangoes.Services.CouponAPIS.Data
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options) 
        {
            
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "10OFF",
                DiscountAmount = 10.00,
                MinAmount = 20

            });
            builder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "20OFF",
                DiscountAmount = 20.00,
                MinAmount = 40

            });

        }

    }
}
