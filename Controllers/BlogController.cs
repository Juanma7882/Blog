using MiBlog.AppDbContext;
using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Mapper;
using MiBlog.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {


        private readonly BlogService _BlogService;


        public BlogController(BlogService serviceBlogService)
        {
            _BlogService  = serviceBlogService ;
        }

        [HttpPost]
        [Route("CrearBlog")]
        public async Task<ActionResult<BlogDTO>> CrearBlogDTO([FromBody] BlogDTO blogDto)
        {
            if (blogDto == null)
            {
                return BadRequest("El Blog no puede estar vacío.");
            }
            try
            {
                var blogCreado = await _BlogService.CrearBlog(blogDto);
                return Ok(blogCreado);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpGet]
        [Route("ListarCardBlog")]
        public async Task<ActionResult<CardBlog>> ListarCardBlog()
        {
            try
            {
                var listaDeCardDTO = await _BlogService.ListarCardBlog();
                return Ok(listaDeCardDTO);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerBlog/{id}")]
        public async Task<ActionResult<CardBlog>> ObtenerBlog(int id)
        {
            try
            {
                var blog = await _BlogService.ObtenerBlog(id);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        [Route("EditarBlog")]
        public async Task<ActionResult<CardBlog>> EditarBlog(BlogDTO blogDTO)
        {
            try
            {
                var blog = await _BlogService.EditarBlog(blogDTO);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        [Route("EliminarBlog")]
        public async Task<ActionResult<bool>> EliminarBlog(int id)
        {
            try
            {
                var BlogEliminado = await _BlogService.EliminarBlog(id);
                return Ok(BlogEliminado);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
