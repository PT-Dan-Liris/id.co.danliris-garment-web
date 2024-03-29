﻿using Manufactures.Domain.GermentReciptSubcon.GarmentSewingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingIns.Configs
{
    public class GarmentSubconSewingInItemConfig : IEntityTypeConfiguration<GarmentSubconSewingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconSewingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconSewingInItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSewingInIdentity)
                   .WithMany(a => a.GarmentSewingInItem)
                   .HasForeignKey(a => a.SewingInId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.DesignColor).HasMaxLength(2000);
            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);
            builder.Property(a => a.Color).HasMaxLength(1000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}