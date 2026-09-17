using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email_Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdministradorId = table.Column<int>(type: "int", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    CapituloId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Capitulos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ContadorAlcances = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capitulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sinopsis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    CapituloInicialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historias_Capitulos_CapituloInicialId",
                        column: x => x.CapituloInicialId,
                        principalTable: "Capitulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Opciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapituloDestinoId = table.Column<int>(type: "int", nullable: false),
                    CapituloIntermedioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opciones_Capitulos_CapituloDestinoId",
                        column: x => x.CapituloDestinoId,
                        principalTable: "Capitulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Opciones_Capitulos_CapituloIntermedioId",
                        column: x => x.CapituloIntermedioId,
                        principalTable: "Capitulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoriaCategorias",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriaCategorias", x => new { x.CategoriasId, x.HistoriaId });
                    table.ForeignKey(
                        name: "FK_HistoriaCategorias_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoriaCategorias_Historias_HistoriaId",
                        column: x => x.HistoriaId,
                        principalTable: "Historias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalAlcanzadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                        column: x => x.FinalAlcanzadoId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Historias_HistoriaId",
                        column: x => x.HistoriaId,
                        principalTable: "Historias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lecturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_AdministradorId",
                table: "Auditorias",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_CapituloId",
                table: "Auditorias",
                column: "CapituloId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_HistoriaId",
                table: "Auditorias",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Capitulos_HistoriaId",
                table: "Capitulos",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriaCategorias_HistoriaId",
                table: "HistoriaCategorias",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_CapituloInicialId",
                table: "Historias",
                column: "CapituloInicialId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_FinalAlcanzadoId",
                table: "Lecturas",
                column: "FinalAlcanzadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_HistoriaId",
                table: "Lecturas",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_UsuarioId",
                table: "Lecturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_CapituloDestinoId",
                table: "Opciones",
                column: "CapituloDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_CapituloIntermedioId",
                table: "Opciones",
                column: "CapituloIntermedioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditorias_Capitulos_CapituloId",
                table: "Auditorias",
                column: "CapituloId",
                principalTable: "Capitulos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditorias_Historias_HistoriaId",
                table: "Auditorias",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Capitulos_Historias_HistoriaId",
                table: "Capitulos",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_Capitulos_CapituloInicialId",
                table: "Historias");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "HistoriaCategorias");

            migrationBuilder.DropTable(
                name: "Lecturas");

            migrationBuilder.DropTable(
                name: "Opciones");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Capitulos");

            migrationBuilder.DropTable(
                name: "Historias");
        }
    }
}
