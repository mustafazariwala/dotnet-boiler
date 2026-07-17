using DotnetBoiler.Api.Dtos;
using DotnetBoiler.Api.Models;
using DotnetBoiler.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetBoiler.Api.Controllers;

// [ApiController] turns on helpful API behavior:
// - request body validation
// - automatic 400 responses for invalid models
// - cleaner binding from route/query/body data
[ApiController]

// The route prefix for every action in this controller.
// Final URL example: GET /api/todos
[Route("api/todos")]
public sealed class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    // Constructor injection: ASP.NET Core sees ITodoService here and provides the registered implementation.
    // TypeScript comparison: similar to passing a service object into a class constructor.
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    // GET /api/todos
    // Returns every todo item.
    [HttpGet]
    public ActionResult<IReadOnlyCollection<TodoItem>> GetAll()
    {
        return Ok(_todoService.GetAll());
    }

    // GET /api/todos/{id}
    // The "{id:guid}" route constraint means this action only matches valid GUID values.
    [HttpGet("{id:guid}")]
    public ActionResult<TodoItem> GetById(Guid id)
    {
        var todo = _todoService.GetById(id);

        if (todo is null)
        {
            return NotFound(new { message = $"Todo with id '{id}' was not found." });
        }

        return Ok(todo);
    }

    // POST /api/todos
    // [FromBody] tells ASP.NET Core to deserialize JSON from the request body into CreateTodoRequest.
    [HttpPost]
    public ActionResult<TodoItem> Create([FromBody] CreateTodoRequest request)
    {
        var createdTodo = _todoService.Create(request);

        // CreatedAtAction returns HTTP 201 and includes a Location header pointing to the new resource.
        return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
    }

    // PUT /api/todos/{id}
    // PUT usually means "replace/update this resource".
    [HttpPut("{id:guid}")]
    public ActionResult<TodoItem> Update(Guid id, [FromBody] UpdateTodoRequest request)
    {
        var updatedTodo = _todoService.Update(id, request);

        if (updatedTodo is null)
        {
            return NotFound(new { message = $"Todo with id '{id}' was not found." });
        }

        return Ok(updatedTodo);
    }

    // DELETE /api/todos/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var wasDeleted = _todoService.Delete(id);

        if (!wasDeleted)
        {
            return NotFound(new { message = $"Todo with id '{id}' was not found." });
        }

        // 204 means the request succeeded and there is no response body.
        return NoContent();
    }
}
