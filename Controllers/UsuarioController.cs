using Microsoft.AspNetCore.Mvc;
using MiBlog.AppDbContext;
using Microsoft.EntityFrameworkCore;
using MiBlog.DTOs;
using MiBlog.Mapper;
using MiBlog.Servicios;
using Microsoft.AspNetCore.Authorization;
namespace MiBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbBlogContext _appDbContext;
        private readonly MapperClass _mapperClass;
        private readonly UsuarioService _usuarioService;


        public UsuarioController(AppDbBlogContext appDbContext, MapperClass mapperClass, UsuarioService usuarioService)
        {
            _appDbContext = appDbContext;
            _mapperClass = mapperClass;
            _usuarioService = usuarioService;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            try
            {
                string token = await _usuarioService.IniciarSesion(loginDTO);

                if (token == null)
                {
                    return StatusCode(500, "Error al generar el token.");
                }
                return Ok(new
                {
                    success = true,
                    message = "Login exitoso",
                    token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
      

        [HttpPost]
        [Authorize(Roles = "AdministrarUsuarios")]
        [Route("CrearUsuario")]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                return BadRequest("El usuario no puede estar vacío.");
            }
            try
            {
                var usuarioCreado =  await _usuarioService.CrearUsuario(usuarioDTO);
                return Ok(usuarioCreado);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // Código 409 si el usuario ya existe
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al guardar en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }

        }



        [HttpGet]
        [Authorize(Roles = "AdministrarBlog")]
        [Route("ListarUsuarios")]
        public async Task<ActionResult<List<SesionDTO>>> ListarUsuarios()
        {
            try
            {
                List<SesionDTO> sesions = await _usuarioService.ListarSesionDTO();

                return Ok(sesions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al listar usuarios: {ex.Message}");
            }
        }



        [HttpPut("ActualizarUsuario/")]
        public async Task<ActionResult<UsuarioDTO>> ActualizarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await _appDbContext.Usuarios.FindAsync(usuarioDTO.Id);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                bool actualizado = await _usuarioService.EditarUsuario(usuarioDTO);
                if (!actualizado)
                {
                    return BadRequest("No se pudo actualizar el usuario.");
                }

                return Ok(usuarioDTO);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ACTUALIZAR un usuario " + ex.Message);
            }
        }



        //[Auto(Roles = "SuperAdministrador")]
        [HttpGet("ObtenerUsuario/{id}")]
        [Authorize(Roles = "SuperAdministrador")]
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

                bool usuarioEliminado =await _usuarioService.EliminarUsuario(id);

                if (!usuarioEliminado)
                {
                    throw new Exception("Error al eliminar un usuario");
                }

                return Ok(new {messaje = "usuario eliminado correctamente"}); 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al elimanar un usuario", ex);
            }
        }

    }
}
