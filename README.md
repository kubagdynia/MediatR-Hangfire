# MediatR-Hangfire

[![CI](https://img.shields.io/github/actions/workflow/status/kubagdynia/MediatR-Hangfire/dotnet.yml?branch=main)](https://github.com/kubagdynia/MediatR-Hangfire/actions?query=branch%3Amain)

An example of using the MediatR and Hangfire (background processing) libraries to create a simple invoicing application

## Introduction
This is a simple invoicing application that demonstrates how to use the [MediatR](https://github.com/jbogard/MediatR) and [Hangfire](https://www.hangfire.io/) libraries to create a simple invoicing application. The application is built using [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and uses the MediatR library to implement the mediator pattern. The application also uses the Hangfire library to implement background processing.

## Getting Started
To get started with the application, clone the repository to your local machine and run the application using the following steps:

- Clone the repository to your local machine using the following command:
  ```bash
  git clone https://github.com/kubagdynia/MediatR-Hangfire.git
  ```
- Navigate to the directory with the application's web api:
  ```bash
    cd ./MediatR-Hangfire/MediatRTest.Api/
  ```
- Run the application using the following command:
  ```bash
    dotnet run
  ```
  
- The application will start, and you can access it by navigating to the following URL in your browser:
  ```bash
      http://localhost:5190/swagger
  ```
  ![Hangfire Dashboard](.//docs/images/swagger.jpg)
  
- By default, you can also access the Hangfire dashboard by navigating to the following URL in your browser:
  ```bash
      http://localhost:5190/hangfire
  ```
  ![Hangfire Dashboard](.//docs/images/hangfire-dashboard.jpg)

### Project structure

The project is structured as follows:

- MediatRTest.Api - Web API project
- MediatRTest.Api.Tests - Integration tests for the Web API project
- MediatRTest.Core - Core project containing the core logic of the application
- MediatRTest.Data - Data access project containing the database context and data access logic
- MediatRTest.Invoices - Project containing the invoice processing logic (domain logic)
- MediatRTest.Invoices.Tests - Unit tests for the invoice processing logic

### Technologies

List of technologies, frameworks and libraries used for implementation:

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MediatR](https://github.com/jbogard/MediatR) - Mediator pattern implementation
- [FluentValidation](https://docs.fluentvalidation.net) - Validation library
- [Hangfire](https://www.hangfire.io/) - Background processing
- [EF Core 8.0](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew) - Entity Framework Core 8.0
- [SQLite](https://www.sqlite.org/index.html) - Database
- [NUnit](https://nunit.org/) - Unit testing framework


### License
This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).