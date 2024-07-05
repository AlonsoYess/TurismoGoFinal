using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turismo.Infraestructure.EFDataContext.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoUsuario",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Actividad",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Actividad");
        }
    }
}
