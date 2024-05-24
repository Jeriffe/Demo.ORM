using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Date.EFCoreRepository
{
    internal class PatientMap : IEntityTypeConfiguration<TPatient>
    {
        public void Configure(EntityTypeBuilder<TPatient> builder)
        {
            builder.HasKey(p => p.PatientId);

            builder.ToTable("T_PATIENT");

            builder.Property(p => p.PatientType).HasColumnType("smallint");

        }
    }
}
