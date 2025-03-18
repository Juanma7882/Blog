using MiBlog.AppDbContext;
using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Enums;
using MiBlog.Migrations;
using Microsoft.EntityFrameworkCore;
using System;

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
        // usuario <-> usuarioDTO 
        // usuario -> sesionDTO
        // LoginDTO -> sesionDTO

        #region USUARIO
        public static UsuarioDTO MapUsuarioToUsuarioDTO(Usuario usuario)
        {
            return new UsuarioDTO
            {
                NombreUsuario = usuario.NombreUsuario,
                Clave = usuario.Clave, 
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                UsuarioRoles = usuario.UsuarioRoles.Select(ur => (RolesEnum)ur.IdRol).ToList()
            };
        }

        public static Usuario MapUsuarioDtoToUsuario(UsuarioDTO usuarioDTO)
        {
            return new Usuario
            {
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

        public static SesionDTO MapUsuarioToSesiondto(Usuario usuario)
        {
            return new SesionDTO
            {
                IdPersona = usuario.IdPersona,
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                //UsuarioRoles = usuario.UsuarioRoles.Select(ur => (RolesEnum)ur.IdRol).ToList()
                UsuarioRoles = usuario.UsuarioRoles
                    .Where(ur => Enum.IsDefined(typeof(RolesEnum), ur.IdRol))
                    .Select(ur => (RolesEnum)ur.IdRol)
                    .ToList()
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

            return new SesionDTO
            {
                IdPersona = usuario.IdPersona,
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Dni = usuario.Dni,
                UsuarioRoles = usuario.UsuarioRoles.Select(ur => (RolesEnum)ur.IdRol).ToList()
            };
        }

        #endregion


    }
}
