using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.PatientDbMigrations
{
    public partial class patientlocation_link : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientLocations_LocationId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_LocationId",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "PatientLocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientLocations_PatientId",
                table: "PatientLocations",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLocations_Patients_PatientId",
                table: "PatientLocations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLocations_Patients_PatientId",
                table: "PatientLocations");

            migrationBuilder.DropIndex(
                name: "IX_PatientLocations_PatientId",
                table: "PatientLocations");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PatientLocations");

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
    }
}
