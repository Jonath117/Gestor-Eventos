# Contexto de Arquitectura y Reglas de Desarrollo

## 1. Arquitectura Base
* **Estilo:** Monolito Modular basado en Clean Architecture y Domain-Driven Design (DDD).
* **Tecnología:** .NET 10 (C# 14).
* **Gestión de Dependencias:** Uso estricto de `Directory.Packages.props` (Central Package Management). No agregar versiones en los `.csproj`.

## 2. Reglas Estructurales por Módulo
Cada módulo (ej. `Events`, `Identity`, `Logistics`) contiene 4 proyectos físicos (`.csproj`):
* `[Module].Domain`: Entidades, Excepciones de Dominio, Value Objects, Interfaces de Repositorio. (Cero dependencias externas).
* `[Module].Application`: DTOs, Interfaces de Servicios, Lógica de Negocio. (Referencia a Domain).
* `[Module].Infrastructure`: Implementación de Repositorios, EF Core `DbContext` por esquema, Configuración de Entidades. (Referencia a Application).
* `[Module].Presentation`: Controladores HTTP (`[ApiController]`). (Referencia a Application).

## 3. Patrón de Organización en Application (CQRS + Vertical Slices)
Las operaciones dentro de `[Module].Application` NO se organizan por carpetas técnicas (`/Handlers`, `/Requests`), sino por **Feature**.
* **Estructura obligatoria:** `/Features/{Entity}/{OperationName}/`
* **Ejemplo:** `/Features/Events/GetAllEvents/`
* **Archivos esperados por Feature:**
    * `GetAllEventsQuery.cs` (El Request de MediatR).
    * `GetAllEventsHandler.cs` (El IRequestHandler).
    * `GetAllEventsResponse.cs` o DTOs específicos de esta operación.
    * `GetAllEventsValidator.cs` (FluentValidation).

## 4. Reglas de Integración y Host (`Web.API`)
* La inyección de dependencias se encapsula en la capa `Infrastructure` mediante métodos de extensión (ej. `IServiceCollection AddEventsModule()`).
* El proyecto `Web.API` es el único punto de entrada, y solo existe para encadenar las dependencias en `Program.cs` y arrancar el servidor.
* Para la comunicación inter-módulo, está **prohibido** referenciar proyectos de `Infrastructure` o `Domain` de otro módulo. Usar interfaces de integración en `Shared.Kernel` o eventos en memoria (MediatR).  
* También de encapsulará la inyección de controladores en la capa `Presentation` con el patrón `ServiceCollectionExtensions`, en un archivo llamado `DependencyInjection.cs` en la raíz del proyecto Presentation.

## 5. Estilo de Código (.editorconfig Compliance)
* **Namespaces:** Usar `file-scoped namespaces` obligatoriamente.
* **Tipado:** Evitar `var` a menos que el tipo sea estrictamente obvio en la parte derecha de la asignación. Preferir tipos explícitos.
* **Constructores:** Preferir `Primary Constructors` (C# 12+) para inyección de dependencias en Handlers y Controllers.
* **Retornos API:** Los Controllers deben retornar `IActionResult` estandarizados, evitando exponer excepciones crudas al cliente.
* Evitar `using System;`, estamos usando usings implícitos.