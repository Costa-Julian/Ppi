using Application.Dto;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using Xunit;

namespace Presentation.test
{
    public class OrdenControllerTests
    {
        private readonly Mock<IOrdenService> _ordenService = new();
        private OrdenController CreateSut() => new(_ordenService.Object);

        // --------------------
        // GET /orden/{id}
        // --------------------
        [Fact]
        public async Task GetById_Existente_Retorna200OkConDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var dto = new DtoOrdenResponse(
                cuentaId: 1, nombreActivo: "GGAL", operacion: 'C',
                cantidad: 5, precio: 100m, montoTotal: 500m, estado: "Ejecutado");
            _ordenService.Setup(s => s.GetById(10, ct)).ReturnsAsync(dto);
            var sut = CreateSut();

            // Act
            var result = await sut.GetById(10, ct);

            // Assert
            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(StatusCodes.Status200OK);
            ok.Value.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetById_Inexistente_Retorna404()
        {
            // Arrange
            var ct = CancellationToken.None;
            _ordenService.Setup(s => s.GetById(999, ct)).ReturnsAsync((DtoOrdenResponse?)null);
            var sut = CreateSut();

            // Act
            var result = await sut.GetById(999, ct);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        // --------------------
        // POST ~/Orden
        // --------------------
        [Fact]
        public async Task InsertNewOrden_ModelStateInvalido_RetornaValidationProblem422()
        {
            // Arrange
            var ct = CancellationToken.None;
            var sut = CreateSut();
            sut.ModelState.AddModelError("req", "invalid");

            var req = new OrdenRequest(cuentaId: 1, nombreActivo: "ALUA", operacion: 'C', cantidad: 10, precio: 12.34m);

            // Act
            var result = await sut.InsertNewOrden(req, ct);

            // Assert
            var obj = result as ObjectResult;
            obj.Should().NotBeNull();
            obj!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
            obj.Value.Should().BeOfType<ValidationProblemDetails>();
        }

        [Fact]
        public async Task InsertNewOrden_Exitoso_Retorna201CreatedAtRouteConId()
        {
            // Arrange
            var ct = CancellationToken.None;
            var req = new OrdenRequest(1, "AL30", 'C', 2, 200m);
            _ordenService.Setup(s => s.CreateOrderAsynck(req, ct)).ReturnsAsync(123);

            var sut = CreateSut();

            // Act
            var result = await sut.InsertNewOrden(req, ct);

            // Assert
            var created = result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            created!.StatusCode.Should().Be(StatusCodes.Status201Created);
            created.RouteName.Should().Be("Orden");
            created.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(123);
            created.Value.Should().BeEquivalentTo(new OrdenController.IdResponse(123));
        }

        [Fact]
        public async Task InsertNewOrden_ActivoNoEncontrado_Retorna404Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            var req = new OrdenRequest(1, "NOEXISTE", 'C', 1, 10m);
            _ordenService
                .Setup(s => s.CreateOrderAsynck(req, ct))
                .ThrowsAsync(new KeyNotFoundException("Ticker NOEXISTE"));

            var sut = CreateSut();

            // Act
            var result = await sut.InsertNewOrden(req, ct);

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFound.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("Activo no encontrado");
        }

        [Fact]
        public async Task InsertNewOrden_ReglaNegocioInvalida_Retorna422Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            var req = new OrdenRequest(1, "GGAL", 'C', 0, null); // Cantidad <= 0, por ejemplo
            _ordenService
                .Setup(s => s.CreateOrderAsynck(req, ct))
                .ThrowsAsync(new ArgumentException("Cantidad debe ser > 0"));

            var sut = CreateSut();

            // Act
            var result = await sut.InsertNewOrden(req, ct);

            // Assert
            var obj = result as ObjectResult;
            obj.Should().NotBeNull();
            obj!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
            obj.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("Reglas de negocio invalidas");
        }

        // --------------------
        // GET ~/Orden
        // --------------------
        [Fact]
        public async Task GetAll_SinElementos_Retorna204()
        {
            // Arrange
            var ct = CancellationToken.None;
            _ordenService.Setup(s => s.GetAll(ct)).ReturnsAsync(new List<DtoOrdenResponse>());
            var sut = CreateSut();

            // Act
            var result = await sut.GetAll(ct);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetAll_ConElementos_Retorna200Ok()
        {
            // Arrange
            var ct = CancellationToken.None;
            var list = new List<DtoOrdenResponse>
        {
            new(cuentaId: 1, nombreActivo: "ALUA", operacion: 'C', cantidad: 10, precio: 1.23m, montoTotal: 12.3m, estado: "Ejecutado")
        };
            _ordenService.Setup(s => s.GetAll(ct)).ReturnsAsync(list);
            var sut = CreateSut();

            // Act
            var result = await sut.GetAll(ct);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(StatusCodes.Status200OK);
            ok.Value.Should().BeEquivalentTo(list);
        }

        // --------------------
        // PUT ~/Orden/{id}/estado
        // --------------------
        [Fact]
        public async Task SetEstado_Exitoso_Retorna204()
        {
            // Arrange
            var ct = CancellationToken.None;
            var body = new SetEstadoRequest(estadoId: 2);
            _ordenService.Setup(s => s.UpdateOrdenEstado(7, 2, ct)).Returns(Task.CompletedTask);
            var sut = CreateSut();

            // Act
            var result = await sut.SetEstado(7, body, ct);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _ordenService.Verify(s => s.UpdateOrdenEstado(7, 2, ct), Times.Once);
        }

        [Fact]
        public async Task SetEstado_OrdenNoEncontrada_Retorna404Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            var body = new SetEstadoRequest (estadoId: 1);
            _ordenService
                .Setup(s => s.UpdateOrdenEstado(99, 1, ct))
                .ThrowsAsync(new KeyNotFoundException("Orden 99"));

            var sut = CreateSut();

            // Act
            var result = await sut.SetEstado(99, body, ct);

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFound.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("Orden no encontrada");
        }

        [Fact]
        public async Task SetEstado_DatosInvalidos_Retorna422Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            var body = new SetEstadoRequest (estadoId:-1);
            _ordenService
                .Setup(s => s.UpdateOrdenEstado(1, -1, ct))
                .ThrowsAsync(new ArgumentException("estadoId invalido"));

            var sut = CreateSut();

            // Act
            var result = await sut.SetEstado(1, body, ct);

            // Assert
            var obj = result as ObjectResult;
            obj.Should().NotBeNull();
            obj!.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
            obj.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("Datos inválidos");
        }

        [Fact]
        public async Task SetEstado_TransicionNoPermitida_Retorna409Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            var body = new SetEstadoRequest(estadoId: 3);
            _ordenService
                .Setup(s => s.UpdateOrdenEstado(2, 3, ct))
                .ThrowsAsync(new InvalidOperationException("No permitido"));

            var sut = CreateSut();

            // Act
            var result = await sut.SetEstado(2, body, ct);

            // Assert
            var conflict = result as ObjectResult;
            conflict.Should().NotBeNull();
            conflict!.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            conflict.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("Conflicto de estado");
        }

        // --------------------
        // DELETE /orden/{id}
        // --------------------
        [Fact]
        public async Task Delete_Exitoso_Retorna204()
        {
            // Arrange
            var ct = CancellationToken.None;
            _ordenService.Setup(s => s.DeleteOrden(5, ct)).Returns(Task.CompletedTask);
            var sut = CreateSut();

            // Act
            var result = await sut.Delete(5, ct);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _ordenService.Verify(s => s.DeleteOrden(5, ct), Times.Once);
        }

        [Fact]
        public async Task Delete_NoEncontrada_Retorna404Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            _ordenService
                .Setup(s => s.DeleteOrden(77, ct))
                .ThrowsAsync(new KeyNotFoundException("77"));

            var sut = CreateSut();

            // Act
            var result = await sut.Delete(77, ct);

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFound.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("Orden no encontrada");
        }

        [Fact]
        public async Task Delete_Conflicto_Retorna409Problem()
        {
            // Arrange
            var ct = CancellationToken.None;
            _ordenService
                .Setup(s => s.DeleteOrden(11, ct))
                .ThrowsAsync(new InvalidOperationException("No se puede eliminar"));

            var sut = CreateSut();

            // Act
            var result = await sut.Delete(11, ct);

            // Assert
            var conflict = result as ObjectResult;
            conflict.Should().NotBeNull();
            conflict!.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            conflict.Value.Should().BeOfType<ProblemDetails>()
                .Which.Title.Should().Be("No se puede eliminar la orden");
        }
    }

}
