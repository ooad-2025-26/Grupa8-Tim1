using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Migrations
{
    /// <inheritdoc />
    public partial class PocetnaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CitalackaGrupa",
                columns: table => new
                {
                    GrupaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zanr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Regija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaksimalanBrojClanova = table.Column<int>(type: "int", nullable: false),
                    TipGrupe = table.Column<int>(type: "int", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitalackaGrupa", x => x.GrupaId);
                });

            migrationBuilder.CreateTable(
                name: "Knjiga",
                columns: table => new
                {
                    KnjigaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojStranica = table.Column<int>(type: "int", nullable: false),
                    Zanr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjiga", x => x.KnjigaId);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilnaSlika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LokacijaX = table.Column<double>(type: "float", nullable: false),
                    LokacijaY = table.Column<double>(type: "float", nullable: false),
                    Uloga = table.Column<int>(type: "int", nullable: false),
                    StatusNaloga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.KorisnikId);
                });

            migrationBuilder.CreateTable(
                name: "KnjigaGrupa",
                columns: table => new
                {
                    KnjigaGrupaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnjigaId = table.Column<int>(type: "int", nullable: false),
                    GrupaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnjigaGrupa", x => x.KnjigaGrupaId);
                    table.ForeignKey(
                        name: "FK_KnjigaGrupa_CitalackaGrupa_GrupaId",
                        column: x => x.GrupaId,
                        principalTable: "CitalackaGrupa",
                        principalColumn: "GrupaId");
                    table.ForeignKey(
                        name: "FK_KnjigaGrupa_Knjiga_KnjigaId",
                        column: x => x.KnjigaId,
                        principalTable: "Knjiga",
                        principalColumn: "KnjigaId");
                });

            migrationBuilder.CreateTable(
                name: "ClanstvoGrupe",
                columns: table => new
                {
                    ClanstvoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    GrupaId = table.Column<int>(type: "int", nullable: false),
                    DatumPridruzivanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusClanstva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanstvoGrupe", x => x.ClanstvoId);
                    table.ForeignKey(
                        name: "FK_ClanstvoGrupe_CitalackaGrupa_GrupaId",
                        column: x => x.GrupaId,
                        principalTable: "CitalackaGrupa",
                        principalColumn: "GrupaId");
                    table.ForeignKey(
                        name: "FK_ClanstvoGrupe_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId");
                });

            migrationBuilder.CreateTable(
                name: "Profil",
                columns: table => new
                {
                    ProfilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    OpisProfila = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lokacija = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profil", x => x.ProfilId);
                    table.ForeignKey(
                        name: "FK_Profil_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId");
                });

            migrationBuilder.CreateTable(
                name: "SadrzajGrupe",
                columns: table => new
                {
                    SadrzajId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    GrupaId = table.Column<int>(type: "int", nullable: false),
                    DatumObjave = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipSadrzaja = table.Column<int>(type: "int", nullable: false),
                    StatusSadrzaja = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SadrzajGrupe", x => x.SadrzajId);
                    table.ForeignKey(
                        name: "FK_SadrzajGrupe_CitalackaGrupa_GrupaId",
                        column: x => x.GrupaId,
                        principalTable: "CitalackaGrupa",
                        principalColumn: "GrupaId");
                    table.ForeignKey(
                        name: "FK_SadrzajGrupe_Korisnik_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId");
                });

            migrationBuilder.CreateTable(
                name: "ZahtjevZaPristup",
                columns: table => new
                {
                    ZahtjevId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    GrupaId = table.Column<int>(type: "int", nullable: false),
                    DatumZahtjeva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusZahtjeva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZahtjevZaPristup", x => x.ZahtjevId);
                    table.ForeignKey(
                        name: "FK_ZahtjevZaPristup_CitalackaGrupa_GrupaId",
                        column: x => x.GrupaId,
                        principalTable: "CitalackaGrupa",
                        principalColumn: "GrupaId");
                    table.ForeignKey(
                        name: "FK_ZahtjevZaPristup_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId");
                });

            migrationBuilder.CreateTable(
                name: "Komentar",
                columns: table => new
                {
                    KomentarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SadrzajId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumKomentara = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusKomentara = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentar", x => x.KomentarId);
                    table.ForeignKey(
                        name: "FK_Komentar_Korisnik_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId");
                    table.ForeignKey(
                        name: "FK_Komentar_SadrzajGrupe_SadrzajId",
                        column: x => x.SadrzajId,
                        principalTable: "SadrzajGrupe",
                        principalColumn: "SadrzajId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClanstvoGrupe_GrupaId",
                table: "ClanstvoGrupe",
                column: "GrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanstvoGrupe_KorisnikId",
                table: "ClanstvoGrupe",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_KnjigaGrupa_GrupaId",
                table: "KnjigaGrupa",
                column: "GrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_KnjigaGrupa_KnjigaId",
                table: "KnjigaGrupa",
                column: "KnjigaId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_AutorId",
                table: "Komentar",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_SadrzajId",
                table: "Komentar",
                column: "SadrzajId");

            migrationBuilder.CreateIndex(
                name: "IX_Profil_KorisnikId",
                table: "Profil",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_SadrzajGrupe_AutorId",
                table: "SadrzajGrupe",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_SadrzajGrupe_GrupaId",
                table: "SadrzajGrupe",
                column: "GrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_ZahtjevZaPristup_GrupaId",
                table: "ZahtjevZaPristup",
                column: "GrupaId");

            migrationBuilder.CreateIndex(
                name: "IX_ZahtjevZaPristup_KorisnikId",
                table: "ZahtjevZaPristup",
                column: "KorisnikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanstvoGrupe");

            migrationBuilder.DropTable(
                name: "KnjigaGrupa");

            migrationBuilder.DropTable(
                name: "Komentar");

            migrationBuilder.DropTable(
                name: "Profil");

            migrationBuilder.DropTable(
                name: "ZahtjevZaPristup");

            migrationBuilder.DropTable(
                name: "Knjiga");

            migrationBuilder.DropTable(
                name: "SadrzajGrupe");

            migrationBuilder.DropTable(
                name: "CitalackaGrupa");

            migrationBuilder.DropTable(
                name: "Korisnik");
        }
    }
}
