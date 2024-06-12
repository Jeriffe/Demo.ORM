using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class ProductMap : IEntityTypeConfiguration<TProduct>
    {
        public void Configure(EntityTypeBuilder<TProduct> entity)
        {
            entity.HasKey(p => p.Id);

            entity.ToTable("T_Product");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            //entity.Property(e => e.Price).HasColumnType("NUMERIC(18, 4)");
            entity.Property(e => e.Price).HasColumnType("NUMERIC");
        }
    }
}
