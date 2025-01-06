using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarageCar_Car_CarId",
                table: "GarageCar");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCar_Garage_GarageId",
                table: "GarageCar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GarageCar",
                table: "GarageCar");

            migrationBuilder.RenameTable(
                name: "GarageCar",
                newName: "GarageCars");

            migrationBuilder.RenameIndex(
                name: "IX_GarageCar_CarId",
                table: "GarageCars",
                newName: "IX_GarageCars_CarId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Car",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarageCars",
                table: "GarageCars",
                columns: new[] { "GarageId", "CarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Car_OwnerId",
                table: "Car",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_AspNetUsers_OwnerId",
                table: "Car",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GarageCars_Car_CarId",
                table: "GarageCars",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarageCars_Garage_GarageId",
                table: "GarageCars",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_AspNetUsers_OwnerId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCars_Car_CarId",
                table: "GarageCars");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCars_Garage_GarageId",
                table: "GarageCars");

            migrationBuilder.DropIndex(
                name: "IX_Car_OwnerId",
                table: "Car");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GarageCars",
                table: "GarageCars");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Car");

            migrationBuilder.RenameTable(
                name: "GarageCars",
                newName: "GarageCar");

            migrationBuilder.RenameIndex(
                name: "IX_GarageCars_CarId",
                table: "GarageCar",
                newName: "IX_GarageCar_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GarageCar",
                table: "GarageCar",
                columns: new[] { "GarageId", "CarId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GarageCar_Car_CarId",
                table: "GarageCar",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarageCar_Garage_GarageId",
                table: "GarageCar",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
