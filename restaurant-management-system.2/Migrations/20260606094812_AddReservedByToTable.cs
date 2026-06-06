using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restaurant_management_system._2.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedByToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReservedBy",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedBy",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Orders");
        }
    }
}
