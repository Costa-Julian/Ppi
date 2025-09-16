using Application;
using Application.Dto;
using Application.Interfaces;
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

        [HttpGet("{id:int}", Name = "GetOrdenById")]
        public IActionResult GetOrdenById(int id) => Ok(new { id });

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> Orden([FromBody] OrdenRequest request, CancellationToken ct) 
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try 
            {
                var id = await _ordenService.CreateOrderAsynck(request, ct);
                return CreatedAtRoute("GetOrdenById", new { id }, new IdResponse(id));
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
    } 
}
