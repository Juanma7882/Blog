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
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }


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

            modelBuilder.Entity<Rol>(rol =>
            {
                rol.ToTable("Roles");
                rol.HasKey(rol => rol.IdRol);
                rol.Property(rol => rol.IdRol).UseIdentityColumn().ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.ToTable("UsuariosRoles");
                entity.HasKey(usuariorol => usuariorol.IdUsuarioRol);

                entity.HasOne(usuariorol => usuariorol.Usuario)
                    .WithMany(usuario => usuario.UsuarioRoles)
                    .HasForeignKey(usuariorol => usuariorol.IdUsuario);

                entity.HasOne(usuariorol => usuariorol.Rol)
                    .WithMany(rol => rol.UsuarioRoles)
                    .HasForeignKey(usuariorol => usuariorol.IdRol);
            });

            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, NombreRol = "SuperAdministrador" },
                new Rol { IdRol = 2, NombreRol = "AdministrarUsuarios" },
                new Rol { IdRol = 3, NombreRol = "AdministrarBlog" }
             );
        }


    }
}
