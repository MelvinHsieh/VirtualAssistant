using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    public partial class AddedImageURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Medicine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");

            migrationBuilder.UpdateData(
                table: "Medicine",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageURL",
                value: "https://va-cdn.azureedge.net/images/Paracetamol-1.jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Medicine");
        }
    }
}
