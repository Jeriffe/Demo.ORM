using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class OrderItemMap : IEntityTypeConfiguration<TOrderItem>
    {
        public void Configure(EntityTypeBuilder<TOrderItem> builder)
        {
            builder.HasKey(p => p.Id);

            builder.ToTable("T_OrderItem");
        }
    }
}