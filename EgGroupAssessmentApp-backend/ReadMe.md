# Eg Group Assessment App - Backend

Welcome to the backend component of the Eg Group Assessment Application. This project is a robust, scalable, and secure Web API built with modern .NET technologies to support user management and authentication features.

---

## Technologies Used

- **Framework:** .NET 8.0 Web API
- **Database:** SQL Server
- **ORM:** Entity Framework Core (EF Core)
- **Authentication:** JWT (JSON Web Tokens) with Bearer Scheme
- **Documentation:** Swagger / OpenAPI
- **Architecture:** Clean Layered Architecture

---

## Architecture & Design Choices

The project follows a **Clean Layered Architecture** approach to ensure maintainability, testability, and separation of concerns.

### 1. Project Structure
- **Core:** Contains Domain Models, Interface definitions, and DTOs (Data Transfer Objects). This is the "heart" of the application and has no dependencies on other layers.
- **Infrastructure:** Handles data persistence, including `DbContext` and Repository implementations.
- **Services:** Contains the business logic of the application. Services interact with repositories and are consumed by the API layer.
- **API (EgGroupAssessmentApp):** The entry point of the application. Handles HTTP requests, authentication middleware, and API documentation.

### 2. Key Design Patterns
- **Repository Pattern:** Decouples the business logic from the data access layer, making the system easier to test and allowing for easier database swaps if needed.
- **Service Layer:** Encapsulates business logic, ensuring that controllers remain thin and focused only on handling request/response flow.
- **Dependency Injection (DI):** Heavily utilized for better decoupling and easier unit testing.

### 3. Security & Validation
- **JWT Authentication:** Implemented to provide stateless, secure communication between the frontend and backend.
- **Global Error Handling:** A centralized exception handling middleware is implemented to ensure consistent error responses across all endpoints.
- **CORS:** Configured to allow seamless connection with common frontend development ports (3000, 5173).

### 4. Design Decisions & Rationale

**Why JWT Authentication?**  
JWT (JSON Web Tokens) was chosen over traditional session-based authentication for several key reasons: it enables stateless authentication, making the API scalable and suitable for distributed systems; tokens are self-contained with user claims, reducing database lookups; and it provides seamless integration with modern frontend frameworks. This approach aligns with RESTful API best practices and supports future microservices architecture if needed.

**Why Clean Layered Architecture?**  
The layered architecture ensures clear separation of concerns, making the codebase maintainable and testable. Each layer has a specific responsibility, preventing tight coupling and allowing independent evolution of business logic, data access, and API layers.

**Why Repository Pattern?**  
The Repository Pattern abstracts data access logic, making it easier to swap databases or mock data sources during testing. This design choice enhances testability and maintains flexibility for future infrastructure changes.

---

## Setup Instructions

Follow these steps to get the backend running on your local machine.

### Prerequisites
- **.NET 8.0 SDK** (Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0))
- **SQL Server** (Express or LocalDB)
- **IDE:** Visual Studio 2022 or VS Code

### 1. Database Configuration
Update the connection string in `EgGroupAssessmentApp/appsettings.json, appsettings.Development.json` to point to your local SQL Server instance:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EgGroupAssessmentDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Apply Migrations
Open your terminal or Package Manager Console and run the following command to create the database and tables:

```bash
# Using .NET CLI
dotnet ef database update --project EgGroupAssessmentApp.Infrastructure --startup-project EgGroupAssessmentApp

# OR within Visual Studio Package Manager Console Ensure Project should be EgGroupAssessmentApp.Infrastructure
Update-Database
```

### 3. Run the Application
Start the API using your IDE's run button or the command line:

```bash
cd EgGroupAssessmentApp
dotnet run
```

### 4. Access API Documentation
Once the application is running, you can explore the API endpoints using Swagger UI at:
- **`https://localhost:7198/swagger`** (Note: Port may vary based on your local configuration)

---

## API Endpoints Summary

### Authentication
- **POST /api/Auth/login:** Authenticate users and receive a JWT token.

### User Management
- **GET /api/Users:** Retrieve all user profiles (Admin only).
- **POST /api/Users:** Create a new user (Admin only).
- **GET /api/Users/{id}:** Get specific user details (Self or Admin).
- **PUT /api/Users/{id}:** Update user details (Self or Admin).
- **DELETE /api/Users/{id}:** Delete a user (Admin only).

### Dashboard
- **GET /api/Dashboard:** Access general user dashboard.
- **GET /api/Dashboard/admin:** Access administrative dashboard (Admin only).

---
*Developed as part of the EG Group Technical Assessment.*
