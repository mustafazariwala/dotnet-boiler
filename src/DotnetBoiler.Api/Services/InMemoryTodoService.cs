using DotnetBoiler.Api.Dtos;
using DotnetBoiler.Api.Models;

namespace DotnetBoiler.Api.Services;

// This service stores todos in memory so the app is easy to run without a database.
// The data resets every time the application restarts.
public sealed class InMemoryTodoService : ITodoService
{
    // Dictionary gives fast lookup by id.
    // TypeScript comparison: similar to Map<string, TodoItem>.
    private readonly Dictionary<Guid, TodoItem> _todos = new();

    public InMemoryTodoService()
    {
        // Seed data makes the API useful immediately after startup.
        var firstTodo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Learn ASP.NET Core routing",
            Description = "Open TodoController.cs and inspect the route attributes.",
            IsCompleted = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var secondTodo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Practice dependency injection",
            Description = "Find where ITodoService is registered in Program.cs.",
            IsCompleted = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _todos[firstTodo.Id] = firstTodo;
        _todos[secondTodo.Id] = secondTodo;
    }

    public IReadOnlyCollection<TodoItem> GetAll()
    {
        return _todos.Values
            .OrderBy(todo => todo.CreatedAt)
            .ToArray();
    }

    public TodoItem? GetById(Guid id)
    {
        _todos.TryGetValue(id, out var todo);
        return todo;
    }

    public TodoItem Create(CreateTodoRequest request)
    {
        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            IsCompleted = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _todos[todo.Id] = todo;
        return todo;
    }

    public TodoItem? Update(Guid id, UpdateTodoRequest request)
    {
        var todo = GetById(id);

        if (todo is null)
        {
            return null;
        }

        todo.Title = request.Title.Trim();
        todo.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        todo.IsCompleted = request.IsCompleted;

        // Keep CompletedAt synchronized with IsCompleted.
        todo.CompletedAt = request.IsCompleted ? DateTimeOffset.UtcNow : null;

        return todo;
    }

    public bool Delete(Guid id)
    {
        return _todos.Remove(id);
    }
}
