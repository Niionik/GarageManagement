using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitializeIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Car_GarageId",
                table: "Car",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Garage_GarageId",
                table: "Car",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Garage_GarageId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_GarageId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "Car");
        }
    }
}
