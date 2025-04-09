using MiBlog.AppDbContext;
using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Enums;
using MiBlog.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiBlog.Mapper
{
    public class MapperClass
    {
        readonly AppDbBlogContext _appDbContext;
        
        public MapperClass(AppDbBlogContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // MODELOS DE MAPEO
        // rol -> rolDTO

        // usuario <-> usuarioDTO 
        // usuario <-> sesionDTO
        // LoginDTO -> sesionDTO

        // eitqueta <-> etiquetaDTO

        // Blog <-> blogDTO
        // blog -> blogcard

        #region Rol
        public async Task<RolDTO> RolToRolDTO(int id)
        {
            var rol = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.IdRol == id);
            if (rol == null)
            {
                return null; // Tambien se puede lanzar una execepcion
            }
            return new RolDTO
            {
                IdRol = rol.IdRol,
                NombreRol = rol.NombreRol
            };                
        }

        #endregion


        #region USUARIO
        public  UsuarioDTO MapUsuarioToUsuarioDTO(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.IdPersona,
                NombreUsuario = usuario.NombreUsuario,
                Clave = usuario.Clave, 
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                UsuarioRoles = usuario.UsuarioRoles.Select(ur => (RolesEnum)ur.IdRol).ToList()
            };
        }

        public  Usuario MapUsuarioDtoToUsuario(UsuarioDTO usuarioDTO)
        {
            return new Usuario
            {
                IdPersona = usuarioDTO.Id,
                NombreUsuario = usuarioDTO.NombreUsuario,
                Clave = usuarioDTO.Clave,  // Recuerda encriptar la clave en producción
                Nombre = usuarioDTO.Nombre,
                Apellido = usuarioDTO.Apellido,
                Email = usuarioDTO.Email,
                Dni = usuarioDTO.Dni,

                UsuarioRoles = usuarioDTO.UsuarioRoles.Select(rolEnum => new UsuarioRol
                {
                    IdRol = (int)rolEnum  // Convierte el enum en el ID del rol correspondiente
                }).ToList()
            };
        }

        public async Task<SesionDTO> MapUsuarioToSesiondto(Usuario usuario)
        {
            var roles = await _appDbContext.UsuarioRoles
                .Where(ru => ru.IdUsuario == usuario.IdPersona)
                .Select(ru => ru.Rol.NombreRol)
                .ToListAsync();

            return new SesionDTO
            {
                IdPersona = usuario.IdPersona,
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                UsuarioRoles =  roles,
            };
        }

        public async Task<Usuario> MapSesionDtoToUsuario(SesionDTO sesionDTO)
        {
           
            Usuario usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == sesionDTO.NombreUsuario);

            if (usuario == null) throw new Exception("El usuario es nulo");

            return new Usuario
            {
                IdPersona = usuario.IdPersona,
                NombreUsuario = usuario.NombreUsuario,
                Clave = usuario.Clave,  // Recuerda encriptar la clave en producción
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                UsuarioRoles = usuario.UsuarioRoles,
            };
        }

        public async Task<SesionDTO?> MapLoginDtoToSesionDto(LoginDTO loginDTO)
        {
            // Buscar el usuario en la base de datos con su nombre de usuario
            var usuario = await _appDbContext.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == loginDTO.NombreDeUsuario);

            if (usuario == null)
            {
                return null; // Usuario no encontrado
            }

            var roles = await _appDbContext.UsuarioRoles
                .Where(ru => ru.IdUsuario == usuario.IdPersona)
                .Select(ru => ru.Rol.NombreRol)
                .ToListAsync();

            return new SesionDTO
            {
                IdPersona = usuario.IdPersona,
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                UsuarioRoles =roles,
            };
        }

        #endregion


        #region ETIQUETA
        public EtiquetaDTO MapEtiquetaToEtiquetaDTO(Etiqueta etiqueta)
        {
            if (etiqueta == null)
            {
                throw new ArgumentNullException(nameof(etiqueta), "La etiqueta no puede ser nula.");
            }
            return new EtiquetaDTO
            {
                IdEtiqueta = etiqueta.IdEtiqueta,
                Nombre = etiqueta.Nombre,
            };
        }

        public async Task<Etiqueta> MapEtiquetaDTOToEtiqueta(EtiquetaDTO etiquetaDTO)
        {
            if (etiquetaDTO == null)
            {
                throw new ArgumentNullException(nameof(etiquetaDTO), "El DTO de etiqueta no puede ser nulo.");
            }

            var etiquetaExiste = await _appDbContext.etiquetas.FirstOrDefaultAsync(e => e.Nombre == etiquetaDTO.Nombre);

            if(etiquetaExiste != null)
            {
                return new Etiqueta
                {
                    IdEtiqueta = etiquetaDTO.IdEtiqueta,
                    Nombre = etiquetaDTO.Nombre,
                    BlogEtiquetas = etiquetaExiste.BlogEtiquetas
                };
            }
            else
            {
                return new Etiqueta
                {
                    IdEtiqueta = etiquetaDTO.IdEtiqueta,
                    Nombre = etiquetaDTO.Nombre,
                    BlogEtiquetas = new List<Entities.BlogEtiqueta>() // Lista vacía para evitar null
                };
            }
        }
        #endregion

        #region Blog

        public async Task<BlogDTO> MapBlogToBlogDTO(Entities.Blog blog)
        {
            var etiquetas = await _appDbContext.blogEtiquetas
                 .Where(be => be.IdBlog == blog.IdBlog)
                .Include(be => be.Etiqueta)
                .Select(be => be.Etiqueta.Nombre)
                .ToListAsync();
            return new BlogDTO
            {
                 IdBlog = blog.IdBlog,
                 Titulo = blog.Titulo,
                 Contenido = blog.Contenido,
                 FechaDePublicacion = blog.FechaDePublicacion,
                  Descripcion = blog.Descripcion,
                  Enlace = blog.Enlace,
                  Etiquetas  = etiquetas,
            };
        }

        public async Task<Entities.Blog> MapBlogDTOToBlog(BlogDTO blogDTO)
        {
            if (blogDTO == null)
                throw new ArgumentNullException(nameof(blogDTO));

            var blog = new Entities.Blog
            {
                IdBlog = blogDTO.IdBlog,
                Titulo = blogDTO.Titulo,
                Contenido = blogDTO.Contenido,
                FechaDePublicacion = blogDTO.FechaDePublicacion,
                Descripcion = blogDTO.Descripcion,
                Enlace = blogDTO.Enlace,
                BlogEtiquetas = new List<Entities.BlogEtiqueta>()
            };

            foreach (var nombreEtiqueta in blogDTO.Etiquetas)
            {
                var etiqueta = await _appDbContext.etiquetas
                    .FirstOrDefaultAsync(e => e.Nombre.ToLower() == nombreEtiqueta.ToLower());

                if (etiqueta != null)
                {
                    blog.BlogEtiquetas.Add(new Entities.BlogEtiqueta
                    {
                        IdEtiqueta = etiqueta.IdEtiqueta,
                        Etiqueta = etiqueta
                    });
                }
                else
                {
                    // Opcional: crear etiqueta si no existe
                    var nuevaEtiqueta = new Etiqueta { Nombre = nombreEtiqueta };
                    _appDbContext.etiquetas.Add(nuevaEtiqueta);
                    await _appDbContext.SaveChangesAsync();

                    blog.BlogEtiquetas.Add(new Entities.BlogEtiqueta
                    {
                        IdEtiqueta = nuevaEtiqueta.IdEtiqueta,
                        Etiqueta = nuevaEtiqueta
                    });
                }
            }

            return blog;
        }


        public async Task<CardBlog> MapBlogToCardBlog(Entities.Blog blog)
        {
            var etiquetas = await _appDbContext.blogEtiquetas
                .Where(be => be.IdBlog == blog.IdBlog)
                .Include(be => be.Etiqueta)
                .Select(be => be.Etiqueta.Nombre)
                .ToListAsync();
            return new CardBlog
            {
                 IdBlog = blog.IdBlog,
                 Titulo = blog.Titulo,
                 FechaDePublicacion= blog.FechaDePublicacion,
                 Descripcion = blog.Descripcion,
                 Enlace  = blog.Enlace,
                BlogEtiquetas = etiquetas,
            };
        }


       

        #endregion
    }
}
