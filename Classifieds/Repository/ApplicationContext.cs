using Classifieds.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Classifieds.Repository
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<Advert> Adverts { set; get; }
        public virtual DbSet<AdvertDetail> AdvertDetails { set; get; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
