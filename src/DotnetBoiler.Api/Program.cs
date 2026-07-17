using DotnetBoiler.Api.Services;

// Program.cs is the entry point for a modern ASP.NET Core app.
// TypeScript comparison: this file plays a role similar to an Express server.ts/index.ts file.

var builder = WebApplication.CreateBuilder(args);

// The service container is .NET's built-in dependency injection system.
// Anything registered here can be requested in constructors, like TodoController(ITodoService service).
builder.Services.AddControllers();
builder.Services.AddSingleton<ITodoService, InMemoryTodoService>();

var app = builder.Build();

// Middleware runs in order for every HTTP request.
// Add app.UseHttpsRedirection() here later after HTTPS is configured for your environment.

// Authorization middleware is included now so it is clear where auth would plug in later.
// This boilerplate does not require login yet.
app.UseAuthorization();

// This discovers all classes decorated with [ApiController] and maps their route attributes.
app.MapControllers();

// Starts Kestrel, the ASP.NET Core web server.
app.Run();
