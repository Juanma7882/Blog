using MiBlog.DTOs;
using MiBlog.Entities;
using MiBlog.Mapper;
using MiBlog.Repositories.Contrato;
using Microsoft.EntityFrameworkCore;

namespace MiBlog.Servicios
{
    public class EtiquetaService
    {
        private readonly IGenericRepository<Etiqueta> _genericRepository;
        private readonly MapperClass _mapperClass;
      

        public EtiquetaService(IGenericRepository<Etiqueta> genericRepository, MapperClass mapperClass)
        {
            _genericRepository = genericRepository;
            _mapperClass = mapperClass;
        }

        public async Task<EtiquetaDTO> CrearEtiqueta(EtiquetaDTO etiquetaDTO)
        {
            try
            {
                if (etiquetaDTO == null)
                {
                    throw new ArgumentNullException(nameof(etiquetaDTO), "La etiqueta no puede ser nula.");
                }

                var etiquetaExiste = await _genericRepository.Obtener(e => e.Nombre == etiquetaDTO.Nombre);

                if (etiquetaExiste != null)
                {
                    throw new InvalidOperationException("El nombre de la etiqueta ya existe.");
                }

                var nuevaEtiqueta = await _mapperClass.MapEtiquetaDTOToEtiqueta(etiquetaDTO);

                Etiqueta etiquetaCreada = await _genericRepository.Crear(nuevaEtiqueta);

                return _mapperClass.MapEtiquetaToEtiquetaDTO(etiquetaCreada);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear una etiqueta en la base de datos", ex);
            }
        }

        public async Task<List<EtiquetaDTO>> BuscarEtiquetas(string nombre)
        {
            try
            {
                var query = await _genericRepository.Consultar(e => e.Nombre.Contains(nombre)); // Espera la consulta
                var listaDeEtiquetas = await query.ToListAsync(); // Convierte la consulta en lista

                return listaDeEtiquetas
                           .Select(e => _mapperClass.MapEtiquetaToEtiquetaDTO(e)) // Mapea cada Etiqueta a DTO
                           .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar las etiquetas en la base de datos", ex);
            }
        }

        public async Task<List<EtiquetaDTO>> ListarEtiquetas()
        {
            try
            {
                var queryEtiquetas = await _genericRepository.Consultar(); // Trae todas las etiquetas
                var listaDeEtiquetas = await queryEtiquetas.ToListAsync(); // Convierte a lista

                return listaDeEtiquetas
                    .Select(e => _mapperClass.MapEtiquetaToEtiquetaDTO(e)) // Mapea cada Etiqueta a DTO
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Listar las etiquetas en la base de datos", ex);
            }
        }

        public async Task<bool> EliminarEtiqueta(string nombre)
        {
            try
            {
                var Etiqueta = await _genericRepository.Obtener(u => u.Nombre == nombre);

                if (Etiqueta == null)
                {
                    throw new TaskCanceledException("El Etiqueta no existe");
                }

                bool EtiquetaEliminada = await _genericRepository.Eliminar(Etiqueta);

                if (!EtiquetaEliminada)
                    throw new TaskCanceledException("No se pudo eliminar");

                return EtiquetaEliminada;
            }
             catch (Exception ex)
            {
                throw new Exception("Error al Eliminar Usuario un usuario", ex);
            }
        }


    }
}
