using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCapituloActualALectura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                table: "Lecturas");

            migrationBuilder.AddColumn<int>(
                name: "CapituloActualId",
                table: "Lecturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_CapituloActualId",
                table: "Lecturas",
                column: "CapituloActualId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Capitulos_CapituloActualId",
                table: "Lecturas",
                column: "CapituloActualId",
                principalTable: "Capitulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                table: "Lecturas",
                column: "FinalAlcanzadoId",
                principalTable: "Capitulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Capitulos_CapituloActualId",
                table: "Lecturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                table: "Lecturas");

            migrationBuilder.DropIndex(
                name: "IX_Lecturas_CapituloActualId",
                table: "Lecturas");

            migrationBuilder.DropColumn(
                name: "CapituloActualId",
                table: "Lecturas");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                table: "Lecturas",
                column: "FinalAlcanzadoId",
                principalTable: "Capitulos",
                principalColumn: "Id");
        }
    }
}
