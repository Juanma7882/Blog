using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiBlog.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionEnRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 2,
                column: "NombreRol",
                value: "AdministrarUsuarios");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 3,
                column: "NombreRol",
                value: "AdministrarBlog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 2,
                column: "NombreRol",
                value: "Administrar Usuarios");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "IdRol",
                keyValue: 3,
                column: "NombreRol",
                value: "Administrar Blog");
        }
    }
}
