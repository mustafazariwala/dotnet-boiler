namespace DotnetBoiler.Api.Models;

// A model represents the data your app works with internally.
// This project keeps it simple, but in larger apps models often map to database tables.
public sealed class TodoItem
{
    // Guid is a globally unique identifier.
    // TypeScript comparison: this is usually represented as a string on the frontend.
    public Guid Id { get; init; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? CompletedAt { get; set; }
}
