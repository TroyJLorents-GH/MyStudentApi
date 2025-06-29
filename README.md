# Student Hiring System API

![Azure](https://img.shields.io/badge/hosted%20on-Azure_App_Service-blue)

## Live API (Swagger UI)

[https://api.troystaticsite.com/swagger/index.html](https://studenthiringsystem-ccf5djgdg9dpb6ek.westus-01.azurewebsites.net/swagger/index.html)

- Live Site: https://www.troystaticsite.com/
---

## About Me

I'm **Troy Lorents**, a full-stack engineer with 7+ years of experience designing and building secure, scalable applications. This ASP.NET Core Web API serves as the backend for the Student Hiring System, demonstrating my skills in API design, Entity Framework Core, SQL Server integration, and cloud deployment.

---

## Table of Contents

1. [Overview](#overview)
2. [Tech Stack](#tech-stack)
3. [Prerequisites](#prerequisites)
4. [Getting Started](#getting-started)

   * [Clone Repository](#clone-repository)
   * [Configure Environment](#configure-environment)
   * [Run Locally](#run-locally)
5. [API Documentation](#api-documentation)
6. [Features & Endpoints](#features--endpoints)
7. [Deployment](#deployment)
8. [CI/CD](#cicd)
9. [Dev Highlights](#dev-highlights)
10. [License](#license)

---

## Overview

This project implements a **RESTful Web API** using **ASP.NET Core** that powers the Student Hiring System. It exposes endpoints for:

* **Student Lookup**: search by ID or name (`tlorents` or `123456789`)
* **Class Assignment**: assign students to classes with auto-calculated compensation and cost center
* **Bulk Upload**: ingest batches of student hires
* **Dashboard**: provide hiring progress and administrative overview
* **Print Confirmation**: generate printable hiring statements

The API connects to a **MSSQL** database via **Entity Framework Core**, and is secured and configured to support CORS for a React.js frontend.

---

## Tech Stack

* **Framework**: .NET 8 (ASP.NET Core Web API)
* **ORM**: Entity Framework Core
* **Database**: Microsoft SQL Server
* **Documentation**: Swagger / OpenAPI
* **Hosting**: Azure App Service
* **CI/CD**: GitHub Actions
* **Tools**: Visual Studio, Git, PowerShell

---

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/sql-server) (local or Azure SQL)
* [Azure CLI](https://docs.microsoft.com/cli/azure) (for deployment)

---

## Getting Started

### Clone Repository

```bash
git clone https://github.com/TroyJLorents-GH/MyStudentApi.git
cd MyStudentApi
```

### Configure Environment

1. Copy **appsettings.json** to **appsettings.Development.json** (for local testing) or **appsettings.Production.json** (for live host).
2. Update the `ConnectionStrings:DefaultConnection` key with your SQL Server connection string.
3. (Optional) Set environment variables for sensitive settings:

   * `JWT__Key`
   * `AppSettings__MasterPassword`

### Run Locally

```bash
dotnet restore
dotnet build
dotnet run --launch-profile Development
```

Browse to `https://localhost:5001/swagger/index.html` to explore the API.

---

## API Documentation

Interactive documentation is available via Swagger at:

```
https://<your-domain>/swagger/index.html
```

It details all available endpoints, request/response models, and error codes.

---

## Features & Endpoints

| Method | Endpoint                       | Description                                |
| ------ | ------------------------------ | ------------------------------------------ |
| GET    | `/api/StudentLookp/{id}`       | Gets student by ID                         |
| GET    | `/api/StudentClassAssignment`           | Get all students added/hired                         |
| GET    | `/api/StudentClassAssignment/{id}` | Gets specific student on Master Dashboard            |
| POST   | `/api/StudentClassAssignment`             | Assign a student to a class with student/class details                |
| PUT   | `/api/StudentClassAssignment/{id}`              | Updates hiring record info           |
| GET    | `/api/StudentClassAssignment/totalhours/{studentid}` | Get remaining hours student can be hired for, max is 20, if already hired for 5 hours will list 15 hours remaining           |
| GET    | `/api/MasterIAGraderApplication`               | Retrieved Students who have filled out application         |
| GET    | `/api/Class/subject`          | Get list of all Class Subjects being taught for that term (CSE) |
| GET    | `/api/Class/catalog`          | Get list of catalog numbers (class numbers) based on class subject (CSE - 100) |
| GET    | `/api/Class/classnumbers`          | Get correspodning course number |
| GET    | `/api/Class/details/{classNum}`          | Get list class details needed for HR Use |

---

## Deployment

This API is deployed to **Azure App Service**. Configuration is automated through GitHub Actions:

1. On push to `main`, build and run tests.
2. Publish artifact via `dotnet publish`.
3. Deploy to Azure App Service using `azure/webapps-deploy@v2` action.

See [`.github/workflows/ci-cd.yml`](.github/workflows/ci-cd.yml) for details.

---

## CI/CD

* **Build**: `dotnet build` on every PR and push
* **Tests**: `dotnet test` (unit and integration)
* **Publish**: `dotnet publish --configuration Release`
* **Deploy**: Azure App Service deployment with zero-downtime swap

---

## Dev Highlights

* **Secure Secrets**: Managed via GitHub Actions Secrets and appsettings
* **CORS Configuration**: Allows cross-origin calls from our React frontend
* **Compensation Logic**: Hourly rate, doubled hours, pay periods, cost center mapping implemented in service layer
* **Logging**: Integrated with built-in `ILogger` for structured logs (Azure Monitor compatible)
* **Error Handling**: Global exception middleware for consistent error responses

---
### Created by Troy Lorents | @TroyJLorents-GH

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
