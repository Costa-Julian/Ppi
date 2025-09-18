PPI .NET Challenge


El objetivo de este proyecto es mostrar una implementación **clara y mantenible** de una API de inversiones con ASP.NET Core.
Incluye cálculos por tipo de activo mediante Strategy, persistencia desacoplada con Unit of Work, Swagger para documentación y tests unitarios para reglas críticas.

## Prerequisitos:

.NET 8.0 SDK

## Estructura de la solución
```bash
Ppi.sln
├─ src/
│  ├─ Core/
│  │  ├─ Domain/        # Entidades, reglas, Value Objects
│  │  └─ Application/   # Casos de uso, servicios, puertos/repos
│  ├─ Infrastructure/   # EF Core, repos, UoW, mapeos
│  └─ Presentation/
│     └─ WebApi/        # API ASP.NET Core + Swagger
└─ tests/
   ├─ Application.Core.CalculoActivoTest/
   └─ Presentation.OrdenControlesTest/           
```

## Run
```bash
dotnet restore
dotnet build
dotnet run --project src/Presentation/WebApi/WebApi.csproj
```

## Funcionalidad de dominio


- Acción (1)

  Precio no se recibe en la request (se obtiene del repositorio de activos)

  Comisión: 0.6% del monto

  Impuesto: 21% sobre la comisión

- Bono (2)

  Se recibe precio y cantidad

  Comisión: 0.2% del monto

  Impuesto: 21% sobre la comisión

- FCI (3)

  Precio unitario provisto por la request

  Sin comisión ni impuestos

## Estrategia de cálculo:

- ICalculoTotal + CalculoOrdenFactory (elige la estrategia por CanHandle(Activo)).

- IUnitOfWork garantiza una transacción y un SaveChanges por caso de uso.

## Documentación (Swagger)

- UI: /swagger

- JSON: /swagger/v1/swagger.json 

Los endpoints principales:

- POST /Orden — Crea una orden (201 Created + Location)

- GET /Orden/{id} — Obtiene una orden (200/404)

- GET /Orden — Lista órdenes (200 o 204)

- PUT /Orden/{id}/estado — Cambia estado (204, idempotente)

- DELETE /api/orden/{id} — Elimina una orden (204/404/409)

## Ejemplo curl (crear orden de Acción; no envia precio):
```bash
curl -X POST http://localhost:8080/Orden \
  -H "Content-Type: application/json" \
  -d '{"cuentaId":135,"nombreActivo":"AAPL","operacion":"C","cantidad":5,"precio":null}'
```

Testing

Se incluyen tests unitarios para reglas críticas y cálculos por tipo de activo.

Ejecutar:
```bash
dotnet test
```

Cobertura esperada:

- Validaciones de request y reglas (precio requerido/denegado según tipo)
- Cálculo total por tipo (Acción 0.6% + IVA 21%; Bono 0.2% + IVA 21%; FCI sin cargos)
- Errores esperados (activo inexistente, transiciones de estado no permitidas)
- Idempotencia en UpdateOrdenEstadoAsync

## Tecnologías

- ASP.NET Core 8
- Entity Framework Core 8
- Patrón Strategy para cálculos y Unit of Work para persistencia
- Swashbuckle.AspNetCore
- xUnit, FluentAssertions, Moq/NSubstitute 


```bash
