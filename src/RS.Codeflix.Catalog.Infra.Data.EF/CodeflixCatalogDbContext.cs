using Microsoft.EntityFrameworkCore;
using RS.Codeflix.Catalog.Infra.Data.EF.Configurations;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.Codeflix.Catalog.Infra.Data.EF
{
    public class CodeflixCatalogDbContext
        : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public CodeflixCatalogDbContext(DbContextOptions<CodeflixCatalogDbContext> options) 
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
