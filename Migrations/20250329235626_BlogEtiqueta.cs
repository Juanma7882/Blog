using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiBlog.Migrations
{
    /// <inheritdoc />
    public partial class BlogEtiqueta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioBlogs");

            migrationBuilder.AddColumn<int>(
                name: "IdBlogEtiqueta",
                table: "BlogEtiquetas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdBlogEtiqueta",
                table: "BlogEtiquetas");

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

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioBlogs_IdBlog",
                table: "UsuarioBlogs",
                column: "IdBlog");
        }
    }
}
