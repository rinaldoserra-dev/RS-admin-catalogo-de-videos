using Microsoft.EntityFrameworkCore;
using RS.CodeFlix.Catalog.Infra.Data.EF.Configurations;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.Infra.Data.EF
{
    public class CodeFlixCatalogDbContext
        : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public CodeFlixCatalogDbContext(DbContextOptions<CodeFlixCatalogDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Notification>();
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
