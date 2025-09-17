using Application;
using Application.Dto;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdenController : ControllerBase
    {
    
        private readonly IOrdenService _ordenService;
        public record IdResponse(int Id);

        public OrdenController(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        /// <summary>
        /// GET /api/orden/{id} → Devuelve orden por id
        /// </summary>
        [HttpGet("{id:int}", Name = "Orden")]
        [ProducesResponseType(typeof(DtoOrdenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DtoOrdenResponse>> GetById(int id, CancellationToken ct)
        {
            var dto = await _ordenService.GetById(id, ct);
            return dto is null ? NotFound() : Ok(dto);
        }
        /// <summary>
        ///     Crea una nueva orden
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("~/Orden")]
        [ProducesResponseType(typeof(IdResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> InsertNewOrden([FromBody] OrdenRequest request, CancellationToken ct) 
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try 
            {
                var id = await _ordenService.CreateOrderAsynck(request, ct);
                return CreatedAtRoute("Orden", new { id }, new IdResponse(id));
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(Problem(title: "Activo no encontrado", detail: ex.Message));
            }
            catch(ArgumentException ex)
            {
                return UnprocessableEntity(Problem(title: "Reglas de negocio invalidas", detail: ex.Message));
            }
        }
        /// <summary>Devuelve todas las órdenes.</summary>
        [HttpGet ("~/ Orden")] 
        [ProducesResponseType(typeof(List<DtoOrdenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await _ordenService.GetAll(ct);

            if (items is null || items.Count == 0)
                return NoContent();

            return Ok(items);
        }

        [HttpPut("api/orden/{id:int}/estado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SetEstado(int id, [FromBody] SetEstadoRequest body, CancellationToken ct)
        {
            await _ordenService.UpdateOrdenEstado(id, body.estadoId, ct);
            return NoContent(); 
        }
    } 
}
