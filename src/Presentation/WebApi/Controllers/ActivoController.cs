using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivoController : ControllerBase
    {
        private readonly IActivoService _service;

        public ActivoController(IActivoService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<Activo>> Activos(CancellationToken ct) => _service.GetAll(ct);

    }
}
