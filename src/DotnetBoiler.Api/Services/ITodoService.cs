using DotnetBoiler.Api.Dtos;
using DotnetBoiler.Api.Models;

namespace DotnetBoiler.Api.Services;

// Interfaces define a contract without committing to a specific implementation.
// TypeScript comparison: very similar to a TS interface with method signatures.
public interface ITodoService
{
    IReadOnlyCollection<TodoItem> GetAll();

    TodoItem? GetById(Guid id);

    TodoItem Create(CreateTodoRequest request);

    TodoItem? Update(Guid id, UpdateTodoRequest request);

    bool Delete(Guid id);
}
