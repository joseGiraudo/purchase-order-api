# CLAUDE.md — purchase-orders-api

Contexto del proyecto para Claude Code. Leer antes de generar o modificar código.

## Qué es este proyecto

API backend de un sistema interno de **gestión de órdenes de compra** (Purchase Orders). Empleados crean órdenes, supervisores las aprueban o rechazan, y las órdenes aprobadas siguen un flujo hasta la entrega, con posibilidad de adjuntar factura.

Es un proyecto de práctica — la prioridad es código prolijo, bien organizado en capas, y buenas prácticas por sobre la cantidad de features.

Repo hermano (frontend): `purchase-orders-web` (React + TypeScript). Este repo expone la API que consume.

## Stack

- .NET 8, ASP.NET Core Web API
- Entity Framework Core (Code First, migrations)
- SQL Server
- Autenticación JWT
- xUnit para tests
- GitHub Actions para CI

## Convenciones

- **Todo en inglés**: nombres de clases, propiedades, tablas, endpoints, branches, mensajes de commit. La conversación conmigo puede ser en español, pero el código y el repo van en inglés.
- Arquitectura en capas: `Api`, `Domain`, `Application`, `Infrastructure`. No mezclar responsabilidades entre capas (por ejemplo, no poner lógica de negocio en los controllers).
- Commits en formato convencional: `feat:`, `fix:`, `chore:`, `test:`, `docs:`.
- Una rama por feature: `feature/nombre-corto`, mergeada a `main` vía PR (aunque sea un PR que yo mismo apruebo, para dejar el historial prolijo).

## Estructura de carpetas

```
src/
  PurchaseOrders.Api/
    Controllers/
    Middleware/
    Program.cs

  PurchaseOrders.Domain/
    Entities/
    Enums/
    Interfaces/

  PurchaseOrders.Application/
    Services/
    Dtos/
    Validators/
    Interfaces/

  PurchaseOrders.Infrastructure/
    Persistence/
      AppDbContext.cs
      Migrations/
    Repositories/

tests/
  PurchaseOrders.Tests/
    Services/
    Repositories/
```

Todas las carpetas, subcarpetas, clases e interfaces van en inglés (`Services/`, `Repositories/`, `Dtos/`, `Validators/`, `Entities/`, etc.), sin excepción. Ninguna carpeta debe quedar en español aunque la conversación conmigo sea en español.

## Modelo de dominio

Decisiones ya tomadas — no las cuestiones sin avisar, seguilas:

- Un `Supplier` tiene muchos `Product` (1:N, no N:M).
- Un `PurchaseOrder` pertenece a un solo `Supplier` (el proveedor va a nivel de orden).
- Al crear un `OrderItem`, el `UnitPrice` se copia del `Product.ReferencePrice` en ese momento (price snapshotting) — no se recalcula después si el precio de referencia cambia.
- `User.SupervisorId` es una auto-referencia a `User`, para modelar la jerarquía empleado → supervisor.

### Entidades

| Entidad | Campos clave |
|---|---|
| `User` | `Name`, `Email`, `PasswordHash`, `Role` (`Employee`/`Supervisor`/`Admin`), `SupervisorId` |
| `Supplier` | `Name`, `TaxId`, `ContactName`, `Email` |
| `Product` | `SupplierId`, `Name`, `Description`, `Sku`, `ReferencePrice` |
| `PurchaseOrder` | `Number`, `EmployeeId`, `SupplierId`, `Status`, `CreatedAt`, `TotalAmount`, `RejectionReason` |
| `OrderItem` | `PurchaseOrderId`, `ProductId`, `Quantity`, `UnitPrice` |
| `Invoice` | `PurchaseOrderId`, `InvoiceNumber`, `FileUrl`, `Amount`, `UploadedAt` |
| `StatusHistory` | `PurchaseOrderId`, `PreviousStatus`, `NewStatus`, `UserId`, `ChangedAt`, `Comment` |

### Flujo de estados (`OrderStatus`)

```
Created → Approved → Sent → Delivered
    ↓
Rejected          Cancelled
```

Transiciones inválidas (ej: `Rejected` → `Sent`) deben rechazarse en la capa de aplicación, no en la base de datos. Cada cambio de estado válido debe generar un registro en `StatusHistory`.

### Reglas de autorización

- `Employee`: puede crear órdenes propias, verlas, cancelarlas solo si están en `Created`, y adjuntar facturas a las suyas.
- `Supervisor`: puede ver y aprobar/rechazar órdenes de los `User` que tengan `SupervisorId` apuntando a él.
- `Admin`: acceso total.

## Cómo trabajar conmigo en este repo

- Avanzá de a una feature chica por vez (alineada a los sprints del plan del proyecto), no generes todo el backend de una.
- Después de generar código, corré `dotnet build` y `dotnet test` para validar antes de decir que está listo.
- Si vas a crear una migración de EF Core, explicá qué cambio en el modelo la origina antes de correr `dotnet ef migrations add`.
- No asumas decisiones de modelo que no estén en este archivo — si hace falta una, preguntame primero.