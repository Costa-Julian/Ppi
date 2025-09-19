using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class EfAppDbContext(DbContextOptions<EfAppDbContext> options) : DbContext(options)
    {
        public DbSet<Orden> Ordenes => Set<Orden>();
        public DbSet<Activo> Activos => Set<Activo>();
        public DbSet<Estado> Estados => Set<Estado>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Orden>();
            o.ToTable("Ordenes");
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Estado>(b =>
            {
                b.ToTable("Estados");
                b.Property(e => e.Id).ValueGeneratedNever();
            }
            );


            var a = modelBuilder.Entity<Activo>();
            a.ToTable("Activos");
            a.HasKey(x => x.Id);
            a.Property(x => x.Id).ValueGeneratedOnAdd();

            a.Property(x => x.Ticker)
             .HasColumnName("Ticker")
             .IsRequired().HasMaxLength(32);

            a.Property(x => x.Nombre)
             .HasColumnName("Nombre")
             .IsRequired().HasMaxLength(128);

            a.Ignore(x => x.TipoActivo);            

            a.HasDiscriminator<int>("TipoActivo")
             .HasValue<Activo>(0)
             .HasValue<Accion>(1)
             .HasValue<Bono>(2)
             .HasValue<Fci>(3);
        }
    }
}
