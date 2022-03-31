using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.MedicineDbMigrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoseUnits",
                columns: table => new
                {
                    Unit = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoseUnits", x => x.Unit);
                });

            migrationBuilder.CreateTable(
                name: "Medicine_Colors",
                columns: table => new
                {
                    Color = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine_Colors", x => x.Color);
                });

            migrationBuilder.CreateTable(
                name: "Medicine_Shapes",
                columns: table => new
                {
                    Shape = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine_Shapes", x => x.Shape);
                });

            migrationBuilder.CreateTable(
                name: "Medicine_Types",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine_Types", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Medicine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indication = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dose = table.Column<double>(type: "float", nullable: false),
                    DoseUnit = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Shape = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicine_DoseUnits_DoseUnit",
                        column: x => x.DoseUnit,
                        principalTable: "DoseUnits",
                        principalColumn: "Unit",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medicine_Medicine_Colors_Color",
                        column: x => x.Color,
                        principalTable: "Medicine_Colors",
                        principalColumn: "Color");
                    table.ForeignKey(
                        name: "FK_Medicine_Medicine_Shapes_Shape",
                        column: x => x.Shape,
                        principalTable: "Medicine_Shapes",
                        principalColumn: "Shape");
                    table.ForeignKey(
                        name: "FK_Medicine_Medicine_Types_Type",
                        column: x => x.Type,
                        principalTable: "Medicine_Types",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DoseUnits",
                column: "Unit",
                values: new object[]
                {
                    "mg",
                    "µg"
                });

            migrationBuilder.InsertData(
                table: "Medicine_Colors",
                column: "Color",
                values: new object[]
                {
                    "Blauw",
                    "Geel",
                    "Groen",
                    "Rood",
                    "Wit",
                    "Zwart"
                });

            migrationBuilder.InsertData(
                table: "Medicine_Shapes",
                column: "Shape",
                values: new object[]
                {
                    "Hexagonaal",
                    "Rond",
                    "Vierkant"
                });

            migrationBuilder.InsertData(
                table: "Medicine_Types",
                column: "Type",
                values: new object[]
                {
                    "Capsule",
                    "Pil",
                    "Spuit",
                    "Tablet"
                });

            migrationBuilder.InsertData(
                table: "Medicine",
                columns: new[] { "Id", "Color", "Dose", "DoseUnit", "Indication", "Name", "Shape", "Type" },
                values: new object[,]
                {
                    { 1, null, 10.0, "mg", "Hypercholesterolemie", "Rosuvastatine", null, "Tablet" },
                    { 2, null, 12.5, "mg", "Hypertensei", "Hydrochloorthiazide", null, "Tablet" },
                    { 3, null, 500.0, "mg", "Diabetes Mellitus type 2", "Metformine", null, "Tablet" },
                    { 4, null, 80.0, "mg", "Maagbeschermer", "Pantroprazol msr", null, "Tablet" },
                    { 5, null, 50.0, "mg", "Antibiotica (Urineweginfectie)", "Nitrofurantione", null, "Tablet" },
                    { 6, null, 10.0, "mg", "Somberheid", "Temazepam", null, "Tablet" },
                    { 7, null, 40.0, "mg", "Hartfalen", "Furosemide", null, "Tablet" },
                    { 8, null, 5.0, "mg", "Nycturie", "Finasteride", null, "Capsule" },
                    { 9, null, 10.0, "mg", "Slaapproblemen", "Oxazepam", null, "Tablet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_Color",
                table: "Medicine",
                column: "Color");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_DoseUnit",
                table: "Medicine",
                column: "DoseUnit");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_Shape",
                table: "Medicine",
                column: "Shape");

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_Type",
                table: "Medicine",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicine");

            migrationBuilder.DropTable(
                name: "DoseUnits");

            migrationBuilder.DropTable(
                name: "Medicine_Colors");

            migrationBuilder.DropTable(
                name: "Medicine_Shapes");

            migrationBuilder.DropTable(
                name: "Medicine_Types");
        }
    }
}
