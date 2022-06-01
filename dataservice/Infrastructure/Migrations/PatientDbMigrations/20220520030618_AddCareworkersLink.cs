using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.PatientDbMigrations
{
    public partial class AddCareworkersLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CareWorkerId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "BirthDate", "CareWorkerId", "Email", "FirstName", "HomeNumber", "LastName", "PhoneNumber", "PostalCode", "Status" },
                values: new object[] { 2, new DateTime(1998, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "testtesteranderson@test.com", "Teste", "123", "Anderson", "0612346789", "1234 DE", "active" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "CareWorkerId",
                table: "Patients");
        }
    }
}
