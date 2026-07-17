# DotnetBoiler API

A small ASP.NET Core Web API boilerplate for learning .NET from a TypeScript/Node background.

Important language note: .NET backend APIs are usually written in C#, not TypeScript. This project is intentionally C#, but the comments and explanations compare concepts to TypeScript so you can learn the .NET way quickly.

## What You Will Learn

- How a .NET API starts in `Program.cs`
- How controllers map HTTP routes to C# methods
- How request DTOs validate incoming JSON
- How models represent application data
- How dependency injection connects controllers to services
- How to run, test, and extend an ASP.NET Core API

## Project Structure

```text
dotnet-boiler/
├── DotnetBoiler.sln
├── README.md
└── src/
    └── DotnetBoiler.Api/
        ├── Controllers/
        │   └── TodoController.cs
        ├── Dtos/
        │   ├── CreateTodoRequest.cs
        │   └── UpdateTodoRequest.cs
        ├── Models/
        │   └── TodoItem.cs
        ├── Services/
        │   ├── ITodoService.cs
        │   └── InMemoryTodoService.cs
        ├── Program.cs
        ├── DotnetBoiler.Api.csproj
        ├── DotnetBoiler.Api.http
        ├── appsettings.json
        └── appsettings.Development.json
```

## TypeScript To .NET Translation

| TypeScript / Node | C# / ASP.NET Core | In this project |
| --- | --- | --- |
| `package.json` | `.csproj` | `DotnetBoiler.Api.csproj` |
| `index.ts` or `server.ts` | `Program.cs` | app startup and middleware |
| Express router/controller | ASP.NET Core controller | `TodoController.cs` |
| TypeScript interface for request body | DTO class | `CreateTodoRequest.cs` |
| Domain type/interface | Model class | `TodoItem.cs` |
| Service class/module | Service class | `InMemoryTodoService.cs` |
| Dependency injection container such as NestJS | Built-in DI container | `builder.Services...` |
| `npm run dev` | `dotnet run` | run the local API |

## Prerequisites

Install the .NET SDK:

```bash
dotnet --version
```

This boilerplate targets `.NET 8`.

## Run The API

From the repository root:

```bash
dotnet run --project src/DotnetBoiler.Api
```

The terminal will print URLs similar to:

```text
Now listening on: http://localhost:5189
Now listening on: https://localhost:7000
```

Use the HTTP URL for the examples below unless your local HTTPS certificate is trusted.

If you are running inside a restricted sandbox and startup appears to hang before printing the listening URL, disable configuration file watchers:

```bash
DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false dotnet run --project src/DotnetBoiler.Api
```

## Test With curl

Get all todos:

```bash
curl http://localhost:5189/api/todos
```

Create a todo:

```bash
curl -X POST http://localhost:5189/api/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn controllers","description":"Read TodoController.cs"}'
```

Get one todo:

```bash
curl http://localhost:5189/api/todos/REPLACE_WITH_TODO_ID
```

Update a todo:

```bash
curl -X PUT http://localhost:5189/api/todos/REPLACE_WITH_TODO_ID \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn controllers","description":"Updated from curl","isCompleted":true}'
```

Delete a todo:

```bash
curl -X DELETE http://localhost:5189/api/todos/REPLACE_WITH_TODO_ID
```

## API Endpoints

| Method | Path | Purpose |
| --- | --- | --- |
| `GET` | `/api/todos` | List all todos |
| `GET` | `/api/todos/{id}` | Get one todo by id |
| `POST` | `/api/todos` | Create a todo |
| `PUT` | `/api/todos/{id}` | Update a todo |
| `DELETE` | `/api/todos/{id}` | Delete a todo |

## Request And Response Examples

Create request:

```json
{
  "title": "Learn dependency injection",
  "description": "Find AddSingleton in Program.cs"
}
```

Todo response:

```json
{
  "id": "5c2d2e1e-6c4c-4c6a-9a0f-2bb4f0d89a10",
  "title": "Learn dependency injection",
  "description": "Find AddSingleton in Program.cs",
  "isCompleted": false,
  "createdAt": "2026-07-17T00:00:00+00:00",
  "completedAt": null
}
```

Validation example:

```json
{
  "title": "A"
}
```

That fails because `Title` has `[MinLength(2)]`.

## How A Request Flows Through The App

1. `Program.cs` starts the API and registers services.
2. `app.MapControllers()` finds `TodoController`.
3. `TodoController` receives a request such as `POST /api/todos`.
4. ASP.NET Core deserializes JSON into `CreateTodoRequest`.
5. Validation attributes such as `[Required]` and `[MinLength]` are checked.
6. The controller calls `ITodoService`.
7. `InMemoryTodoService` creates, updates, reads, or deletes data.
8. The controller returns an HTTP response such as `200 OK`, `201 Created`, `404 Not Found`, or `204 No Content`.

## Key .NET Concepts

### Solution File

`DotnetBoiler.sln` groups projects together. A real app might later include:

- `DotnetBoiler.Api`
- `DotnetBoiler.Tests`
- `DotnetBoiler.Worker`
- `DotnetBoiler.Domain`

### Project File

`DotnetBoiler.Api.csproj` is similar to `package.json`, but for .NET. It controls:

- target framework
- package references
- compile settings

This project avoids external NuGet packages so it can build without internet access.

### Program.cs

`Program.cs` configures:

- dependency injection
- controllers
- middleware
- route mapping
- server startup

Look for:

```csharp
builder.Services.AddSingleton<ITodoService, InMemoryTodoService>();
```

That means: when something asks for `ITodoService`, provide the same shared `InMemoryTodoService` instance.

### Controllers

Controllers are classes that handle HTTP requests.

```csharp
[HttpGet("{id:guid}")]
public ActionResult<TodoItem> GetById(Guid id)
```

This maps to:

```text
GET /api/todos/{id}
```

### DTOs

DTOs define the shape of incoming or outgoing API data.

Use DTOs instead of exposing every internal model property directly. This keeps your API contract intentional.

### Services

Services hold business logic. Controllers should stay thin:

- read request data
- call a service
- return an HTTP result

The service decides how todos are created, updated, and deleted.

### Dependency Injection

Dependency injection avoids manually creating dependencies inside controllers.

Instead of this:

```csharp
var service = new InMemoryTodoService();
```

ASP.NET Core does this for you:

```csharp
public TodoController(ITodoService todoService)
```

This makes testing and swapping implementations easier.

## Useful .NET CLI Commands

Restore dependencies:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run --project src/DotnetBoiler.Api
```

Format:

```bash
dotnet format
```

## Suggested Learning Path

1. Run the API.
2. Call `GET /api/todos`.
3. Open `TodoController.cs` and match each endpoint to each method.
4. Open `ITodoService.cs` and understand the contract.
5. Open `InMemoryTodoService.cs` and follow how the data changes.
6. Add a new field to `TodoItem`, such as `Priority`.
7. Add that field to `CreateTodoRequest` and `UpdateTodoRequest`.
8. Update the service so the field is saved.
9. Test again with curl.

## Next Improvements To Try

- Add Entity Framework Core and a real database
- Add authentication with JWT
- Add automated tests with xUnit
- Add Swagger/OpenAPI once NuGet package restore is available
- Split business logic into separate projects
- Add Docker support

## Why There Is No Swagger Package Yet

The default ASP.NET Core template often includes Swagger through the `Swashbuckle.AspNetCore` NuGet package. This workspace could not restore packages from NuGet, so the boilerplate avoids external dependencies for now.

When NuGet access is available, you can add Swagger with:

```bash
dotnet add src/DotnetBoiler.Api package Swashbuckle.AspNetCore
```

Then wire up Swagger in `Program.cs`.
