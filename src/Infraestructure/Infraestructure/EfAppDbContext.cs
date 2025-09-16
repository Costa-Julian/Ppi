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
        public DbSet<Orden> ordens => Set<Orden>();
        public DbSet<Activo> activos => Set<Activo>();
        public DbSet<Estado> estado => Set<Estado>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orden>(b =>
            {
                b.ToTable("Ordenes");
                b.HasKey(o => o.Id);
                b.Property(o => o.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
