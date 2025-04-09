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
        public DbSet<Blog> Blogs { get; set; }
        //public DbSet<UsuarioBlog> usuarioBlogs { get; set; }
        public DbSet<Etiqueta> etiquetas { get; set; }
        public DbSet<BlogEtiqueta> blogEtiquetas { get; set; }


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
                tb.Property(usuario => usuario.Dni).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<Rol>(rol =>
            {
                rol.ToTable("Roles");
                rol.HasKey(rol => rol.IdRol);
                rol.Property(rol => rol.IdRol).UseIdentityColumn().ValueGeneratedOnAdd();
                rol.Property(rol => rol.NombreRol).HasMaxLength(100).IsRequired();
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

            modelBuilder.Entity<Blog>(blog =>
            {
                blog.ToTable("Blogs");
                blog.HasKey(b => b.IdBlog);
                blog.Property(b => b.IdBlog).UseIdentityColumn().ValueGeneratedOnAdd();

                blog.Property(b => b.Titulo).HasMaxLength(255).IsRequired();
                blog.Property(b => b.Contenido).IsRequired();
                blog.Property(b => b.FechaDePublicacion).IsRequired();
                blog.Property(b => b.Descripcion).HasMaxLength(500).IsRequired();
                blog.Property(b => b.Enlace).IsRequired();
                
            
            });


            //modelBuilder.Entity<UsuarioBlog>(entity =>
            //{
            //    entity.ToTable("UsuarioBlogs");
            //    entity.HasKey(ub => new { ub.IdUsuario, ub.IdBlog }); // Clave compuesta

            //    entity.HasOne(ub => ub.Usuario)
            //        .WithMany(u => u.UsuarioBlog)
            //        .HasForeignKey(ub => ub.IdUsuario);

            //    entity.HasOne(ub => ub.blog)
            //        .WithMany(b => b.usuarioBlog)
            //        .HasForeignKey(ub => ub.IdBlog);
            //});

            modelBuilder.Entity<Etiqueta>(entity =>
            {
                entity.ToTable("Etiquetas");
                entity.HasKey(e => e.IdEtiqueta);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<BlogEtiqueta>(entity =>
            {
                entity.ToTable("BlogEtiquetas");
                entity.HasKey(be => new { be.IdBlog, be.IdEtiqueta });

                entity.HasOne(be => be.Blog)
                    .WithMany(b => b.BlogEtiquetas)
                    .HasForeignKey(be => be.IdBlog);

                entity.HasOne(be => be.Etiqueta)
                    .WithMany(e => e.BlogEtiquetas)
                    .HasForeignKey(be => be.IdEtiqueta);
            });
        }


    }
}
