using System.ComponentModel.DataAnnotations;

namespace DotnetBoiler.Api.Dtos;

// This DTO represents the JSON shape clients send when updating a todo.
// Keeping create/update DTOs separate lets each endpoint have different validation rules later.
public sealed class UpdateTodoRequest
{
    [Required]
    [MinLength(2)]
    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    // bool is non-nullable, so clients must send true or false.
    public bool IsCompleted { get; init; }
}
