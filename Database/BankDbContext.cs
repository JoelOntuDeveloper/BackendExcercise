using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public partial class BankDbContext : DbContext
    {
        public BankDbContext()
        {
        }

        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Cuenta> Cuentas { get; set; } = null!;
        public virtual DbSet<Movimiento> Movimientos { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
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

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.ToTable("Cuenta");

                entity.HasKey(e => e.CuentaId)
                    .HasName("PK__Cuenta__40072E81D3C5F8A9");

                entity.HasIndex(e => e.NumeroCuenta, "UQ__Cuenta__E039507B74E3E813")
                    .IsUnique();

                entity.Property(e => e.NumeroCuenta).HasMaxLength(50);

                entity.Property(e => e.SaldoInicial).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TipoCuenta).HasMaxLength(50);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuenta_Cliente");
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.ToTable("Movimiento");

                entity.HasIndex(e => new { e.CuentaId, e.Fecha, e.TipoMovimiento }, "UQ_Movimiento")
                    .IsUnique();

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Saldo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TipoMovimiento).HasMaxLength(50);

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Cuenta)
                    .WithMany(p => p.Movimiento)
                    .HasForeignKey(d => d.CuentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movimiento_Cuenta");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
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
