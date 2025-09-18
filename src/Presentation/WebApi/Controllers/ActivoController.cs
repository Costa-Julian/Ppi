using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SwaggerTag("Consulta de activos")]
    public class ActivoController : ControllerBase
    {
        private readonly IActivoService _service;

        public ActivoController(IActivoService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar activos", OperationId = "ListActivos")]
        [ProducesResponseType(typeof(List<Activo>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<List<Activo>> Activos(CancellationToken ct) => _service.GetAll(ct);

    }
}
