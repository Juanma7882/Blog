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

namespace MiBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbBlogContext _appDbContext;
        private readonly IMapper _mapper;

        public UsuarioController(AppDbBlogContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        [HttpPost]
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
                Usuario nuevoUsuario = _mapper.Map<Usuario>(usuarioDTO);

                _appDbContext.Usuarios.Add(nuevoUsuario);
                await _appDbContext.SaveChangesAsync();

                // Mapear de Usuario a UsuarioDTO
                UsuarioDTO usuarioCreado = _mapper.Map<UsuarioDTO>(nuevoUsuario);

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


        [HttpGet]
        [Route("ListarUsuarios")]
        public async Task<ActionResult<UsuarioDTO>> ListarUsuarios()
        {
            try
            {
                List<Usuario> usuarios = await _appDbContext.Usuarios.ToListAsync();
                List<UsuarioDTO> usuariosDTO = _mapper.Map<List<UsuarioDTO>>(usuarios);
                return Ok(usuariosDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Listar un usuario " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDTO>> ActualizarUsuario(int id,UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await _appDbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Mapear datos sobre la entidad existente en lugar de reemplazarla
                _mapper.Map(usuarioDTO, usuario);

                _appDbContext.Entry(usuario).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();

                return Ok(usuarioDTO);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ACTUALIZAR un usuario " +ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = await _appDbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                SesionDTO UsuarioSesion = _mapper.Map<SesionDTO>(usuario);

                return Ok(UsuarioSesion);
            }
            catch(Exception ex)
            {
                throw new Exception("Error al obtener un usuario " +ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> EliminarUsuario(int id)
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

                return NoContent();
            }
            catch(Exception ex)
            {
                throw new Exception("Error al elimanar un usuario", ex);
            }
        }



    }
}
