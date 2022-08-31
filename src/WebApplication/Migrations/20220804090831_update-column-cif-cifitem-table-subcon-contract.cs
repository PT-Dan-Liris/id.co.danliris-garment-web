using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updatecolumncifcifitemtablesubconcontract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "CIF",
                table: "GarmentSubconContracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CIFItem",
                table: "GarmentSubconContractItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CIF",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "CIFItem",
                table: "GarmentSubconContractItems");
        }
    }
}
