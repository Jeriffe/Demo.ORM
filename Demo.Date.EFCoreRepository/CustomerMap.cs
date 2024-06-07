using Demo.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Date.EFCoreRepository
{
    internal class CustomerMap : IEntityTypeConfiguration<TCustomer>
    {
        public void Configure(EntityTypeBuilder<TCustomer> builder)
        {
            builder.HasKey(p => p.Id);

            builder.ToTable("T_Customer");
        }
    }
}
