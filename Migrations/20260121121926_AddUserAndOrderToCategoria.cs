using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlDeGastos.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndOrderToCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GastoAutomatico",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Categoria",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Categoria",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GastoAutomatico_UserId",
                table: "GastoAutomatico",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_UserId",
                table: "Categoria",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categoria_User_UserId",
                table: "Categoria",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GastoAutomatico_User_UserId",
                table: "GastoAutomatico",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_User_UserId",
                table: "Categoria");

            migrationBuilder.DropForeignKey(
                name: "FK_GastoAutomatico_User_UserId",
                table: "GastoAutomatico");

            migrationBuilder.DropIndex(
                name: "IX_GastoAutomatico_UserId",
                table: "GastoAutomatico");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_UserId",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GastoAutomatico");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categoria");
        }
    }
}
