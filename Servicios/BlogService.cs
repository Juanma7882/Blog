using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Mapper;
using MiBlog.Repositories.Contrato;
using Microsoft.EntityFrameworkCore;

namespace MiBlog.Servicios
{
    public class BlogService
    {
        private readonly IGenericRepository<Blog> _genericRepository;
        private readonly MapperClass _mapperClass;


        public BlogService(IGenericRepository<Blog> genericRepository,MapperClass mapperClass)
        {
            _genericRepository = genericRepository;
            _mapperClass = mapperClass;
        }


        public async Task<BlogDTO> CrearBlog(BlogDTO blogDTO)
        {
            try
            {
                if (blogDTO == null)
                {
                    throw new ArgumentNullException(nameof(blogDTO), "El Blog no puede estar vacío");
                }
                var blogExistente = await _genericRepository.Consultar(b => b.Titulo == blogDTO.Titulo);

                if (blogExistente.FirstOrDefault() != null)
                {
                    throw new InvalidOperationException("El Titulo del Blog ya existe");
                }

                var blog = await _mapperClass.MapBlogDTOToBlog(blogDTO);

                var nuevoBlog = await _genericRepository.Crear(blog);

                return await _mapperClass.MapBlogToBlogDTO(nuevoBlog);


            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear un blog", ex);
            }
        }


        public async Task<List<CardBlog>> ListarCardBlog()
        {
            try
            {
                var queryBlog = await _genericRepository.Consultar();
                var listaDeBlog = await queryBlog.ToListAsync();


                List<CardBlog> usuarioDTOs = new List<CardBlog>();

                foreach (var blog in listaDeBlog)
                {
                    CardBlog cardBlog = await _mapperClass.MapBlogToCardBlog(blog);
                    usuarioDTOs.Add(cardBlog);
                }

                return usuarioDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar un usuario", ex);
            }
        }

        public async Task<BlogDTO> ObtenerBlog(int id)
        {
            try
            {
                var blog = await _genericRepository.Obtener(b => b.IdBlog == id);
                if (blog == null)
                {
                    throw new InvalidOperationException("Blog no encontrado");
                }

                var blogDto = await _mapperClass.MapBlogToBlogDTO(blog);
                

                return blogDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar un usuario", ex);
            }
        }



        public async Task<bool> EditarBlog(BlogDTO blogDTO)
        {
            try
            {

                var blog = await _genericRepository.Obtener(b => b.Titulo == blogDTO.Titulo && b.IdBlog == blogDTO.IdBlog);
                if (blog == null)
                {
                    throw new InvalidOperationException("Blog no encontrado.");
                }

                
                var blogEditado = await _mapperClass.MapBlogDTOToBlog(blogDTO);
                return await _genericRepository.Editar(blogEditado);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el blog", ex);
            }
        }


        public async Task<bool> EliminarBlog(int id)
        {
            try
            {
                var blog = await _genericRepository.Obtener(b => b.IdBlog == id);

                if (blog == null)
                {
                    throw new TaskCanceledException("El Blog no existe");
                }

                bool blogEliminado = await _genericRepository.Eliminar(blog);

                if (!blogEliminado)
                    throw new TaskCanceledException("No se pudo eliminar");

                return blogEliminado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Eliminar el blog", ex);
            }
        }



    }
}
