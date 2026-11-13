# BookGarden

A Library Management System built with **ASP.NET Core MVC**, **C#**,
**Entity Framework Core**, and **SQL Server**.

BookGarden is a learning project focused on building a real-world .NET
application step by step, with an emphasis on clean architecture,
database access, CRUD operations, and authentication.

## Project Goals

-   Build a practical ASP.NET Core MVC application.
-   Understand the complete request flow from MVC controllers to
    services and the database.
-   Practice Entity Framework Core, dependency injection, validation,
    and database migrations.
-   Add secure user authentication and role-based authorization.

## Technology Stack

-   **Language:** C#
-   **Framework:** ASP.NET Core MVC (.NET 10)
-   **ORM:** Entity Framework Core 10
-   **Database:** Microsoft SQL Server / LocalDB
-   **Frontend:** HTML, CSS, JavaScript, Razor Views
-   **Authentication:** ASP.NET Core Identity (implementation in
    progress)

## Solution Structure

The solution is separated into projects to keep responsibilities
organized:

  -----------------------------------------------------------------------
  Project                             Responsibility
  ----------------------------------- -----------------------------------
  `BookGarden`                        ASP.NET Core MVC web application,
                                      controllers, views, configuration,
                                      and application startup

  `BookGardenDataAccess`              Entity Framework Core
                                      `ApplicationDbContext`, database
                                      access, and migrations

  `BookGarden.Business`               Business-layer models and related
                                      business logic

  `BookGarden.Models`                 Shared model types, where
                                      applicable

  `BookGarden.Utility`                Shared utility code
  -----------------------------------------------------------------------

> Project contents may evolve as the application is developed. Check the
> solution in Visual Studio for the current project and folder
> structure.

## Features

### Product Management

-   Product CRUD functionality.
-   Product listing and details pages.
-   Product information such as title, author, description, ISBN,
    prices, category, and cover image.
-   Product image upload and display.
-   Rich-text product descriptions using Quill.
-   Confirmation dialogs for destructive actions using SweetAlert2.
-   Product data loaded through an API endpoint and displayed with
    DataTables.

### Category Management

-   Category CRUD functionality.
-   Entity Framework Core database integration.
-   Category seed data.

### Home Page and Product Details

-   Home page displays available products.
-   Product details page displays the selected product's information.
-   Invalid or missing product IDs return a 404 response.

### Identity and Security --- In Progress

-   ASP.NET Core Identity Entity Framework Core package added.
-   `ApplicationDbContext` configured to inherit from
    `IdentityDbContext<IdentityUser>`.
-   Identity database migration created and applied.
-   Identity tables verified in SQL Server.
-   Custom `ApplicationUser` model created; integration with the
    DbContext and the remaining registration, login, and role workflows
    are still in progress.

## Architecture Overview

The application uses dependency injection and a service layer to
separate MVC controllers from data-access implementation.

Typical request flow:

``` text
Browser
   |
   v
MVC Controller
   |
   v
Business Service Interface
   |
   v
Service Implementation
   |
   v
ApplicationDbContext
   |
   v
Entity Framework Core
   |
   v
SQL Server
```

## Getting Started

### Prerequisites

-   Visual Studio with ASP.NET and web development workload.
-   .NET 10 SDK.
-   SQL Server LocalDB or a compatible SQL Server instance.
-   Git.

### Setup

1.  Clone the repository:

    ``` bash
    git clone <your-repository-url>
    ```

2.  Open the solution in Visual Studio. Select the solution file that
    exists in your checkout (for example,
    `BookGarden.Architecture.sln`).

3.  Check the connection string in the web project's
    `appsettings.Development.json` and update it for your SQL Server
    environment if necessary.

4.  Restore NuGet packages:

    ``` bash
    dotnet restore
    ```

5.  Build the solution:

    ``` bash
    dotnet build
    ```

6.  Run the `BookGarden` web project from Visual Studio or with the
    appropriate `dotnet run` command.

> Database migrations are part of the development workflow. Review the
> migration state and connection string before applying migrations to
> your own database.

## Development Notes

-   Keep secrets and machine-specific connection details out of source
    control.
-   Review generated EF Core migrations before applying them.
-   Preserve existing Category and Product data when changing the
    database schema.
-   Run the application and test affected pages after each significant
    change.
-   Commit small, meaningful changes with clear Git commit messages.

## Current Learning Progress

The project is being developed incrementally while studying an ASP.NET
Core/.NET course.

Completed work includes core product and category management, the home
page, and product details. The current development focus is **ASP.NET
Core Identity**, followed by registration, login, user profile fields,
and role-based authorization.

## License

This project is currently a personal learning project. Add a license
file if you decide to distribute or reuse it publicly.
