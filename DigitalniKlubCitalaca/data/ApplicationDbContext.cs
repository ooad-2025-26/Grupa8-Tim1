using DigitalniKlubCitalaca.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DigitalniKlubCitalaca.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Profil> Profili { get; set; }
        public DbSet<CitalackaGrupa> CitalackeGrupe { get; set; }
        public DbSet<ClanstvoGrupe> ClanstvaGrupe { get; set; }
        public DbSet<ZahtjevZaPristup> ZahtjeviZaPristup { get; set; }
        public DbSet<SadrzajGrupe> SadrzajiGrupe { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<Knjiga> Knjige { get; set; }
        public DbSet<KnjigaGrupa> KnjigeGrupe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Profil>().ToTable("Profil");
            modelBuilder.Entity<CitalackaGrupa>().ToTable("CitalackaGrupa");
            modelBuilder.Entity<ClanstvoGrupe>().ToTable("ClanstvoGrupe");
            modelBuilder.Entity<ZahtjevZaPristup>().ToTable("ZahtjevZaPristup");
            modelBuilder.Entity<SadrzajGrupe>().ToTable("SadrzajGrupe");
            modelBuilder.Entity<Komentar>().ToTable("Komentar");
            modelBuilder.Entity<Knjiga>().ToTable("Knjiga");
            modelBuilder.Entity<KnjigaGrupa>().ToTable("KnjigaGrupa");

            modelBuilder.Entity<Profil>()
                .HasOne(p => p.Korisnik)
                .WithMany()
                .HasForeignKey(p => p.KorisnikId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClanstvoGrupe>()
                .HasOne(c => c.Korisnik)
                .WithMany()
                .HasForeignKey(c => c.KorisnikId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClanstvoGrupe>()
                .HasOne(c => c.CitalackaGrupa)
                .WithMany()
                .HasForeignKey(c => c.GrupaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ZahtjevZaPristup>()
                .HasOne(z => z.Korisnik)
                .WithMany()
                .HasForeignKey(z => z.KorisnikId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ZahtjevZaPristup>()
                .HasOne(z => z.CitalackaGrupa)
                .WithMany()
                .HasForeignKey(z => z.GrupaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SadrzajGrupe>()
                .HasOne(s => s.Autor)
                .WithMany()
                .HasForeignKey(s => s.AutorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SadrzajGrupe>()
                .HasOne(s => s.CitalackaGrupa)
                .WithMany()
                .HasForeignKey(s => s.GrupaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Komentar>()
                .HasOne(k => k.Autor)
                .WithMany()
                .HasForeignKey(k => k.AutorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Komentar>()
                .HasOne(k => k.SadrzajGrupe)
                .WithMany()
                .HasForeignKey(k => k.SadrzajId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KnjigaGrupa>()
                .HasOne(kg => kg.Knjiga)
                .WithMany()
                .HasForeignKey(kg => kg.KnjigaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KnjigaGrupa>()
                .HasOne(kg => kg.CitalackaGrupa)
                .WithMany()
                .HasForeignKey(kg => kg.GrupaId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}