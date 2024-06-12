using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class OrderItemMap : IEntityTypeConfiguration<TOrderItem>
    {
        public void Configure(EntityTypeBuilder<TOrderItem> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(e => e.CreateDate).HasColumnType("DATETIME");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            //entity.Property(e => e.Price).HasColumnType("NUMERIC(18, 4)");
            entity.Property(e => e.Price).HasColumnType("NUMERIC");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
          .HasForeignKey(d => d.ProductId);
         // .OnDelete(DeleteBehavior.ClientSetNull);

            entity.ToTable("T_OrderItem");
        }
    }
}