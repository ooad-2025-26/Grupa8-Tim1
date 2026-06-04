using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalniKlubCitalaca.Migrations
{
    /// <inheritdoc />
    public partial class DodanaRegijaGradKorisnik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Regija",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Regija",
                table: "AspNetUsers");
        }
    }
}
