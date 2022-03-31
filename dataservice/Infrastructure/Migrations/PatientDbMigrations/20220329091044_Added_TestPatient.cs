using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.PatientDbMigrations
{
    public partial class Added_TestPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "BirthDate", "Email", "FirstName", "HomeNumber", "LastName", "PhoneNumber", "PostalCode" },
                values: new object[] { 1, new DateTime(1993, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "testtester@test.com", "Test", "215", "Tester", "0687654321", "5223 DE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
