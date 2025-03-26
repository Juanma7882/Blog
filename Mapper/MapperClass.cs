using MiBlog.AppDbContext;
using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Enums;
using MiBlog.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

       
    }
}
