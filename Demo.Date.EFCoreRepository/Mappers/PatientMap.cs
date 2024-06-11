using Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Date.EFCoreRepository
{
    internal class PatientMap : IEntityTypeConfiguration<TPatient>
    {
        public void Configure(EntityTypeBuilder<TPatient> entity)
        {
            entity.HasKey(e => e.PatientId);

            entity.ToTable("T_PATIENT");

            //Changed PatientType to int in DB
            entity.Property(e => e.PatientType);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.MedRecNumber).HasMaxLength(255);
            entity.Property(e => e.MiddleInitial).HasMaxLength(255);
            entity.Property(e => e.SiteId).HasColumnName("SiteID");
        }
    }


    
}
