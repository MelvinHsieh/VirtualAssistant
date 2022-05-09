using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    public partial class Added_EntityStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PatientIntakes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Medicine_Types",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Medicine_Shapes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Medicine_Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Medicine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DoseUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DoseUnits",
                keyColumn: "Unit",
                keyValue: "mg",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "DoseUnits",
                keyColumn: "Unit",
                keyValue: "µg",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 5,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 6,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 7,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 8,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 9,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Colors",
                keyColumn: "Color",
                keyValue: "Blauw",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Colors",
                keyColumn: "Color",
                keyValue: "Geel",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Colors",
                keyColumn: "Color",
                keyValue: "Groen",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Colors",
                keyColumn: "Color",
                keyValue: "Rood",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Colors",
                keyColumn: "Color",
                keyValue: "Wit",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Colors",
                keyColumn: "Color",
                keyValue: "Zwart",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Shapes",
                keyColumn: "Shape",
                keyValue: "Hexagonaal",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Shapes",
                keyColumn: "Shape",
                keyValue: "Rond",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Shapes",
                keyColumn: "Shape",
                keyValue: "Vierkant",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Types",
                keyColumn: "Type",
                keyValue: "Capsule",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Types",
                keyColumn: "Type",
                keyValue: "Pil",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Types",
                keyColumn: "Type",
                keyValue: "Spuit",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "Medicine_Types",
                keyColumn: "Type",
                keyValue: "Tablet",
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Status",
                value: "active");

            migrationBuilder.UpdateData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Status",
                value: "active");

            migrationBuilder.CreateIndex(
                name: "IX_PatientIntakes_MedicineId",
                table: "PatientIntakes",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientIntakes_Medicine_MedicineId",
                table: "PatientIntakes",
                column: "MedicineId",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientIntakes_Medicine_MedicineId",
                table: "PatientIntakes");

            migrationBuilder.DropIndex(
                name: "IX_PatientIntakes_MedicineId",
                table: "PatientIntakes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PatientIntakes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Medicine_Types");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Medicine_Shapes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Medicine_Colors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Medicine");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DoseUnits");
        }
    }
}
