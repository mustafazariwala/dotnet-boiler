export interface TodoItem {
  id: string;
  title: string;
  description: string | null;
  isCompleted: boolean;
  createdAt: string;
  completedAt: string | null;
}

export interface CreateTodoRequest {
  title: string;
  description?: string;
}

export interface UpdateTodoRequest {
  title: string;
  description?: string;
  isCompleted: boolean;
}

// Keeping fetch calls in one file makes it easy to see how React talks to the .NET API.
// In development, Vite proxies /api to http://localhost:5189. In production, .NET serves both frontend and API.
async function request<T>(url: string, options?: RequestInit): Promise<T> {
  const response = await fetch(url, {
    headers: {
      "Content-Type": "application/json",
      ...options?.headers
    },
    ...options
  });

  if (!response.ok) {
    throw new Error(`Request failed with ${response.status}`);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export function getTodos(): Promise<TodoItem[]> {
  return request<TodoItem[]>("/api/todos");
}

export function createTodo(payload: CreateTodoRequest): Promise<TodoItem> {
  return request<TodoItem>("/api/todos", {
    method: "POST",
    body: JSON.stringify(payload)
  });
}

export function updateTodo(id: string, payload: UpdateTodoRequest): Promise<TodoItem> {
  return request<TodoItem>(`/api/todos/${id}`, {
    method: "PUT",
    body: JSON.stringify(payload)
  });
}

export function deleteTodo(id: string): Promise<void> {
  return request<void>(`/api/todos/${id}`, {
    method: "DELETE"
  });
}
