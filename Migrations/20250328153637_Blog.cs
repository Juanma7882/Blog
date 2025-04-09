using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiBlog.Migrations
{
    /// <inheritdoc />
    public partial class Blog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreRol",
                table: "Roles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    IdBlog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDePublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Enlace = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.IdBlog);
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    IdEtiqueta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.IdEtiqueta);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioBlogs",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdBlog = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioBlogs", x => new { x.IdUsuario, x.IdBlog });
                    table.ForeignKey(
                        name: "FK_UsuarioBlogs_Blogs_IdBlog",
                        column: x => x.IdBlog,
                        principalTable: "Blogs",
                        principalColumn: "IdBlog",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioBlogs_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogEtiquetas",
                columns: table => new
                {
                    IdBlog = table.Column<int>(type: "int", nullable: false),
                    IdEtiqueta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogEtiquetas", x => new { x.IdBlog, x.IdEtiqueta });
                    table.ForeignKey(
                        name: "FK_BlogEtiquetas_Blogs_IdBlog",
                        column: x => x.IdBlog,
                        principalTable: "Blogs",
                        principalColumn: "IdBlog",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogEtiquetas_Etiquetas_IdEtiqueta",
                        column: x => x.IdEtiqueta,
                        principalTable: "Etiquetas",
                        principalColumn: "IdEtiqueta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogEtiquetas_IdEtiqueta",
                table: "BlogEtiquetas",
                column: "IdEtiqueta");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioBlogs_IdBlog",
                table: "UsuarioBlogs",
                column: "IdBlog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogEtiquetas");

            migrationBuilder.DropTable(
                name: "UsuarioBlogs");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "NombreRol",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
