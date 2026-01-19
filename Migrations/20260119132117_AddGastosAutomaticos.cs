using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlDeGastos.Migrations
{
    /// <inheritdoc />
    public partial class AddGastosAutomaticos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GastoAutomatico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Importe = table.Column<decimal>(type: "TEXT", nullable: false),
                    Concepto = table.Column<string>(type: "TEXT", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoMovimiento = table.Column<int>(type: "INTEGER", nullable: false),
                    DiaDelMes = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    UltimoMesGenerado = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastoAutomatico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GastoAutomatico_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GastoAutomatico_CategoriaId",
                table: "GastoAutomatico",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GastoAutomatico");
        }
    }
}
