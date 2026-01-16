using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Datos;

public class ContextoBaseDatos : DbContext
{
    public ContextoBaseDatos(DbContextOptions<ContextoBaseDatos> opciones)
        : base(opciones)
    {
    }

    public DbSet<Estudiante> estudiantes { get; set; } = null!;
    public DbSet<Profesor> profesores { get; set; } = null!;
    public DbSet<Nota> notas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(entidad =>
        {
            entidad.ToTable("Estudiantes");
            entidad.HasKey(e => e.id);
            entidad.Property(e => e.id).HasColumnName("id").ValueGeneratedOnAdd();
            entidad.Property(e => e.nombre).HasColumnName("nombre").IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Profesor>(entidad =>
        {
            entidad.ToTable("Profesores");
            entidad.HasKey(p => p.id);
            entidad.Property(p => p.id).HasColumnName("id").ValueGeneratedOnAdd();
            entidad.Property(p => p.nombre).HasColumnName("nombre").IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Nota>(entidad =>
        {
            entidad.ToTable("Notas");
            entidad.HasKey(n => n.id);
            entidad.Property(n => n.id).HasColumnName("id").ValueGeneratedOnAdd();
            entidad.Property(n => n.nombre).HasColumnName("nombre").IsRequired().HasMaxLength(200);
            entidad.Property(n => n.valor).HasColumnName("valor").HasColumnType("decimal(18,2)");
            entidad.Property(n => n.idProfesor).HasColumnName("idProfesor").IsRequired();
            entidad.Property(n => n.idEstudiante).HasColumnName("idEstudiante").IsRequired();

            entidad.HasOne(n => n.profesor)
                .WithMany(p => p.notas)
                .HasForeignKey(n => n.idProfesor)
                .OnDelete(DeleteBehavior.Restrict);

            entidad.HasOne(n => n.estudiante)
                .WithMany(e => e.notas)
                .HasForeignKey(n => n.idEstudiante)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
