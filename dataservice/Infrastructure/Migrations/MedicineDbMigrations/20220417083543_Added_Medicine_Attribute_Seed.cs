using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    public partial class Added_Medicine_Attribute_Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Wit", "Vierkant" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Rood", "Rond" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Wit", "Vierkant" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Blauw", "Vierkant" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Wit", "Vierkant" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Groen", "Rond" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Wit", "Hexagonaal" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Zwart", "Rond" });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Color", "Shape" },
                values: new object[] { "Wit", "Vierkant" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Color", "Shape" },
                values: new object[] { null, null });
        }
    }
}
