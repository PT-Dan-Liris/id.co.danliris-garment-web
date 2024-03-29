﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class SampleFinishingMonitoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentMonitoringSampleFinishingReportTemplate",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    RoJob = table.Column<string>(maxLength: 25, nullable: false),
                    Article = table.Column<string>(maxLength: 100, nullable: true),
                    Stock = table.Column<double>(nullable: false),
                    SewingQtyPcs = table.Column<double>(nullable: false),
                    FinishingQtyPcs = table.Column<double>(nullable: false),
                    RemainQty = table.Column<double>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentMonitoringSampleFinishingReportTemplate", x => x.RoJob);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentMonitoringSampleFinishingReportTemplate");
        }
    }
}
