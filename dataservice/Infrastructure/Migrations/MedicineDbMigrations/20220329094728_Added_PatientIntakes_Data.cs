using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    public partial class Added_PatientIntakes_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PatientIntakes",
                columns: new[] { "Id", "Amount", "IntakeEnd", "IntakeStart", "MedicineId", "PatientId" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 1, 1 },
                    { 2, 1, new TimeSpan(0, 11, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 2, 1 },
                    { 3, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 12, 0, 0, 0), 2, 1 },
                    { 4, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 3, 1 },
                    { 5, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 4, 1 },
                    { 6, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 5, 1 },
                    { 7, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 6, 1 },
                    { 8, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 7, 1 },
                    { 9, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 8, 1 },
                    { 10, 1, new TimeSpan(0, 23, 59, 59, 0), new TimeSpan(0, 0, 0, 0, 0), 9, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PatientIntakes",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
