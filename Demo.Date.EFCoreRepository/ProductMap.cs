using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class ProductMap : IEntityTypeConfiguration<TProduct>
    {
        public void Configure(EntityTypeBuilder<TProduct> builder)
        {
            builder.HasKey(p => p.Id);

            builder.ToTable("T_Product");
        }
    }
}
