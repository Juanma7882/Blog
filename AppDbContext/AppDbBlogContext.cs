using Microsoft.EntityFrameworkCore;
using MiBlog.Entities;
namespace MiBlog.AppDbContext
{
    public class AppDbBlogContext : DbContext
    {
        public AppDbBlogContext()
        {
        }

        public AppDbBlogContext(DbContextOptions<AppDbBlogContext>options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.ToTable("Usuarios");
                tb.HasKey(usuario => usuario.IdPersona);
                tb.Property(usuario => usuario.IdPersona).UseIdentityColumn().ValueGeneratedOnAdd();

                tb.HasIndex(u => u.NombreUsuario).IsUnique();
                tb.Property(usuario => usuario.Clave).HasMaxLength(50).IsRequired();
                tb.Property(usuario => usuario.Nombre).HasMaxLength(50).IsRequired();
                tb.Property(usuario => usuario.Apellido).HasMaxLength(50).IsRequired();
                tb.Property(usuario => usuario.Email).HasMaxLength(50).IsRequired();
                tb.Property(usuario => usuario.Dni).IsRequired();
            });
        }


    }
}
