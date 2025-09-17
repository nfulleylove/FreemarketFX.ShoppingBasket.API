# FreemarketFX.ShoppingBasket.API

As part of Freemarket FX's code challenge, this project is a REST-based Web API for an example online shopping basket.

--- 

## Features

- Create a basket.
- Retrieve a basket.
- Add a product to the basket.
- Add multiple of the same product to the basket.
- Get the total cost for the basket (including 20% VAT).
- Get the total cost without VAT.
- Add a discounted item to the basket.
- Add shipping cost to the UK.

---

## Getting Started

### Prerequisites

> Ensure that the following is installed on your machine prior to running the application:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- [Git](https://git-scm.com/).
- Visual Studio 2022.

To run the API locally, perform the following steps:

1. Clone the repo.

`https://github.com/nfulleylove/FreemarketFX.ShoppingBasket.API.git`

2. Restore the dependencies - Right-click on the solution > Restore NuGet Packages.

![restore-dependencies](https://github.com/user-attachments/assets/425aec8d-41b6-4162-a631-010f5d85ca8c)

3. Debug the application - Click on the start button to run the application.

![start-program](https://github.com/user-attachments/assets/25b6548c-5fb4-4039-94f7-6d77e5778430)

---

## Architecture

The API is built in .NET 8, and includes two projects:

1. FreemarketFX.ShoppingBasket.API - The web API.
2. FreemarketFX.ShoppingBasket.API.Tests - The tests project.

### FreemarketFX.ShoppingBasket.API Overview

The API uses EntityFramework.Core with an in-memory database.

It uses a Swagger UI for development.

The structure of the API is as follows:

| Folder               | Description                                                               |
| -------------------- | ------------------------------------------------------------------------- |
| /Controllers         | Contains the Baskets endpoints.                                           |
| /Data                | Contains the repositories and the database context.                       |
| /DataTransferObjects | Contains the data transfer objects that are returned by the API.          |
| /Mappers             | Contains the mappers to convert a domain model to a data transfer object. |
| /Models              | Contains the domain models.                                               |
| /Services            | Contains the services used to call the database and convert the models.   |

#### Program.cs

`Program.cs` registers the database context, repositories, and services for dependency injection throughout the project.

It contains a method named `InitialiseDevelopmentDatabase()`, which loads test data in development.

### FreemarketFX.ShoppingBasket.API.Tests

The tests project uses XUnit as a test runner and NSubstitute for mocking services and repositories.

It contains tests for the mappers, services, and controllers.

Rebuild the solution, then right-click on the tests project > Run Tests to run the unit tests.

The Test Explorer window will show the results:

<img width="483" height="726" alt="image" src="https://github.com/user-attachments/assets/01651820-ae11-4699-829a-4d037e498d2e" />

---

## Test Examples

The database for the API has been seeded with example values in development.

These example values have also been added to the Swagger UI to streamline debugging.

They are added in `Program.cs` via the following code:

```csharp
context.Baskets.Add(new()
{
    Id = new Guid("A1D8D4B0-551D-4A26-A417-5EE3FFE3A4E2"),
    BasketProducts = [
        new (){
            BasketId = new Guid("A1D8D4B0-551D-4A26-A417-5EE3FFE3A4E2"),
            ProductId = new Guid("3CF63300-2F3A-4066-A1D5-47A33D120F36")
        }
        ]
});

context.Products.AddRange(
    new Product() { Id = new Guid("{08254E34-D2EF-4065-B646-CB42D0F87EAF}"), Name = "Table", Price = 399.99M },
    new Product() { Id = new Guid("{43AA1547-F36C-480A-A680-2917BAAD4DB1}"), Name = "Chair", Price = 44.99M },
    new Product() { Id = new Guid("3CF63300-2F3A-4066-A1D5-47A33D120F36"), Name = "Lamp", Price = 19.99M, IsDiscounted = true },
    new Product() { Id = new Guid("{A44EDEDD-937A-48BC-BE6A-3F73A3685D44}"), Name = "Book", Price = 5.99M }
);
```

Feel free to pick IDs from the seeded values to test it out.

---

## Roadmap

- Add multiple items to the basket
- Remove an item from the basket
- Add a discount code to the basket (excluding discounted items)
- Add shipping cost to other countries
