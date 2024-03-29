﻿using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentPreparings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentPreparings.Config
{
    public class GarmentSubconPreparingItemConfig : IEntityTypeConfiguration<GarmentSubconPreparingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconPreparingItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconPreparingItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentPreparingIdentity)
               .WithMany(a => a.GarmentPreparingItem)
               .HasForeignKey(a => a.GarmentSubconPreparingId);

            builder.Property(o => o.ProductCode)
               .HasMaxLength(25);
            builder.Property(o => o.ProductName)
               .HasMaxLength(100);
            builder.Property(o => o.UomUnit)
               .HasMaxLength(100);
            builder.Property(o => o.DesignColor)
               .HasMaxLength(2000);
            builder.Property(o => o.FabricType)
               .HasMaxLength(100);
            builder.Property(o => o.ROSource)
               .HasMaxLength(100);
			builder.Property(o => o.UId)
				.HasMaxLength(255);
            builder.Property(o => o.BeacukaiNo)
               .HasMaxLength(20);
            builder.Property(o => o.BeacukaiType)
               .HasMaxLength(20);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}