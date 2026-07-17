import {
  Check,
  ExternalLink,
  Github,
  Heart,
  Loader2,
  Plus,
  RefreshCcw,
  Sparkles,
  Trash2
} from "lucide-react";
import { FormEvent, useEffect, useMemo, useState } from "react";
import { createTodo, deleteTodo, getTodos, TodoItem, updateTodo } from "./api";

export function App() {
  const [todos, setTodos] = useState<TodoItem[]>([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const completedCount = useMemo(
    () => todos.filter((todo) => todo.isCompleted).length,
    [todos]
  );

  async function loadTodos() {
    setIsLoading(true);
    setError(null);

    try {
      setTodos(await getTodos());
    } catch {
      setError("Could not load todos. Make sure the .NET API is running.");
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    void loadTodos();
  }, []);

  async function handleCreate(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (title.trim().length < 2) {
      setError("Title must be at least 2 characters.");
      return;
    }

    setError(null);
    const created = await createTodo({
      title,
      description: description || undefined
    });

    setTodos((current) => [...current, created]);
    setTitle("");
    setDescription("");
  }

  async function handleToggle(todo: TodoItem) {
    const updated = await updateTodo(todo.id, {
      title: todo.title,
      description: todo.description ?? undefined,
      isCompleted: !todo.isCompleted
    });

    setTodos((current) =>
      current.map((item) => (item.id === updated.id ? updated : item))
    );
  }

  async function handleDelete(id: string) {
    await deleteTodo(id);
    setTodos((current) => current.filter((todo) => todo.id !== id));
  }

  return (
    <main className="shell">
      <div className="floatLayer" aria-hidden="true">
        <span className="floatBox boxOne" />
        <span className="floatBox boxTwo" />
        <span className="floatBox boxThree" />
        <span className="floatBox boxFour" />
      </div>

      <section className="workspace" aria-label="Todo workspace">
        <header className="topbar">
          <div>
            <p className="eyebrow">
              <Sparkles size={15} aria-hidden="true" />
              ASP.NET Core + React
            </p>
            <h1>Todo API Client</h1>
            <p className="intro">
              A polished React surface backed by your .NET boilerplate API.
            </p>
          </div>
          <button className="iconButton" type="button" onClick={loadTodos} title="Refresh todos">
            {isLoading ? <Loader2 className="spin" size={18} /> : <RefreshCcw size={18} />}
          </button>
        </header>

        <div className="summary" aria-label="Todo summary">
          <div>
            <span>{todos.length}</span>
            <p>Total</p>
          </div>
          <div>
            <span>{completedCount}</span>
            <p>Done</p>
          </div>
          <div>
            <span>{todos.length - completedCount}</span>
            <p>Open</p>
          </div>
        </div>

        <form className="composer" onSubmit={handleCreate}>
          <input
            value={title}
            onChange={(event) => setTitle(event.target.value)}
            placeholder="New todo title"
            aria-label="New todo title"
          />
          <input
            value={description}
            onChange={(event) => setDescription(event.target.value)}
            placeholder="Description"
            aria-label="New todo description"
          />
          <button type="submit">
            <Plus size={18} />
            Add
          </button>
        </form>

        {error ? <p className="error">{error}</p> : null}

        <div className="list" aria-live="polite">
          {isLoading ? (
            <div className="empty">Loading todos...</div>
          ) : todos.length === 0 ? (
            <div className="empty">No todos yet.</div>
          ) : (
            todos.map((todo, index) => (
              <article className="todo" key={todo.id} style={{ animationDelay: `${index * 70}ms` }}>
                <button
                  className={todo.isCompleted ? "status done" : "status"}
                  type="button"
                  onClick={() => handleToggle(todo)}
                  title={todo.isCompleted ? "Mark as open" : "Mark as done"}
                >
                  <Check size={16} />
                </button>
                <div className="todoBody">
                  <h2>{todo.title}</h2>
                  <p>{todo.description || "No description"}</p>
                </div>
                <button
                  className="iconButton danger"
                  type="button"
                  onClick={() => handleDelete(todo.id)}
                  title="Delete todo"
                >
                  <Trash2 size={18} />
                </button>
              </article>
            ))
          )}
        </div>
      </section>

      <footer className="footer">
        <span>
          Made with <Heart size={15} aria-hidden="true" /> by{" "}
          <a href="https://thezariwala.com/" target="_blank" rel="noreferrer">
            Mustafa Zariwala
            <ExternalLink size={14} aria-hidden="true" />
          </a>
        </span>
        <a
          className="repoLink"
          href="https://github.com/mustafazariwala/dotnet-boiler"
          target="_blank"
          rel="noreferrer"
        >
          <Github size={16} aria-hidden="true" />
          Fork mustafazariwala/dotnet-boiler
        </a>
      </footer>
    </main>
  );
}
