# Click Tracker Application

Full-stack application with .NET backend, Angular frontend, and MSSQL database for tracking button clicks.

## Project Structure

- **ClickTrackerAPI** - .NET 8 Web API backend
- **ClickTrackerUI** - Angular 16 frontend

## Prerequisites

- .NET 8 SDK
- Node.js and npm
- SQL Server LocalDB (included with Visual Studio)

## Database Setup

The application uses SQL Server LocalDB with the connection string:
```
Server=(localdb)\\mssqllocaldb;Database=ClickTrackerDB;Trusted_Connection=true;TrustServerCertificate=true;
```

The database is automatically created when the backend starts.

## Running the Application

### Backend (.NET API)

1. Navigate to the backend folder:
   ```
   cd ClickTrackerAPI
   ```

2. Run the API:
   ```
   dotnet run
   ```

The API will start on `http://localhost:5000`

### Frontend (Angular)

1. Navigate to the frontend folder:
   ```
   cd ClickTrackerUI
   ```

2. Start the development server:
   ```
   ng serve
   ```

The app will be available at `http://localhost:4200`

## Features

- Click tracking button
- Real-time click counter
- All clicks saved to MSSQL database
- RESTful API endpoints

## API Endpoints

- `GET /api/clicks/count` - Get total click count
- `POST /api/clicks` - Record a new click
- `GET /api/clicks` - Get all click events
