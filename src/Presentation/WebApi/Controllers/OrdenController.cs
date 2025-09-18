using Application;
using Application.Dto;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Operaciones sobre Ordenes de Inversion.")]
    public class OrdenController : ControllerBase
    {
    
        private readonly IOrdenService _ordenService;
        public record IdResponse(int Id);

        public OrdenController(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        /// <summary>
        /// orden/{id} Devuelve orden por id
        /// </summary>
        [HttpGet("{id:int}", Name = "Orden")]
        [ProducesResponseType(typeof(DtoOrdenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary ="Listar ordener por id",OperationId ="OrdenById")]
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
        /// <remarks>Bono/FCI requieren <c>precio</c></remarks>
        /// <returns></returns>
        [HttpPost("~/Orden")]
        [ProducesResponseType(typeof(IdResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [SwaggerOperation(Summary = "Genera una nueva orden en el sistema.", OperationId = "InsertNewOrden")]
        [SwaggerRequestExample(typeof(OrdenRequest), typeof(OrdenRequestExample))]
        public async Task<ActionResult> InsertNewOrden([FromBody] OrdenRequest request, CancellationToken ct) 
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try 
            {
                var id = await _ordenService.CreateOrderAsync(request, ct);
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
        [HttpGet ("~/Orden")] 
        [ProducesResponseType(typeof(List<DtoOrdenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(Summary = "Obtener totas las ordenes.", OperationId = "GetAllOrden")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await _ordenService.GetAll(ct);

            if (items is null || items.Count == 0)
                return NoContent();

            return Ok(items);
        }
        /// <summary>
        /// Generio el update de estado, en una orden especifica.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <param name="ct"></param>
        /// <remarks>Idempotente: si la orden ya esta en el estado a modificar, devuelve 204.</remarks>
        /// <returns></returns>
        [HttpPut("~/Orden/{id:int}/estado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [SwaggerOperation(Summary = "Actualiza el estado de una orden existente.", OperationId = "SetEstado")]
        public async Task<IActionResult> SetEstado(int id, [FromBody] SetEstadoRequest body, CancellationToken ct)
        {
            try 
            {
                await _ordenService.UpdateOrdenEstado(id, body.estadoId, ct);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Problem(title: "Orden no encontrada", detail: ex.Message));
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(Problem(title: "Datos inválidos", detail: ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(Problem(title: "Conflicto de estado", detail: ex.Message));
            }
        }

        /// <summary>Elimina una orden por Id.</summary>
        [HttpDelete("{id:int}")] //orden/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Eliminar orden", OperationId = "DeleteOrden")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                await _ordenService.DeleteOrden(id, ct);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Problem(title: "Orden no encontrada", detail: ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(Problem(title: "No se puede eliminar la orden", detail: ex.Message));
            }
        }
    } 
}
