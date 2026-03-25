# LabTrack

A web application for monitoring and managing robotic lab automation runs, built with ASP.NET Core MVC, MySQL, MongoDB, and Vue.js.

## Overview

LabTrack simulates the kind of internal tooling used in lab automation environments. Lab technicians can create and manage runs, track their live status, and review historical sensor readings logged during each run.

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core MVC, C# |
| ORM | Entity Framework Core 9 |
| Primary Database | MySQL 8 |
| Sensor Logs | MongoDB 7 |
| Frontend | Razor Views, Vue.js 3, jQuery |
| Containerisation | Docker, docker-compose |
| CI | GitHub Actions |

## Architecture

The solution follows a clean three-layer architecture:

- **LabTrack.Core** — domain models and repository interfaces
- **LabTrack.Infrastructure** — EF Core + MySQL and MongoDB repository implementations
- **LabTrack.Web** — ASP.NET Core MVC controllers and Razor views

This separation ensures the core business logic has no dependency on any specific database or framework.

## Features

- Create, view, edit and delete lab runs
- Update run status (Pending → Running → Completed / Failed)
- Log sensor readings (temperature, pressure, RPM etc.) per run via MongoDB
- Live sensor log panel powered by Vue.js with Axios polling
- Colour coded status badges

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run locally

1. Clone the repo
```bash
   git clone https://github.com/Bardawil95/LabTrack.git
   cd LabTrack
```

2. Start the databases
```bash
   docker-compose up -d
```

3. Apply database migrations
```bash
   dotnet ef database update --project LabTrack.Infrastructure --startup-project LabTrack.Web
```

4. Run the app
```bash
   dotnet run --project LabTrack.Web
```

5. Open your browser at `https://localhost:5001`

## Database Design

**MySQL — structured lab run data**

| Column | Type | Description |
|---|---|---|
| Id | INT | Primary key |
| Name | VARCHAR(200) | Run name |
| Description | VARCHAR(1000) | Run description |
| Status | VARCHAR(50) | Pending / Running / Completed / Failed |
| CreatedAt | DATETIME | Creation timestamp |
| StartedAt | DATETIME | When the run started (nullable) |
| CompletedAt | DATETIME | When the run finished (nullable) |
| CreatedBy | VARCHAR(100) | Author |

**MongoDB — timeseries sensor logs**

| Field | Type | Description |
|---|---|---|
| _id | ObjectId | Auto-generated |
| LabRunId | INT | Links back to MySQL LabRun |
| Timestamp | DATETIME | Reading timestamp |
| SensorName | STRING | e.g. Temperature, Pressure |
| Value | DOUBLE | Reading value |
| Unit | STRING | e.g. °C, bar, rpm |