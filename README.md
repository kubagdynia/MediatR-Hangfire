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

### Design Overview

The communication between the API and the domain layer is divided into commands and queries. Commands are used to perform actions that change the state of the system, while queries are used to retrieve data from the system. The MediatR library is used to implement the mediator pattern, which allows the API to send commands and queries to the domain layer without having to know the details of how they are handled.

#### Invoice creation process:

When an attempt is made to create an invoice, a [command to create the invoice](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Api/Invoices/CreateInvoice.cs) is sent. The data [validation](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Core/Behaviors/ValidationBehavior.cs) process is implemented using a MediatR pipeline, with validation handled by the FluentValidation library. If [validation fails](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Invoices/Commands/Validators/CreateInvoiceCommandValidator.cs), a DomainException is thrown. This exception is then caught by the global exception handler ([GlobalExceptionHandler](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Core/Exceptions/GlobalExceptionHandler.cs)), which creates an appropriate error description and returns it to the API user.

If the validation succeeds, the appropriate [handler](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Invoices/Commands/Handlers/CreateInvoiceHandler.cs) processes the invoice creation request. The invoice is created and saved in the database, and an event signaling the invoice creation is emitted. If the Hangfire system is enabled ([configuration](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Api/appsettings.json)), this event is queued for background processing. There are two handlers attached to the invoice creation event:

- [SendEmailAboutInvoiceCreationEventHandler](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Invoices/Events/Handlers/SendEmailAboutInvoiceCreationEventHandler.cs) – responsible for sending an email notification about the invoice creation.

- [CreateInvoicePaymentRequestEventHandler](https://github.com/kubagdynia/MediatR-Hangfire/blob/main/MediatRTest.Invoices/Events/Handlers/CreateInvoicePaymentRequestEventHandler.cs) – initiates the payment request process for the invoice.

This model allows for separation of application logic and ensures scalability and ease of system expansion.

![Invoice creation process](.//docs/images/invoice-creation-process.svg)

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