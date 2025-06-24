using AgenciaTurismo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AgenciaTurismo.Data
{
    public class AgenciaTurismoDbContext : IdentityDbContext
    {
        public AgenciaTurismoDbContext(DbContextOptions<AgenciaTurismoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<PacoteTuristico> PacotesTuristicos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Destino>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<PacoteTuristico>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Reserva>().HasQueryFilter(r => !r.IsDeleted);

            modelBuilder.Entity<Reserva>()
                .Property(r => r.DataReserva)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.PacoteTuristico)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PacoteTuristicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PacoteTuristico>()
                .HasOne(p => p.Destino)
                .WithMany(d => d.PacotesTuristicos)
                .HasForeignKey(p => p.DestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "UserTokens");
            });
        }
    }
}