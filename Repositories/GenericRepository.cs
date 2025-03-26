using MiBlog.AppDbContext;
using MiBlog.Repositories.Contrato;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

//using MiBlog.Repositories.Contrato.IGenericRepository;
namespace MiBlog.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
      
        private readonly AppDbBlogContext _appDbContext;

        public GenericRepository(AppDbBlogContext appDbBlogContext)
        {
            _appDbContext = appDbBlogContext;
        }

        public async Task<TModel> Crear(TModel model)
        {
            try 
            {
                _appDbContext.Add(model);
                await _appDbContext.SaveChangesAsync();
                return model;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al crear el modelo",ex);
            }
        }


        public async Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro)
        {
            try
            {
                TModel modelo = await _appDbContext.Set<TModel>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el modelo", ex);
            }
        }


        public async Task<bool> Editar(TModel modelo)
        {
            try
            {
                _appDbContext.Set<TModel>().Update(modelo);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Editar el modelo", ex);
            }
        }

        public async Task<bool> Eliminar(TModel modelo)
        {
            try
            {
                _appDbContext.Set<TModel>().Remove(modelo);
                await _appDbContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception($"Error al elimninar ", ex);
            }
        }

        public async Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModel> queryModelo = filtro == null
                ? _appDbContext.Set<TModel>()// si no hay filto vamos a ejecutar esto
                : _appDbContext.Set<TModel>().Where(filtro);// si hay filto vamos a ejecutar esto

                return queryModelo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar ", ex);
            }
        }
      
    }
}
