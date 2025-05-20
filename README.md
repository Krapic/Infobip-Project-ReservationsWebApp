# Infobip Project - Reservations Web Application

This repository contains a web application for managing reservations, developed as part of a project for Infobip.

## Technologies
- **ASP.NET Core** - Backend application
- **Entity Framework Core** - Database operations
- **Fluent UI** - Frontend components for user interface
- **React** - Frontend framework for UI development
- **SQLite** - Database used

## Features
- Create, edit, and delete reservations
- View all reservations
- Authentication and authorization implementation
- Reservation filtering and searching

## Project Setup

### Prerequisites
- .NET SDK 6.0 or newer
- Node.js and npm

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Krapic/Infobip-Projekt-RezervacijeWebApp.git
   ```
2. Navigate to the backend directory and run the application:
   ```bash
   cd backend
   dotnet restore
   dotnet run
   ```
3. Navigate to the frontend directory and run the application:
   ```bash
   cd frontend
   npm install
   npm start
   ```

### Database Migrations
To apply database migrations, use the following commands:

1. Generate migration:
   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. Update the database:
   ```bash
   dotnet ef database update
   ```

## Configuration
The application uses SQLite by default but can be configured to use other databases (e.g., SQL Server). Configuration is located in the `appsettings.json` file.

## Contributions
Feel free to submit PRs for any suggestions or improvements.

## License
This project is licensed under the MIT License.
