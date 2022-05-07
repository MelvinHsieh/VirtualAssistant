using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    public partial class AddedCareWorkers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CareWorkerFunctions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareWorkerFunctions", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "CareWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Function = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareWorkers_CareWorkerFunctions_Function",
                        column: x => x.Function,
                        principalTable: "CareWorkerFunctions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CareWorkerFunctions",
                columns: new[] { "Name", "Status" },
                values: new object[] { "Dokter", "active" });

            migrationBuilder.InsertData(
                table: "CareWorkerFunctions",
                columns: new[] { "Name", "Status" },
                values: new object[] { "Verpleger", "active" });

            migrationBuilder.InsertData(
                table: "CareWorkers",
                columns: new[] { "Id", "FirstName", "Function", "LastName", "Status" },
                values: new object[,]
                {
                    { 1, "Petra", "Verpleger", "Janssen", "active" },
                    { 2, "Henny", "Verpleger", "Heeren", "active" },
                    { 3, "Peter", "Dokter", "Peters", "active" },
                    { 4, "Frida", "Dokter", "Leuken", "active" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareWorkers_Function",
                table: "CareWorkers",
                column: "Function");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareWorkers");

            migrationBuilder.DropTable(
                name: "CareWorkerFunctions");
        }
    }
}
