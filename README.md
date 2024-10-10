Aqui está um exemplo de **README.md** que você pode usar para o projeto **ProductsSample** com .NET 8:

---

# ProductsSample Web API

This is a sample project built using **Clean Architecture**, implementing **CQRS** with **Mediator** and following the **Result Pattern**. The project is developed using **.NET 8** and includes unit tests with **xUnit**. The project uses **Entity Framework Core (InMemory)** for data persistence during testing.

## Technologies Used

- **.NET 8**
- **Clean Architecture**
- **CQRS (Command Query Responsibility Segregation)**
- **Mediator Pattern**
- **Result Pattern** for handling success and failure in a consistent way
- **Entity Framework Core (InMemory)** for testing and data storage
- **xUnit** for unit testing
- **Swagger** for API documentation

## Getting Started

### Prerequisites

- **.NET SDK 8.0** or later
- **Visual Studio** or **VS Code** with C# support
- **Postman** or **curl** for testing APIs (optional)
- **Git** (optional)

### Setup and Running the Application

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/ProductsSample.git
   cd ProductsSample
   ```

2. **Restore NuGet packages:**

   Navigate to the solution root and restore the required NuGet packages by running:

   ```bash
   dotnet restore
   ```

3. **Run the application:**

   You can run the application in development mode using the following command:

   ```bash
   dotnet run --launch-profile Development --project ProductsSample.Api
   ```

   Alternatively, you can use **Visual Studio** to open the solution, set **ProductsSample.Api** as the startup project, and run it.

4. **API Endpoints:**

   After running the application, you can explore the API via Swagger at:

   ```
   http://localhost:5079/swagger
   ```

   Swagger provides an interactive UI for testing the endpoints.

### Launch Settings

The `launchSettings.json` file contains the configuration for different environments:

- **Development**: `https://localhost:7090` or `http://localhost:5079`
- **Staging**: Similar to Development but with environment set to `Staging`.
- **Production**: Configured to use HTTPS.

You can switch environments by passing the `--launch-profile` option with `dotnet run`, or by setting the profile in your IDE.

### Testing

Unit tests are implemented using **xUnit** with the **FluentAssertions** library for assertions. The project uses **InMemory** database with **Entity Framework Core** for testing data persistence.

To run the unit tests, use the following command:

```bash
dotnet test
```

### Clean Architecture Overview

The project follows **Clean Architecture** principles:

- **Api Layer** (`ProductsSample.Api`): This contains the controllers and is the entry point for handling HTTP requests.
- **Application Layer** (`ProductsSample.Application`): Contains the core business logic and **CQRS Handlers**.
- **Domain Layer** (`ProductsSample.Domain`): Contains the core domain models and entities.
- **Infrastructure Layer** (`ProductsSample.Infrastructure`): Handles external concerns, such as database access, using **EF Core InMemory** for testing purposes.

### CQRS and Mediator

The application uses the **Mediator Pattern** to handle **Commands** and **Queries**:

- **Commands**: For write operations (e.g., `AddNewProductCommand`, `EditProductDetailsCommand`, `DeleteProductCommand`).
- **Queries**: For read operations (e.g., `GetProductByIdQuery`, `ListAllProductsQuery`).

All these commands and queries are handled by the respective **Handler** classes that process the requests and return a consistent **Result**.

### Result Pattern

The **Result Pattern** is used to wrap responses, ensuring consistent handling of success and failure scenarios. Each command and query returns a `Result<T>` object, where `T` is the response type.

```csharp
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Response { get; set; }
    public string ErrorMessage { get; set; }
}
```

### Example Commands and Queries

#### Add New Product Command

```csharp
public class AddNewProductCommand : ICommand<Result<string>>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
```

#### Get Product By Id Query

```csharp
public class GetProductByIdQuery : IQuery<Result<object>>
{
    public string ProductId { get; set; }
}
```

### Swagger

The API exposes Swagger documentation for easy exploration of endpoints. You can view it at:

```
http://localhost:5079/swagger
```

---

### Contribution Guidelines

Feel free to fork this repository, make improvements, and submit pull requests. Make sure to follow coding standards and include unit tests for any new features.
