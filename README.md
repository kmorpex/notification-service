# C# Notification Service

A C# .NET Notification Service designed with Clean Architecture principles, leveraging Domain-Driven Design (DDD) and CQRS.

## Overview

This project is a C# .NET Notification Service designed with Clean Architecture principles, leveraging Domain-Driven Design (DDD) for strategic design and modeling, and implementing CQRS for a clear separation of commands and queries. It includes robust validation mechanisms to ensure the integrity of incoming data and utilizes a background task queue for efficient processing of notification tasks without blocking the main execution thread.

## Table of Contents

- [Technologies](#technologies)
- [Architecture Overview](#architecture-overview)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Providers](#providers)
- [Configuration](#configuration)
- [License](#license)

## Technologies

- .NET 8
- MediatR
- FluentValidator

## Architecture Overview

The project follows the principles of Clean Architecture and Domain-Driven Design to ensure a separation of concerns and a focus on the core business logic. Key architectural layers include:

- **Domain Layer**: Contains the domain entities, aggregates, value objects, and domain services.
- **Application Layer**: Orchestrates the application's use cases by interacting with the domain layer. It's free from business logic but responsible for coordinating application behavior.
- **Infrastructure Layer**: Implements the details of external concerns such as databases, external services, and frameworks. It includes repositories, database access, and external service integrations.
- **Presentation Layer**: Represents API. It communicates with the application layer to execute use cases.

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later

## Usage

The Notification Service provides a RESTful API for sending notifications. The following endpoints are available:

- **POST /api/v1/email/send**: Sends an email notification.
- **POST /api/v1/sms/send**: Sends an SMS notification.

### Request Examples

#### Send Email

```json
{
  "to": [
    "test@test.com"
  ],
  "subject": "Test Email",
  "content": "This is a test email"
}
```

#### Send SMS

```json
{
  "phoneNumber": "+12007702024",
  "content": "This is a test SMS"
}
```

## Providers

The Notification Service supports multiple providers for sending notifications. The following providers are currently available:

- **Email**: Uses the Fake and SendGrid APIs to send email messages.
- **SMS**: Uses the Fake to send SMS messages.

Fake providers are used for testing and development purposes and print the notification content to the console instead of sending it to the actual recipient.

You can add new providers by implementing the `IEmailProvider` or `ISMSProvider` interface and registering the new provider in the dependency injection container.

## Configuration

The Notification Service can be configured using the `appsettings.json` file. You can specify the following settings:

- **EmailProvider**: The email provider to use (e.g., `Fake`, `SendGrid`).
- **SMSProvider**: The SMS provider to use (e.g., `Fake`).

## License
This project is licensed under the [MIT License](LICENSE). See the LICENSE file for details.