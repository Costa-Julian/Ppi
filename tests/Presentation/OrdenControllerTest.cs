using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;
namespace Presentation
{
        [TestFixture]
        public class OrdenControllerTest
        {
            private Mock<IOrdenService> _ordenService = null!;
            private OrdenController _sut = null!;
            private CancellationToken _ct;

            public OrdenControllerTest() { }
            

            [SetUp]
            public void Setup()
            {
                _ordenService = new Mock<IOrdenService>(MockBehavior.Strict);
                _sut = new OrdenController(_ordenService.Object);
                _ct = CancellationToken.None;
            }

            [Test]
            public async Task GetById_Existente_Retorna200OkConDto()
            {
                var dto = new OrdenResponseDto(
                    cuentaId: 1, nombreActivo: "GGAL", operacion: 'C',
                    cantidad: 5, precio: 100m, montoTotal: 500m, estado: "Ejecutado");

                _ordenService.Setup(s => s.GetById(10, _ct)).ReturnsAsync(dto);

                var action = await _sut.GetById(10, _ct);

                var ok = action.Result as OkObjectResult;
                ok.Should().NotBeNull();
                ok!.StatusCode.Should().Be(StatusCodes.Status200OK);
                ok.Value.Should().BeEquivalentTo(dto);

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task GetById_Inexistente_Retorna404()
            {
                _ordenService.Setup(s => s.GetById(999, _ct)).ReturnsAsync((OrdenResponseDto?)null);

                var action = await _sut.GetById(999, _ct);

                action.Result.Should().BeOfType<NotFoundResult>();
                _ordenService.VerifyAll();
            }

            [Test]
            public async Task InsertNewOrden_ModelStateInvalido_Retorna422ValidationProblem()
            {
                _sut.ModelState.AddModelError("req", "invalid");

                var req = new OrdenRequest(cuentaId: 1, nombreActivo: "ALUA", operacion: 'C', cantidad: 10, precio: 12.34m);

                var result = await _sut.InsertNewOrden(req, _ct);

                var obj = result as ObjectResult;
                obj.Should().NotBeNull();
                obj!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
                obj.Value.Should().BeOfType<ValidationProblemDetails>();

                _ordenService.VerifyNoOtherCalls();
            }

            [Test]
            public async Task InsertNewOrden_Exitoso_Retorna201CreatedAtRouteConId()
            {
                var req = new OrdenRequest(1, "AL30", 'C', 2, 200m);
                _ordenService.Setup(s => s.CreateOrderAsync(req, _ct)).ReturnsAsync(123);

                var result = await _sut.InsertNewOrden(req, _ct);

                var created = result as CreatedAtRouteResult;
                created.Should().NotBeNull();
                created!.StatusCode.Should().Be(StatusCodes.Status201Created);
                created.RouteName.Should().Be("Orden");
                created.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(123);
                created.Value.Should().BeEquivalentTo(new OrdenController.IdResponse(123));

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task InsertNewOrden_ActivoNoEncontrado_Retorna404Problem()
            {
                var req = new OrdenRequest(1, "NOEXISTE", 'C', 1, 10m);
                _ordenService
                    .Setup(s => s.CreateOrderAsync(req, _ct))
                    .ThrowsAsync(new KeyNotFoundException("Ticker NOEXISTE"));

                var result = await _sut.InsertNewOrden(req, _ct);
                ExtractStatusCode(result).Should().Be(StatusCodes.Status404NotFound);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("Activo no encontrado");

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task InsertNewOrden_ReglaNegocioInvalida_CantidadCero_Retorna422Problem()
            {
                var req = new OrdenRequest(1, "GGAL", 'C', 0, null);
                _ordenService
                    .Setup(s => s.CreateOrderAsync(req, _ct))
                    .ThrowsAsync(new ArgumentException("Cantidad debe ser > 0"));

                var result = await _sut.InsertNewOrden(req, _ct);

                ExtractStatusCode(result).Should().Be(StatusCodes.Status422UnprocessableEntity);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("Reglas de negocio invalidas");

                _ordenService.VerifyAll();
            }
        [Test]
        public async Task InsertNewOrden_ReglaNegocioInvalida_NombreActivo_Retorna422Problem()
        {
            var req = new OrdenRequest(1, "GASKFKLASKDPOKASDPOEMPOMASKMDPOKREOSLKASDLKÑCMLASMDREPOGAL", 'C', 10, null);
            _ordenService
                .Setup(s => s.CreateOrderAsync(req, _ct))
                .ThrowsAsync(new ArgumentException("El nombre del activo no puede superar los 35 caracteres."));

            var result = await _sut.InsertNewOrden(req, _ct);

            ExtractStatusCode(result).Should().Be(StatusCodes.Status422UnprocessableEntity);

            var pd = ExtractProblemDetails(result);
            pd.Should().NotBeNull();
            pd!.Title.Should().Be("Reglas de negocio invalidas");

            _ordenService.VerifyAll();
        }

        [Test]
            public async Task GetAll_SinElementos_Retorna204()
            {
                _ordenService.Setup(s => s.GetAll(_ct)).ReturnsAsync(new List<OrdenResponseDto>());

                var result = await _sut.GetAll(_ct);

                result.Should().BeOfType<NoContentResult>();
                _ordenService.VerifyAll();
            }

            [Test]
            public async Task GetAll_ConElementos_Retorna200Ok()
            {
                var list = new List<OrdenResponseDto>
            {
                new( cuentaId: 1, nombreActivo: "ALUA", operacion: 'C', cantidad: 10, precio: 1.23m, montoTotal: 12.3m, estado: "Ejecutado")
            };
                _ordenService.Setup(s => s.GetAll(_ct)).ReturnsAsync(list);

                var result = await _sut.GetAll(_ct);

                var ok = result as OkObjectResult;
                ok.Should().NotBeNull();
                ok!.StatusCode.Should().Be(StatusCodes.Status200OK);
                ok.Value.Should().BeEquivalentTo(list);

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task SetEstado_Exitoso_Retorna204()
            {
                var body = new SetEstadoRequest ( estadoId: 2 );
                _ordenService.Setup(s => s.UpdateOrdenEstado(7, 2, _ct)).Returns(Task.CompletedTask);

                var result = await _sut.SetEstado(7, body, _ct);

                result.Should().BeOfType<NoContentResult>();
                _ordenService.VerifyAll();
            }

            [Test]
            public async Task SetEstado_OrdenNoEncontrada_Retorna404Problem()
            {
                var body = new SetEstadoRequest (estadoId: 1);
                _ordenService
                    .Setup(s => s.UpdateOrdenEstado(99, 1, _ct))
                    .ThrowsAsync(new KeyNotFoundException("Orden 99"));

                var result = await _sut.SetEstado(99, body, _ct);
                ExtractStatusCode(result).Should().Be(StatusCodes.Status404NotFound);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("Orden no encontrada");

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task SetEstado_DatosInvalidos_Retorna422Problem()
            {
                var body = new SetEstadoRequest ( estadoId : -1 );
                _ordenService
                    .Setup(s => s.UpdateOrdenEstado(1, -1, _ct))
                    .ThrowsAsync(new ArgumentException("estadoId invalido"));

                var result = await _sut.SetEstado(1, body, _ct);

                ExtractStatusCode(result).Should().Be(StatusCodes.Status422UnprocessableEntity);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("Datos inválidos");

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task SetEstado_TransicionNoPermitida_Retorna409Problem()
            {
                var body = new SetEstadoRequest (estadoId: 3);
                _ordenService
                    .Setup(s => s.UpdateOrdenEstado(2, 3, _ct))
                    .ThrowsAsync(new InvalidOperationException("No permitido"));

                var result = await _sut.SetEstado(2, body, _ct);

                ExtractStatusCode(result).Should().Be(StatusCodes.Status409Conflict);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("Conflicto de estado");

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task Delete_Exitoso_Retorna204()
            {
                _ordenService.Setup(s => s.DeleteOrden(5, _ct)).Returns(Task.CompletedTask);

                var result = await _sut.Delete(5, _ct);

                result.Should().BeOfType<NoContentResult>();
                _ordenService.VerifyAll();
            }

            [Test]
            public async Task Delete_NoEncontrada_Retorna404Problem()
            {
                _ordenService
                    .Setup(s => s.DeleteOrden(77, _ct))
                    .ThrowsAsync(new KeyNotFoundException("77"));

                var result = await _sut.Delete(77, _ct);

                ExtractStatusCode(result).Should().Be(StatusCodes.Status404NotFound);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("Orden no encontrada");

                _ordenService.VerifyAll();
            }

            [Test]
            public async Task Delete_Conflicto_Retorna409Problem()
            {
                _ordenService
                    .Setup(s => s.DeleteOrden(11, _ct))
                    .ThrowsAsync(new InvalidOperationException("No se puede eliminar"));

                var result = await _sut.Delete(11, _ct);

                ExtractStatusCode(result).Should().Be(StatusCodes.Status409Conflict);

                var pd = ExtractProblemDetails(result);
                pd.Should().NotBeNull();
                pd!.Title.Should().Be("No se puede eliminar la orden");

             _ordenService.VerifyAll();
            }

            private static ProblemDetails? ExtractProblemDetails(IActionResult result)
            {
                
                if (result is ObjectResult or1)
                {
                    if (or1.Value is ProblemDetails pd) return pd;       
                    if (or1.Value is ObjectResult or2 && or2.Value is ProblemDetails pd2) return pd2; 
                }
                return null;
            }

            private static int? ExtractStatusCode(IActionResult result)
            {
                return (result as ObjectResult)?.StatusCode;
            }

        }

    }

