using System.Linq.Expressions;

namespace MiBlog.Repositories.Contrato
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        //create read update delete
        Task<TModel> Crear(TModel model);
        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro);
        Task<bool> Editar(TModel modelo);
        Task<bool> Eliminar(TModel modelo);
        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null);

    }
}
