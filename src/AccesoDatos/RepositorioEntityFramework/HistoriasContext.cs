using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.RepositorioEntityFramework
{
    public class HistoriasContext : DbContext
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

        public HistoriasContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

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

            // Lectura referencia Capitulo dos veces (CapituloActual y FinalAlcanzado).
            modelBuilder.Entity<Lectura>()
                .HasOne(l => l.CapituloActual)
                .WithMany()
                .HasForeignKey(l => l.CapituloActualId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lectura>()
                .HasOne(l => l.FinalAlcanzado)
                .WithMany()
                .HasForeignKey(l => l.FinalAlcanzadoId)
                .OnDelete(DeleteBehavior.Restrict);

            // La auditoria sobrevive a la baja de su historia: el repositorio pone las FKs en null.
            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.Historia)
                .WithMany()
                .HasForeignKey(a => a.HistoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.Capitulo)
                .WithMany()
                .HasForeignKey(a => a.CapituloId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
