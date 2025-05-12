using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311_POE_ST10150702.Migrations
{
    /// <inheritdoc />
    public partial class ModifyProductFarmerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FarmerId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId1",
                table: "Products",
                column: "FarmerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Farmers_FarmerId1",
                table: "Products",
                column: "FarmerId1",
                principalTable: "Farmers",
                principalColumn: "FarmerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Farmers_FarmerId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FarmerId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FarmerId1",
                table: "Products");
        }
    }
}
