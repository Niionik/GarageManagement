using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_AspNetUsers_OwnerId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Garage_AspNetUsers_OwnerId",
                table: "Garage");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCars_Car_CarId",
                table: "GarageCars");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCars_Garage_GarageId",
                table: "GarageCars");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenance_Car_CarId",
                table: "Maintenance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Garage",
                table: "Garage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car",
                table: "Car");

            migrationBuilder.RenameTable(
                name: "Garage",
                newName: "Garages");

            migrationBuilder.RenameTable(
                name: "Car",
                newName: "Cars");

            migrationBuilder.RenameIndex(
                name: "IX_Garage_OwnerId",
                table: "Garages",
                newName: "IX_Garages_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Car_OwnerId",
                table: "Cars",
                newName: "IX_Cars_OwnerId");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Garages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Garages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Garages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Garages",
                table: "Garages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_GarageId",
                table: "Cars",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_OwnerId",
                table: "Cars",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Garages_GarageId",
                table: "Cars",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarageCars_Cars_CarId",
                table: "GarageCars",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GarageCars_Garages_GarageId",
                table: "GarageCars",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Garages_AspNetUsers_OwnerId",
                table: "Garages",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenance_Cars_CarId",
                table: "Maintenance",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_OwnerId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Garages_GarageId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCars_Cars_CarId",
                table: "GarageCars");

            migrationBuilder.DropForeignKey(
                name: "FK_GarageCars_Garages_GarageId",
                table: "GarageCars");

            migrationBuilder.DropForeignKey(
                name: "FK_Garages_AspNetUsers_OwnerId",
                table: "Garages");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenance_Cars_CarId",
                table: "Maintenance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Garages",
                table: "Garages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_GarageId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "Garages",
                newName: "Garage");

            migrationBuilder.RenameTable(
                name: "Cars",
                newName: "Car");

            migrationBuilder.RenameIndex(
                name: "IX_Garages_OwnerId",
                table: "Garage",
                newName: "IX_Garage_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_OwnerId",
                table: "Car",
                newName: "IX_Car_OwnerId");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Garage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Garage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Garage",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Garage",
                table: "Garage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car",
                table: "Car",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_AspNetUsers_OwnerId",
                table: "Car",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Garage_AspNetUsers_OwnerId",
                table: "Garage",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenance_Car_CarId",
                table: "Maintenance",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
