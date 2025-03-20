using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiBlog.AppDbContext;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using System.Security.Claims;
using MiBlog.Entities;
using Microsoft.EntityFrameworkCore;
using MiBlog.DTOs;
using MiBlog.Mapper;
using AutoMapper;
using MiBlog.Servicios;
using System.IdentityModel.Tokens.Jwt;
namespace MiBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbBlogContext _appDbContext;
        private readonly JWTService _jwtService;
        private readonly MapperClass _mapperClass;


        public UsuarioController(AppDbBlogContext appDbContext,JWTService jwtService, MapperClass mapperClass)
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
            _mapperClass = mapperClass;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    return BadRequest("Los datos de login no pueden estar vacíos.");
                }
                var usuario = await _jwtService.ValidarLogin(loginDTO);

                if (usuario == null)
                {
                    return BadRequest("Los datos de login no pueden estar vacíos.");
                }

                var token = await _jwtService.GenerarToken(usuario);

                if (token == null)
                {
                    return StatusCode(500, "Error al generar el token.");
                }
                return Ok(new
                {
                    success = true,
                    message = "Login exitoso",
                    token = token
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO == null)
                {
                    return BadRequest("Los datos del usuario no pueden estar vacíos.");
                }

                // Verificar si el nombre de usuario ya existe
                bool existeUsuario = await _appDbContext.Usuarios.AnyAsync(u => u.NombreUsuario == usuarioDTO.NombreUsuario);
                if (existeUsuario)
                {
                    return BadRequest("El nombre de usuario ya está en uso.");
                }

                // Mapear de UsuarioDTO a Usuario
                Usuario nuevoUsuario = MapperClass.MapUsuarioDtoToUsuario(usuarioDTO);

                _appDbContext.Usuarios.Add(nuevoUsuario);
                await _appDbContext.SaveChangesAsync();

                // Mapear de Usuario a UsuarioDTO
                UsuarioDTO usuarioCreado = MapperClass.MapUsuarioToUsuarioDTO(nuevoUsuario);

                return CreatedAtAction(nameof(CrearUsuario), new { id = nuevoUsuario.IdPersona }, usuarioCreado);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error al guardar el usuario en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [Authorize(Roles = "SuperAdministrador")]
        [HttpGet]
        [Route("ListarUsuarios")]
        public async Task<ActionResult<List<SesionDTO>>> ListarUsuarios()
        {
            try
            {
                List<Usuario> usuarios = await _appDbContext.Usuarios
             .Include(u => u.UsuarioRoles)
             .ThenInclude(ur => ur.Rol)
             .ToListAsync();
                List<SesionDTO> sesionDTOs = new List<SesionDTO>();

                foreach (var usuario in usuarios)
                {
                    var sesionDTO = await _mapperClass.MapUsuarioToSesiondto(usuario);
                    sesionDTOs.Add(sesionDTO);
                }
                return Ok(sesionDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al listar usuarios: {ex.Message}");
            }
        }

        [HttpPut("ActualizarUsuario/{id}")]
        public async Task<ActionResult<UsuarioDTO>> ActualizarUsuario(int id, UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await _appDbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                usuario.NombreUsuario = usuarioDTO.NombreUsuario;
                usuario.Clave = usuarioDTO.Clave; // Recuerda encriptar la clave si es necesario
                usuario.Nombre = usuarioDTO.Nombre;
                usuario.Apellido = usuarioDTO.Apellido;
                usuario.Email = usuarioDTO.Email;
                usuario.Dni = usuarioDTO.Dni;


                // Actualizar roles si es necesario
                usuario.UsuarioRoles.Clear();
                usuario.UsuarioRoles.AddRange(usuarioDTO.UsuarioRoles.Select(rol => new UsuarioRol { IdRol = (int)rol, IdUsuario = usuario.IdPersona }));

                await _appDbContext.SaveChangesAsync();

                return Ok(usuarioDTO);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ACTUALIZAR un usuario " + ex.Message);
            }
        }

        //[Authorize(Roles = "SuperAdministrador")]
        [HttpGet("ObtenerUsuario/{id}")]
        public async Task<ActionResult<SesionDTO>> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _appDbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                SesionDTO UsuarioSesion = await _mapperClass.MapUsuarioToSesiondto(usuario);

                return Ok(UsuarioSesion);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener un usuario " + ex.Message);
            }
        }

        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            try
            {

                var usuario = await _appDbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                _appDbContext.Usuarios.Remove(usuario);
                await _appDbContext.SaveChangesAsync();

                return Ok(new {messaje = "usuario eliminado correctamente"}); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al elimanar un usuario", ex);
            }
        }



    }
}
