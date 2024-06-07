using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class OrderMap : IEntityTypeConfiguration<TOrder>
    {
        public void Configure(EntityTypeBuilder<TOrder> builder)
        {
            builder.HasKey(p => p.Id);

            builder.ToTable("T_Order");
        }
    }
}
