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

        // CAMBIO: sin esto, las convenciones por defecto de EF armaban mal el modelo:
        // - Historia.CapituloInicial generaba una FK "fantasma" duplicada (CapituloInicialId1)
        //   en vez de usar la columna CapituloInicialId que ya existe en el dominio.
        // - Historia<->Categoria quedaba uno-a-muchos (una categoría solo podía pertenecer
        //   a una historia), cuando la regla de negocio es muchos-a-muchos.
        // - CapituloIntermedio->Opciones quedaba opcional en vez de obligatoria.
        // Los OnDelete en Restrict evitan el error de SQL Server por múltiples cascade paths
        // hacia la tabla Capitulos (TPH compartida por CapituloIntermedio y CapituloFinal).
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
