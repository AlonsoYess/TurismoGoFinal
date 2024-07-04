using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turismo.Infraestructure.EFDataContext.Migrations
{
    /// <inheritdoc />
    public partial class CorrigiendoRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favoritos_AspNetUsers_UsuarioId1",
                table: "Favoritos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_AspNetUsers_UsuarioId1",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Resenia_AspNetUsers_UsuarioId1",
                table: "Resenia");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_AspNetUsers_UsuarioId1",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_UsuarioId1",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Resenia_UsuarioId1",
                table: "Resenia");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_UsuarioId1",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Favoritos_UsuarioId1",
                table: "Favoritos");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Reserva");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Resenia");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Favoritos");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Reserva",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Resenia",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Notificaciones",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Favoritos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UsuarioId",
                table: "Reserva",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Resenia_UsuarioId",
                table: "Resenia",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_UsuarioId",
                table: "Favoritos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritos_AspNetUsers_UsuarioId",
                table: "Favoritos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_AspNetUsers_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resenia_AspNetUsers_UsuarioId",
                table: "Resenia",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_AspNetUsers_UsuarioId",
                table: "Reserva",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favoritos_AspNetUsers_UsuarioId",
                table: "Favoritos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_AspNetUsers_UsuarioId",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Resenia_AspNetUsers_UsuarioId",
                table: "Resenia");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_AspNetUsers_UsuarioId",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Reserva_UsuarioId",
                table: "Reserva");

            migrationBuilder.DropIndex(
                name: "IX_Resenia_UsuarioId",
                table: "Resenia");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Favoritos_UsuarioId",
                table: "Favoritos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Reserva",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Reserva",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Resenia",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Resenia",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Notificaciones",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Notificaciones",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Favoritos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Favoritos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UsuarioId1",
                table: "Reserva",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Resenia_UsuarioId1",
                table: "Resenia",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId1",
                table: "Notificaciones",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_UsuarioId1",
                table: "Favoritos",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritos_AspNetUsers_UsuarioId1",
                table: "Favoritos",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_AspNetUsers_UsuarioId1",
                table: "Notificaciones",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resenia_AspNetUsers_UsuarioId1",
                table: "Resenia",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_AspNetUsers_UsuarioId1",
                table: "Reserva",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
