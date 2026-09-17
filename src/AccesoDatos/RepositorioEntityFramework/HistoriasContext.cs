using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework
{
    internal class HistoriasContext : DbContext
    {
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Capitulo> Capitulos { get; set; }
        public DbSet<Historia> Historias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Lectura> Lecturas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<CapituloFinal> CapituloFinales { get; set; }
        public DbSet<CapituloIntermedio> CapituloIntermedios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"SERVER=(localdb)\MsSqlLocalDb;Database=HistoriasDB;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Historia>()
                .HasOne(h => h.CapituloInicial)
                .WithMany()
                .HasForeignKey(h => h.CapituloInicialId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Historia>()
                .HasMany(h => h.Categorias)
                .WithMany()
                .UsingEntity(j => j.ToTable("HistoriaCategorias"));

            modelBuilder.Entity<CapituloIntermedio>()
                .HasMany(ci => ci.Opciones)
                .WithOne()
                .HasForeignKey("CapituloIntermedioId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Opcion>()
                .HasOne(o => o.CapituloDestino)
                .WithMany()
                .HasForeignKey(o => o.CapituloDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
