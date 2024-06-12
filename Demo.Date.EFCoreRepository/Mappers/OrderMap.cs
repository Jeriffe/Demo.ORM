using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class OrderMap : IEntityTypeConfiguration<TOrder>
    {
        public void Configure(EntityTypeBuilder<TOrder> entity)
        {
            entity.HasKey(p => p.Id);

            entity.ToTable("T_Order");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.CreateDate).HasColumnType("DATETIME");
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.Description).HasMaxLength(100);
            //entity.Property(e => e.TotalPrice).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.TotalPrice).HasColumnType("NUMERIC");


            entity.HasOne(d => d.TCustomer).WithMany(p => p.Orders)
            .HasForeignKey(d => d.CustomerID);
            //.OnDelete(DeleteBehavior.ClientSetNull)
            //.HasConstraintName("FK_T_Order_CustomerId");
        }
    }
}
