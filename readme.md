# GenericQueryable

GenericQueryable is a library designed to enable asynchronous query execution and data fetching in the application layer **without direct dependencies on infrastructure or ORM-specific code**.  
It abstracts over static async methods from ORMs (such as `ToListAsync` and `SingleOrDefaultAsync` in Entity Framework or NHibernate), delegating the execution to a provider bound to `IQueryable`.

## Why?

In layered architecture, the application layer should remain independent of infrastructure concerns.  
However, async LINQ methods like `ToListAsync()` or `SingleOrDefaultAsync()` are static and tied to specific ORMs, introducing direct dependencies and making testing more difficult.

GenericQueryable addresses this by introducing a unified abstraction over asynchronous query execution.  
It allows application code to perform async fetch operations through an injected provider, **without referencing ORM-specific APIs**.

## Motivation

Most modern ORMs expose async LINQ extensions as static methods. While convenient, this tightly couples calling code to a specific ORM and breaks separation of concerns.  
This makes infrastructure "leak" into the application layer and complicates testing and maintainability.

GenericQueryable decouples async query logic by encapsulating it behind an interface.  
This design promotes clean architecture, simplifies mocking, and allows application code to remain agnostic of the underlying data access implementation.

## Extension Methods

GenericQueryable includes basic async extension methods for `IQueryable<T>` in [`GenericQueryableExtensions.cs`](./src/GenericQueryable/GenericQueryableExtensions.cs).  
You can add your own ORM-specific extensions (e.g., `ContainsAsync`, `AnyAsync`, etc.) by creating static extension classes in your infrastructure layer.

## Unit Testing

You can test code using GenericQueryable without any real database by assigning a mock async executor for `IQueryable<T>` using the `.AsDefaultGenericQueryable()` helper:

```csharp
.AsDefaultGenericQueryable();
```
	 
## EntityFramework

To integrate GenericQueryable with Entity Framework Core, call UseGenericQueryable() on DbContextOptionsBuilder when configuring your DbContext.

```csharp
services.AddDbContext<MyDbContext>(options =>
    options.UseGenericQueryable() // Registers GenericQueryable async query provider for EF Core
```

## Roadmap

- Add support for `Include` methods with strongly-typed lambda expressions to enable more expressive eager loading.  
- Implement `Lock` functionality to provide mechanisms for explicit locking and concurrency control in queries.