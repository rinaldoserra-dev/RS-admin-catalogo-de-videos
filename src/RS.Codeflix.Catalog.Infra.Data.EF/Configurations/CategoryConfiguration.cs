using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RS.CodeFlix.Catalog.Domain.Entity;

namespace RS.CodeFlix.Catalog.Infra.Data.EF.Configurations
{
    internal class CategoryConfiguration
        : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(category => category.Id);
            builder.Property(category => category.Name).HasMaxLength(255);
            builder.Property(catergory => catergory.Description).HasMaxLength(10_000);
        }
    }
}
