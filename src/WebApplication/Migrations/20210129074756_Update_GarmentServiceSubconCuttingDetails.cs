﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_GarmentServiceSubconCuttingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "CuttingInId",
                table: "GarmentServiceSubconCuttingItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
