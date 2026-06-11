using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalniKlubCitalaca.Migrations
{
    /// <inheritdoc />
    public partial class DodanaPdfPutanjaZaKnjigu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfPutanja",
                table: "Knjiga",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfPutanja",
                table: "Knjiga");
        }
    }
}
