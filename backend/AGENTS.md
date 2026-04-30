# Architecture Context and Development Rules

## 1. Base Architecture
* **Style:** Modular Monolith based on Clean Architecture and Domain-Driven Design (DDD).
* **Technology:** .NET 10 (C# 14).
* **Dependency Management:** Strict use of `Directory.Packages.props` (Central Package Management). Do not add versions in `.csproj` files.

## 2. Structural Module Rules
Each module (e.g., `Events`, `Identity`, `Logistics`) contains 4 physical projects (`.csproj`):
* `[Module].Domain`: Entities, Domain Exceptions, Value Objects, Repository Interfaces. (Zero external dependencies).
* `[Module].Application`: DTOs, Service Interfaces, Business Logic. (References Domain).
* `[Module].Infrastructure`: Repository Implementations, EF Core `DbContext` per schema, Entity Configurations. (References Application).
* `[Module].Presentation`: HTTP Controllers (`[ApiController]`). (References Application).

## 3. Application Organization Pattern (CQRS + Vertical Slices)
Operations within `[Module].Application` MUST NOT be organized by technical folders (`/Handlers`, `/Requests`), but by **Feature**.
* **Mandatory structure:** `/Features/{Entity}/{OperationName}/`
* **Example:** `/Features/Events/GetAllEvents/`
* **Expected files per Feature:**
    * `GetAllEventsQuery.cs` (MediatR Request).
    * `GetAllEventsHandler.cs` (IRequestHandler).
    * `GetAllEventsResponse.cs` or operation-specific DTOs.
    * `GetAllEventsValidator.cs` (FluentValidation).

## 4. Integration and Host Rules (`Web.API`)
* Dependency injection is encapsulated in the `Infrastructure` layer via extension methods (e.g., `IServiceCollection AddEventsModule()`).
* The `Web.API` project is the only entry point, existing only to chain dependencies in `Program.cs` and start the server.
* Inter-module communication: It is **forbidden** to reference `Infrastructure` or `Domain` projects from another module. Use integration interfaces in `Shared.Kernel` or in-memory events (MediatR).
* Controller injection will also be encapsulated in the `Presentation` layer using the `ServiceCollectionExtensions` pattern, in a file named `DependencyInjection.cs` at the root of the Presentation project.

## 5. Code Style (.editorconfig Compliance)
* **Namespaces:** Use `file-scoped namespaces` obligatorily.
* **Typing:** Avoid `var` unless the type is strictly obvious from the right side of the assignment. Prefer explicit types.
* **Constructors:** Prefer `Primary Constructors` (C# 12+) for dependency injection in Handlers and Controllers.
* **API Returns:** Controllers must return standardized `IActionResult`, avoiding exposing raw exceptions to the client.
* **Clean Code:** 
    * Avoid `using System;` (we use implicit usings).
    * **No Unused Code:** Do NOT write code that is not used. If a `FluentValidation` validator is created, it MUST be integrated and used in the corresponding flow. Avoid "code smells" related to dead or unreachable code.