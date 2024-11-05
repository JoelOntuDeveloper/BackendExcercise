using Core.Entities;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Customers.API.IntegrationTest {
    public partial class TestDbContext : DbContext {

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Cliente>(entity => {
                entity.ToTable("Cliente");

                entity.HasIndex(e => e.ClienteId, "UQ__Cliente__71ABD086231611E0")
                    .IsUnique();

                entity.Property(e => e.ClienteId).ValueGeneratedNever();

                entity.Property(e => e.Contrasenia).HasMaxLength(255);

                entity.HasOne(d => d.Persona)
                    .WithOne(p => p.Cliente)
                    .HasForeignKey<Cliente>(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_Persona");
            });

            modelBuilder.Entity<Persona>(entity => {
                entity.ToTable("Persona");

                entity.HasIndex(e => e.Identificacion, "UQ__Persona__D6F931E5EFBE8DBA")
                    .IsUnique();

                entity.Property(e => e.Direccion).HasMaxLength(255);

                entity.Property(e => e.Genero)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Identificacion).HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
