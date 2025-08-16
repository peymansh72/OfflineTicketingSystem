# Internal Ticket Support System API

## 📖 Description

This is an ASP.NET Core 8 project that provides the backend API for an internal Ticket Support System. The application allows employees within an organization to create support tickets and track their status. Administrators are responsible for managing, assigning, and resolving these tickets to ensure smooth internal operations.

The API is designed to be self-contained, running locally with a file-based SQLite database, and includes secure, role-based access control.

## ✨ Features

-   **User Registration and Login**: Secure endpoints for user account creation and authentication.
-   **Role-based Authorization**: Three distinct roles (`Employee`, `Admin`) to control access to different API features.
-   **JWT Authentication**: Secure, stateless authentication using JSON Web Tokens (JWT).
-   **Full Ticket Management (CRUD)**: Endpoints to create, read, update, and delete tickets.
-   **Role-Specific Views**: Employees can only see their own tickets, while Admins have a global view.
-   **Ticket Statistics**: An admin-only endpoint to get a summary of ticket counts by status.
-   **Automatic Database Seeding**: An initial Admin account is created automatically the first time the application runs, enabling immediate access.

## 💻 Technologies

-   **Framework**: ASP.NET Core 8
-   **API Documentation**: Swashbuckle (Swagger) `6.6.2`
-   **Database**: SQLite (via `Microsoft.Data.Sqlite` & `Microsoft.EntityFrameworkCore.Sqlite` `9.0.8-preview.8`)
-   **ORM**: Entity Framework Core 8
-   **Authentication**: JWT Bearer Tokens (`Microsoft.AspNetCore.Authentication.JwtBearer` `8.0.0`)
-   **Password Hashing**: BCrypt.Net-Next `4.0.3`

## ⚙️ Setup

To run this project on your local machine, follow these steps.

### Prerequisites

[-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [An IDE like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/).or [JetBrains Rider](https://www.jetbrains.com/).

### Installation and Execution

1.  **Clone the Repository**
    Open your terminal or command prompt and clone the project.
```bash
    git clone <your-repository-url>
    cd <project-folder-name>
```
OR 

1.  **Download and Extract the Project**
    Download the project source code as a .zip file.
    Extract the contents of the ZIP file to a folder on your machine.
    Open your terminal (like PowerShell or Command Prompt) and navigate into the extracted project directory.

2.  **Restore Dependencies**
    This command downloads all the required NuGet packages defined in the project file.
```
    dotnet restore
```

3.  **Update the Database**
    The project uses Entity Framework Core migrations to manage the database schema. The `dotnet ef database update` command will create the `ticketing.db` SQLite file (if it doesn't exist) and set up all the necessary tables.
```
    dotnet ef database update
```
    *Note: If you run the project without this step, the application is also configured to apply migrations automatically on startup.*

4.  **Run the Application**
    This command builds and runs the project. The API will start listening for requests.
```
    dotnet run
```
    By default, the application will be accessible at `http://localhost:5000` or `https://localhost:5001` (check your console output for the exact URLs).

## 🚀 API Usage

### Initial Admin User

The first time you run the application, the database will be seeded with a default administrator account. Use these credentials to log in and start managing the system.

-   **Email**: `admin@bargheto.com`
-   **Password**: `PaswordStrong@001!`

### Authentication Flow

1.  Navigate to the Swagger UI at `https://localhost:<port>/swagger`.
2.  Use the `POST /api/auth/login` endpoint with the admin credentials to receive a JWT token.
3.  Click the **"Authorize"** button at the top of the Swagger page and paste the token in the format `Bearer <your_token>`.
4.  You can now access all the protected endpoints.

### Endpoints Summary

| Method   | Endpoint                | Description                                    | Authorization Required |
| :------- | :---------------------- | :--------------------------------------------- | :--------------------- |
| `POST`   | `/api/auth/register`    | Register a new user (defaults to `Employee`).  | Public                 |
| `POST`   | `/api/auth/login`       | Authenticate and receive a JWT token.          | Public                 |
| `POST`   | `/api/tickets`          | Create a new support ticket.                   | `Employee`             |
| `GET`    | `/api/tickets/my`       | Get tickets created by the current user.       | `Employee`             |
| `GET`    | `/api/tickets`          | Get a list of all tickets in the system.       | `Admin`                |
| `PUT`    | `/api/tickets/{id}`     | Update a ticket's status or assignment.        | `Admin`                |
| `DELETE` | `/api/tickets/{id}`     | Delete a ticket from the system.               | `Admin`                |
| `GET`    | `/api/tickets/stats`    | Get a summary of ticket counts by status.      | `Admin`                |
