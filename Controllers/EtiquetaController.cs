using MiBlog.DTOs;
using MiBlog.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetaController : ControllerBase
    {
        private readonly EtiquetaService _etiquetaService;

        public EtiquetaController(EtiquetaService service)
        {
            _etiquetaService = service;
        }

        [HttpPost]
        [Route("CrearEtiqueta")]
        public async Task<ActionResult<EtiquetaDTO>> CrearEtiqueta([FromBody] EtiquetaDTO etiquetaDTO)
        {
            if (etiquetaDTO == null)
            {
                return BadRequest("El usuario no puede estar vacío.");
            }
            try
            {
                var etiquetaCreada = await _etiquetaService.CrearEtiqueta(etiquetaDTO);
                return Ok(etiquetaCreada);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        [Route("BuscarEtiqueta")]
        public async Task<ActionResult<EtiquetaDTO>> BuscarEtiqueta(string NombreEtiqueta)
        {
            if (NombreEtiqueta == null)
            {
                return BadRequest("la etiqueta no puede estar vacia.");
            }
            try
            {
                var etiquetaCreada = await _etiquetaService.BuscarEtiquetas(NombreEtiqueta);
                return Ok(etiquetaCreada);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        [Route("ListarEtiqueta")]
        public async Task<ActionResult<EtiquetaDTO>> ListarEtiqueta()
        {
            try
            {
                var listaDeEtiquetasDTO = await _etiquetaService.ListarEtiquetas();
                return Ok(listaDeEtiquetasDTO);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        [Route("EliminarEtiqueta")]
        public async Task<ActionResult<bool>> EliminarEtiqueta(string nombre)
        {
            try
            {
                var etiquetaEliminada = await _etiquetaService.EliminarEtiqueta(nombre);
                return Ok(etiquetaEliminada);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
