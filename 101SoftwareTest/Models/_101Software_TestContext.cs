using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace _101SoftwareTest.Models
{
    public partial class _101Software_TestContext : DbContext
    {
        public _101Software_TestContext()
        {
        }

        public _101Software_TestContext(DbContextOptions<_101Software_TestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyImage> PropertyImages { get; set; }
        public virtual DbSet<PropertyTrace> PropertyTraces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=JHAD2807;Database=101Software_Test;user=sa;password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("OWNER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Birthday)
                    .HasColumnType("datetime")
                    .HasColumnName("BIRTHDAY");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Photo)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("PROPERTIES");

                entity.HasIndex(e => e.CodeInternal, "UQ__PROPERTI__9C1D4D11A5706D12")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.CodeInternal)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE_INTERNAL");

                entity.Property(e => e.IdOwner).HasColumnName("ID_OWNER");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Year).HasColumnName("YEAR");

                entity.HasOne(d => d.IdOwnerNavigation)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.IdOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OWNER");
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.ToTable("PROPERTY_IMAGE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Enabled).HasColumnName("ENABLED");

                entity.Property(e => e.Files)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FILES");

                entity.Property(e => e.IdPropiedad).HasColumnName("ID_PROPERTY");

                entity.HasOne(d => d.IdPropertyNavigation)
                    .WithMany(p => p.PropertyImages)
                    .HasForeignKey(d => d.IdPropiedad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROPERTY");

            });

            modelBuilder.Entity<PropertyTrace>(entity =>
            {
                entity.ToTable("PROPERTY_TRACE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateSale)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE_SALE");

                entity.Property(e => e.IdProperty).HasColumnName("ID_PROPERTY");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Tax)
                    .HasColumnType("money")
                    .HasColumnName("TAX");

                entity.Property(e => e.Value)
                    .HasColumnType("money")
                    .HasColumnName("VALUE");

                entity.HasOne(d => d.IdPropertyNavigation)
                    .WithMany(p => p.PropertyTraces)
                    .HasForeignKey(d => d.IdProperty)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROPERTY_TRACE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
