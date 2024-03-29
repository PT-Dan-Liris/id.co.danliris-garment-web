﻿using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentCuttingIns.Configs
{
    public class GarmentSubconCuttingInDetailConfig : IEntityTypeConfiguration<GarmentSubconCuttingInDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingInDetailReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingInDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.FabricType).HasMaxLength(25);
            builder.Property(p => p.PreparingUomUnit).HasMaxLength(10);
            builder.Property(p => p.CuttingInUomUnit).HasMaxLength(10);
            builder.Property(p => p.Color).HasMaxLength(1000);
			builder.Property(p => p.UId).HasMaxLength(255);
			builder.HasOne(w => w.GarmentCuttingInItem)
                .WithMany(h => h.Details)
                .HasForeignKey(f => f.CutInItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
