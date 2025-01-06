using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerToGarage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garage_AspNetUsers_OwnerId",
                table: "Garage");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Garage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Garage_AspNetUsers_OwnerId",
                table: "Garage",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garage_AspNetUsers_OwnerId",
                table: "Garage");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Garage",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Garage_AspNetUsers_OwnerId",
                table: "Garage",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
