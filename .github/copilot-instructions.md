# AspireTickets Copilot Instructions

## Repository Overview

**AspireTickets** is a .NET Aspire demonstration project implementing a ticket and request management system. It showcases enterprise patterns including CQRS, domain-driven design, OpenTelemetry observability, and workflow automation. The repository contains multiple interconnected services built on .NET 10 using Blazor for the frontend and a RESTful API backend.

**Repository Type:** Multi-project .NET solution with Blazor Server web frontend and ASP.NET Core API backend  
**Primary Language:** C# 14.0  
**.NET Target:** .NET 10.0  
**Size:** Medium (5 projects, ~10K LOC)  
**Key Frameworks:** .NET Aspire, Blazor, Carter, Entity Framework Core, FluentValidation, OpenTelemetry

---

## Project Structure

### Solution Layout

```
AspireTodo.sln (root solution file)
??? src/
?   ??? AspireTodo.AppHost/              # Aspire orchestration project (entry point for running locally)
?   ?   ??? AppHost.cs                   # Defines service topology and health checks
?   ?   ??? launchSettings.json
?   ?
?   ??? AspireTodo.ApiService/           # REST API backend (net10.0)
?   ?   ??? Program.cs                   # Service bootstrap with Carter routes, CQRS setup
?   ?   ??? TicketContext.cs             # Entity Framework DbContext
?   ?   ??? Data/                        # EF migrations and database setup
?   ?   ??? Ticket/                      # Domain folder (endpoints, handlers, validators)
?   ?   ??? appsettings.Development.json # Includes local SQLite DB path
?   ?
?   ??? AspireTodo.Web/                  # Blazor Server frontend (net10.0)
?   ?   ??? Program.cs                   # Blazor bootstrap and HttpClient configuration
?   ?   ??? Components/                  # Blazor components and pages
?   ?   ??? TicketApiClient.cs           # HTTP client for API communication
?   ?   ??? Pages/Tickets.razor          # Example component with logging integration
?   ?   ??? App.razor                    # Root component
?   ?
?   ??? AspireTodo.ServiceDefaults/      # Shared service infrastructure (net10.0)
?   ?   ??? Extensions.cs                # OpenTelemetry, health checks, service discovery setup
?   ?
?   ??? AspireTodo.Shared/               # Shared domain types (net10.0)
?   ?   ??? PaginatedResult.cs           # Pagination response wrapper
?   ?   ??? PaginatedRequest.cs          # Pagination query parameters
?   ?   ??? CQRS/                        # Dispatcher, handler interfaces
?   ?   ??? Behaviors/                   # Validation and logging pipeline behaviors
?   ?   ??? Exceptions/                  # Custom exception handler
?   ?
?   ??? TodoStates.Shared/               # Domain-specific shared library (net9.0)
?       ??? Pagination/                  # Pagination types used by both projects
?
??? AspireTodo.Shared/                   # Alternate shared types folder (legacy, may be deprecated)
??? readme.md                            # Project vision and domain documentation
??? LICENSE                              # MIT License
??? .gitignore                           # Standard .NET ignore patterns

```

### Key File Locations

- **Solution file:** `AspireTodo.sln` (start here)
- **Database:** `src/AspireTodo.ApiService/ticket.db` (SQLite, created by EF migrations)
- **EF Migrations:** `src/AspireTodo.ApiService/Migrations/` (schema and seed data)
- **API Endpoints:** `src/AspireTodo.ApiService/Ticket/` (organized by domain feature)
- **Web Pages:** `src/AspireTodo.Web/Components/Pages/` (Blazor pages)

---

## Build & Development Workflow

### Prerequisites

- **.NET SDK:** 10.0.102 or later (run `dotnet --version` to verify)
- **Visual Studio 2022** (recommended) or VS Code with C# extension
- **Git:** For cloning and version control

### Build Commands (Run These in Order)

**1. Restore & Build the Solution**
```powershell
dotnet build
```
- Restores all NuGet packages
- Compiles all 5 projects
- Always succeeds if .NET 10 SDK is installed
- **Expected time:** ~10 seconds for incremental build
- **First build time:** ~30 seconds (including package downloads)

**2. Run Database Migrations (One-Time Setup)**
```powershell
cd src/AspireTodo.ApiService
dotnet ef database update
```
- Creates/updates the SQLite database (`ticket.db`) with seed data
- Uses Entity Framework migrations from `Migrations/` folder
- **Only required once** or after adding new migrations
- Creates sample ticket data automatically via migration seed

**3. Run the Application (with Aspire Orchestration)**
```powershell
cd src/AspireTodo.AppHost
dotnet run
```
- Starts all services (API, Web, health checks) via .NET Aspire
- Automatically applies service discovery between frontend and backend
- Opens browser to web frontend automatically
- **Expected to see:**
  - "Aspire dashboard starting..." in console
  - Services registered with health checks
  - Web frontend accessible at `https://localhost:XXXXX` (port varies)
  - API Swagger docs at API service health endpoint

**Alternative: Run Without Aspire Orchestration (Direct Service Startup)**
```powershell
# Terminal 1: Start API Service
cd src/AspireTodo.ApiService
dotnet run

# Terminal 2: Start Web Frontend
cd src/AspireTodo.Web
dotnet run
```
- Bypasses Aspire orchestration
- Useful for debugging individual services
- Web must be configured to call API at `https+http://localhost:XXXX` (see `Program.cs`)

---

## Architecture & Key Patterns

### CQRS & Mediator Pattern

- **Location:** `src/AspireTodo.Shared/CQRS/`
- **Pattern:** Command Query Responsibility Segregation with mediator
- **How it works:**
  - Commands & Queries are data transfer objects (DTOs)
  - `ICommandHandler<TCommand, TResult>` and `IQueryHandler<TQuery, TResult>` implement business logic
  - `Dispatcher` service routes commands/queries to correct handlers
  - Services auto-scanned via Scrutor in `Program.cs`

### Validation Pipeline

- **Location:** `src/AspireTodo.Shared/Behaviors/ValidationBehavior.cs`
- **Library:** FluentValidation
- **How it works:**
  - Validators registered for each command/query (e.g., `CreateTicketCommandValidator`)
  - `ValidationBehavior` pipeline intercepts all commands and validates before handler execution
  - Failed validations throw `ValidationException` ? handled by custom exception handler

### OpenTelemetry & Logging

- **Location:** `src/AspireTodo.ServiceDefaults/Extensions.cs`
- **Setup:** `builder.ConfigureOpenTelemetry()` configures structured logging and tracing
- **Injected into components:** `ILogger<T>` available via dependency injection
- **Example:** `src/AspireTodo.Web/Components/Pages/Tickets.razor` logs ticket count via `Logger.LogInformation()`

### Entity Framework Core

- **Provider:** SQLite
- **DbContext:** `src/AspireTodo.ApiService/TicketContext.cs`
- **Migrations:** `src/AspireTodo.ApiService/Migrations/` (auto-applied on startup)
- **Seed data:** Applied via migration `20260204190847_seed-data.cs`

---

## Common Tasks & Gotchas

### Adding a New API Endpoint

1. Create a new folder under `src/AspireTodo.ApiService/Ticket/` (e.g., `CreateTicket/`)
2. Add a `CreateTicketCommand.cs` command class
3. Add a `CreateTicketCommandValidator.cs` (FluentValidation)
4. Add a `CreateTicketCommandHandler.cs` implementing `ICommandHandler<CreateTicketCommand, TicketResponse>`
5. Add a `CreateTicketEndpoint.cs` using Carter module:
   ```csharp
   public class CreateTicketEndpoint : ICarterModule
   {
       public void AddRoutes(IEndpointRouteBuilder app)
       {
           app.MapPost("/tickets", Handle);
       }
       
       public async Task Handle(CreateTicketCommand cmd, IDispatcher dispatcher, CancellationToken ct)
       {
           return await dispatcher.SendAsync(cmd, ct);
       }
   }
   ```
6. Rebuild: `dotnet build`
7. Endpoint automatically discovered by Carter routing

### Debugging JSON Deserialization Issues

- **Common issue:** API response JSON structure doesn't match expected type
- **Solution:** Use `JsonSerializerOptions { PropertyNameCaseInsensitive = true }` in `JsonSerializer.Deserialize()`
- **Example:** See `src/AspireTodo.Web/TicketApiClient.cs` for structured logging of raw JSON responses
- **Always log the raw HTTP response** before attempting deserialization to identify shape mismatches

### Adding a New Database Migration

```powershell
cd src/AspireTodo.ApiService
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Running Tests

Currently, **no test projects exist** in the solution. To add unit tests:
1. Create new project: `dotnet new nunit -n AspireTodo.ApiService.Tests`
2. Add project reference to solution
3. Write tests for handlers and validators
4. Run: `dotnet test`

---

## Known Issues & Workarounds

### SQLite Database Locking on Windows

- **Issue:** If `ticket.db` is locked, migrations fail or app crashes
- **Workaround:** Delete `ticket.db` and run `dotnet ef database update` to recreate
- **Prevention:** Ensure only one service instance accesses the DB at a time

### Service Discovery Configuration

- **Issue:** When running without Aspire, frontend cannot find backend API
- **Workaround:** Set API base address explicitly in `src/AspireTodo.Web/Program.cs`:
  ```csharp
  client.BaseAddress = new("http://localhost:5000"); // hardcoded for local dev
  ```
- **Note:** Aspire uses service discovery (`https+http://apiservice`) to route dynamically

### Missing TodoStates.Shared References

- **Issue:** `namespace TodoStates.Shared` not found after refactoring
- **Workaround:** Ensure `src/TodoStates.Shared/TodoStates.Shared.csproj` is referenced in both `ApiService.csproj` and `Web.csproj`
- **Check:** Both `.csproj` files should have:
  ```xml
  <ProjectReference Include="..\TodoStates.Shared\TodoStates.Shared.csproj" />
  ```

---

## Continuous Integration & Validation

**No GitHub Actions workflows are currently configured.** Consider adding:
- `dotnet build` validation
- `dotnet test` for any future test projects
- Code formatting checks (StyleCop.Analyzers)

---

## Code Style & Conventions

- **C# version:** 14.0 (uses latest language features: records, init properties, null coalescing)
- **Naming:** PascalCase for public types and methods; camelCase for parameters and local variables
- **Async:** Always use `async/await`; methods returning `Task` should be named with `Async` suffix
- **Records:** Preferred for DTOs and immutable types (e.g., `PaginationRequest`, `Ticket`)
- **Nullable:** Nullable reference types enabled; use `?` and `!` operators appropriately
- **Implicit usings:** Enabled; no need to explicitly import common namespaces

---

## Troubleshooting Build Failures

| Error | Cause | Fix |
|-------|-------|-----|
| `NU1105: Unable to find project information` | Missing project reference | Rebuild solution to update project graph |
| `CS0006: Metadata file not found` | Incomplete compilation | Run `dotnet clean && dotnet build` |
| `CS0234: Namespace does not exist` | Missing `using` or bad reference | Check project references in `.csproj` files |
| `EF0002: Migration not found` | Migration not applied to DB | Run `dotnet ef database update` |
| `HTTPS connection refused` | Port conflict or service not running | Check `launchSettings.json` for configured ports |

---

## Final Notes

**Trust these instructions.** Only search the codebase if:
1. An instruction is contradicted by actual code behavior
2. You need to understand a specific implementation detail not covered here
3. You're adding a new feature type not mentioned in examples

**Always run the build after changes:** `dotnet build`

For questions about the domain or design decisions, refer to `readme.md`.
