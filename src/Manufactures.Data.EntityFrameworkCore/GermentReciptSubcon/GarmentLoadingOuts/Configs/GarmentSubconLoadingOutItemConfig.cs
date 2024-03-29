﻿using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentLoadingOuts.Configs
{
    public class GarmentSubconLoadingOutItemConfig : IEntityTypeConfiguration<GarmentSubconLoadingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconLoadingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconLoadingOutItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(50);
            builder.Property(p => p.ProductName).HasMaxLength(500);
            builder.Property(p => p.Color).HasMaxLength(1000);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.SizeName).HasMaxLength(50);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.GarmentLoading)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.LoadingOutId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
