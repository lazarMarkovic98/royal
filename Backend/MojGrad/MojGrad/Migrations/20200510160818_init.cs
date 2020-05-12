using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MojGrad.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    KorisnickoIme = table.Column<string>(nullable: true),
                    Lozinka = table.Column<string>(nullable: true),
                    Slika = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    FcmToken = table.Column<string>(nullable: true),
                    Ban = table.Column<DateTime>(nullable: false),
                    Rola = table.Column<int>(nullable: false),
                    Poeni = table.Column<int>(nullable: false),
                    Radius = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medalje",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Slika = table.Column<string>(nullable: true),
                    Naziv = table.Column<string>(nullable: true),
                    Opis = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medalje", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dogadjaji",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Datum = table.Column<DateTime>(nullable: false),
                    Naslov = table.Column<string>(nullable: true),
                    Slika = table.Column<string>(nullable: true),
                    Opis = table.Column<string>(nullable: true),
                    X = table.Column<double>(nullable: false),
                    Y = table.Column<double>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogadjaji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dogadjaji_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Izazovi",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Datum = table.Column<DateTime>(nullable: false),
                    Naslov = table.Column<string>(nullable: true),
                    KategorijaId = table.Column<long>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izazovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Izazovi_Kategorije_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Izazovi_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nadleznost",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KorisnikId = table.Column<long>(nullable: false),
                    KategorijaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nadleznost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nadleznost_Kategorije_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nadleznost_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DodeljeneMedalje",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MedaljaId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DodeljeneMedalje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DodeljeneMedalje_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DodeljeneMedalje_Medalje_MedaljaId",
                        column: x => x.MedaljaId,
                        principalTable: "Medalje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KomentariDogadjaja",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DogadjajId = table.Column<long>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Ocena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KomentariDogadjaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KomentariDogadjaja_Dogadjaji_DogadjajId",
                        column: x => x.DogadjajId,
                        principalTable: "Dogadjaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KomentariDogadjaja_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijaveDogadjaja",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DogadjajId = table.Column<long>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Datum = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijaveDogadjaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrijaveDogadjaja_Dogadjaji_DogadjajId",
                        column: x => x.DogadjajId,
                        principalTable: "Dogadjaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrijaveDogadjaja_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ucesnici",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KorisnikId = table.Column<long>(nullable: false),
                    DogadjajId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ucesnici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ucesnici_Dogadjaji_DogadjajId",
                        column: x => x.DogadjajId,
                        principalTable: "Dogadjaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ucesnici_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Komentari",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostId = table.Column<long>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Ocena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentari_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentari_Izazovi_PostId",
                        column: x => x.PostId,
                        principalTable: "Izazovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prijave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IzazovId = table.Column<long>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prijave_Izazovi_IzazovId",
                        column: x => x.IzazovId,
                        principalTable: "Izazovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prijave_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resenja",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IzazovId = table.Column<long>(nullable: false),
                    KorisnikId = table.Column<long>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Datum = table.Column<DateTime>(nullable: false),
                    Ocena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resenja_Izazovi_IzazovId",
                        column: x => x.IzazovId,
                        principalTable: "Izazovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resenja_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlikeIzazova",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IzazovId = table.Column<long>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlikeIzazova", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlikeIzazova_Izazovi_IzazovId",
                        column: x => x.IzazovId,
                        principalTable: "Izazovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlikeResenja",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResenjeId = table.Column<long>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlikeResenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlikeResenja_Resenja_ResenjeId",
                        column: x => x.ResenjeId,
                        principalTable: "Resenja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DodeljeneMedalje_KorisnikId",
                table: "DodeljeneMedalje",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_DodeljeneMedalje_MedaljaId",
                table: "DodeljeneMedalje",
                column: "MedaljaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogadjaji_KorisnikId",
                table: "Dogadjaji",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Izazovi_KategorijaId",
                table: "Izazovi",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Izazovi_KorisnikId",
                table: "Izazovi",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_KorisnikId",
                table: "Komentari",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_PostId",
                table: "Komentari",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_KomentariDogadjaja_DogadjajId",
                table: "KomentariDogadjaja",
                column: "DogadjajId");

            migrationBuilder.CreateIndex(
                name: "IX_KomentariDogadjaja_KorisnikId",
                table: "KomentariDogadjaja",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Nadleznost_KategorijaId",
                table: "Nadleznost",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Nadleznost_KorisnikId",
                table: "Nadleznost",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_IzazovId",
                table: "Prijave",
                column: "IzazovId");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_KorisnikId",
                table: "Prijave",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijaveDogadjaja_DogadjajId",
                table: "PrijaveDogadjaja",
                column: "DogadjajId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijaveDogadjaja_KorisnikId",
                table: "PrijaveDogadjaja",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Resenja_IzazovId",
                table: "Resenja",
                column: "IzazovId");

            migrationBuilder.CreateIndex(
                name: "IX_Resenja_KorisnikId",
                table: "Resenja",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_SlikeIzazova_IzazovId",
                table: "SlikeIzazova",
                column: "IzazovId");

            migrationBuilder.CreateIndex(
                name: "IX_SlikeResenja_ResenjeId",
                table: "SlikeResenja",
                column: "ResenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ucesnici_DogadjajId",
                table: "Ucesnici",
                column: "DogadjajId");

            migrationBuilder.CreateIndex(
                name: "IX_Ucesnici_KorisnikId",
                table: "Ucesnici",
                column: "KorisnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DodeljeneMedalje");

            migrationBuilder.DropTable(
                name: "Komentari");

            migrationBuilder.DropTable(
                name: "KomentariDogadjaja");

            migrationBuilder.DropTable(
                name: "Nadleznost");

            migrationBuilder.DropTable(
                name: "Prijave");

            migrationBuilder.DropTable(
                name: "PrijaveDogadjaja");

            migrationBuilder.DropTable(
                name: "SlikeIzazova");

            migrationBuilder.DropTable(
                name: "SlikeResenja");

            migrationBuilder.DropTable(
                name: "Ucesnici");

            migrationBuilder.DropTable(
                name: "Medalje");

            migrationBuilder.DropTable(
                name: "Resenja");

            migrationBuilder.DropTable(
                name: "Dogadjaji");

            migrationBuilder.DropTable(
                name: "Izazovi");

            migrationBuilder.DropTable(
                name: "Kategorije");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
