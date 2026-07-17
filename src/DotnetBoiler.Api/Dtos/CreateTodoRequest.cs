using System.ComponentModel.DataAnnotations;

namespace DotnetBoiler.Api.Dtos;

// DTO means Data Transfer Object.
// This type represents the JSON shape clients send when creating a todo.
// TypeScript comparison:
// interface CreateTodoRequest { title: string; description?: string; }
public sealed class CreateTodoRequest
{
    // Data annotations are validation rules read by ASP.NET Core.
    // [Required] and [MinLength] make invalid request bodies return HTTP 400 automatically.
    [Required]
    [MinLength(2)]
    public string Title { get; init; } = string.Empty;

    // A nullable string means clients may omit this property or send null.
    public string? Description { get; init; }
}
