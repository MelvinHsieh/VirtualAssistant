using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.PatientDbMigrations
{
    public partial class PatientLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PatientLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLocations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PatientLocations",
                columns: new[] { "Id", "Department", "RoomId", "Status" },
                values: new object[] { 1, "TestLocatie", "1", "active" });

            migrationBuilder.InsertData(
                table: "PatientLocations",
                columns: new[] { "Id", "Department", "RoomId", "Status" },
                values: new object[] { 2, "TestLocatie", "2", "active" });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_LocationId",
                table: "Patients",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientLocations_LocationId",
                table: "Patients",
                column: "LocationId",
                principalTable: "PatientLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientLocations_LocationId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "PatientLocations");

            migrationBuilder.DropIndex(
                name: "IX_Patients_LocationId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Patients");
        }
    }
}
