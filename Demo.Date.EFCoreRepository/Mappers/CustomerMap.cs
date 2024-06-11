using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class CustomerMap : IEntityTypeConfiguration<TCustomer>
    {
        public void Configure(EntityTypeBuilder<TCustomer> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Birthday).HasColumnType("DATETIME");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(500);

            entity.Property(e => e.Phone).HasMaxLength(10);

            entity.ToTable("T_Customer");
        }
    }
}
