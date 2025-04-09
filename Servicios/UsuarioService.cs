using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Mapper;
using MiBlog.Repositories;
using MiBlog.Repositories.Contrato;
using Microsoft.EntityFrameworkCore;

namespace MiBlog.Servicios
{
    public class UsuarioService
    {
        private readonly IGenericRepository<Usuario> _genericRepository;
        private readonly MapperClass _mapperClass;
        private readonly JWTService _jwtService;


        public UsuarioService(IGenericRepository<Usuario> genericRepository,MapperClass mapperClass, JWTService jwtService)
        {
            _genericRepository = genericRepository;
            _mapperClass = mapperClass;
            _jwtService = jwtService;
        }

        public async Task<SesionDTO> ValidarUsuario(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    throw new ArgumentNullException(nameof(loginDTO), "Los datos de inicio de sesión no pueden estar vacíos.");
                }

                var queryUsuario = await _genericRepository.Consultar(u =>
                    u.NombreUsuario == loginDTO.NombreDeUsuario &&
                    u.Clave == loginDTO.Clave
                    );

                if (queryUsuario.FirstOrDefault() == null) 
                {
                    throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
                }

                var sesionDTO = await _mapperClass.MapLoginDtoToSesionDto(loginDTO);

                return sesionDTO;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al validar el usuario",ex);
            }
        }

        public async Task<string> IniciarSesion(LoginDTO loginDTO)
        {
            try
            {
                SesionDTO sesionDTO  = await ValidarUsuario(loginDTO);

                Usuario usuario = await _mapperClass.MapSesionDtoToUsuario(sesionDTO);

                var token = await _jwtService.GenerarToken(usuario);
                
                return token;
            }
            catch (Exception ex)
            {
                throw new($"Error interno: {ex.Message}");
            }
        }



        public async Task<UsuarioDTO> CrearUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                if(usuarioDTO == null)
                {
                    throw new ArgumentNullException(nameof(usuarioDTO), "El usuario no puede estar vacío");
                }

                var usuarioExistente = await _genericRepository.Consultar(u =>
                u.NombreUsuario == usuarioDTO.NombreUsuario
                );

                if (usuarioExistente.FirstOrDefault() != null)
                {
                    throw new InvalidOperationException("El nombre de usuario elegido no se encuatra disponible");
                }

                Usuario usuario = new Usuario {
                    NombreUsuario = usuarioDTO.NombreUsuario,
                    Clave = usuarioDTO.Clave,
                    Nombre = usuarioDTO.Nombre,
                    Apellido = usuarioDTO.Apellido,
                    Email = usuarioDTO.Email,
                    Dni = usuarioDTO.Dni,
                    UsuarioRoles = usuarioDTO.UsuarioRoles.Select(rolEnum => new UsuarioRol
                    {
                        IdRol = (int)rolEnum  // Convierte el enum en el ID del rol correspondiente
                    }).ToList(),
                };

                Usuario usuarioCreado = await _genericRepository.Crear(usuario);

                return _mapperClass.MapUsuarioToUsuarioDTO(usuarioCreado);
            }
            catch (Exception ex)
            {
                // Aquí capturamos errores de base de datos o de otro tipo
                throw new Exception("Error al crear un usuario en la base de datos", ex);
            }
        }

      

        public async Task<List<SesionDTO>> ListarSesionDTO()
        {
            try
            {
                var queryUsuario = await _genericRepository.Consultar();
                var listaUsuario = queryUsuario.Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol).ToList();

                List<SesionDTO> usuarioDTOs = new List<SesionDTO>();

                foreach (var usuarios in listaUsuario)
                {
                    SesionDTO us = await _mapperClass.MapUsuarioToSesiondto(usuarios);
                    usuarioDTOs.Add(us);
                }

                return usuarioDTOs;

            }
            catch (Exception ex) 
            {
                throw new Exception("Error al listar un usuario", ex);
            }
        }

        public async Task<bool> EditarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO == null)
                {
                    throw new ArgumentNullException(nameof(usuarioDTO), "El usuario no puede estar vacío");
                }

                var usuarioEncontrado = await _genericRepository.Obtener(u => u.IdPersona == usuarioDTO.Id);
                if (usuarioEncontrado == null)
                {
                    throw new InvalidOperationException("Usuario no encontrado.");
                }

                // Actualizar los valores
                usuarioEncontrado.NombreUsuario = usuarioDTO.NombreUsuario;
                usuarioEncontrado.Clave = usuarioDTO.Clave; // Encriptar la clave si es necesario
                usuarioEncontrado.Nombre = usuarioDTO.Nombre;
                usuarioEncontrado.Apellido = usuarioDTO.Apellido;
                usuarioEncontrado.Email = usuarioDTO.Email;
                usuarioEncontrado.Dni = usuarioDTO.Dni;

                usuarioEncontrado.UsuarioRoles = usuarioDTO.UsuarioRoles.Select(rolEnum => new UsuarioRol
                {
                    IdRol = (int)rolEnum  // Convierte el enum en el ID del rol correspondiente
                }).ToList();

                return await _genericRepository.Editar(usuarioEncontrado);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar un usuario", ex);
            }
        }


        public async Task<bool> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _genericRepository.Obtener(u => u.IdPersona == id);

                if (usuario == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                bool usuarioEliminado =await _genericRepository.Eliminar(usuario);

                if (!usuarioEliminado)
                    throw new TaskCanceledException("No se pudo eliminar");

                return usuarioEliminado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Eliminar Usuario un usuario", ex);
            }
        }



    }
}
