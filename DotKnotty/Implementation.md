# DotKnotty Implementation Details

## Overview
DotKnotty is a web application built using ASP.NET Core Razor Pages. It simulates a space pirate ship repair service called "GalacticDock", allowing users to manage ship configurations and create repair tickets.

## Technology Stack
- ASP.NET Core 6.0
- Entity Framework Core 6.0
- SQLite Database
- Razor Pages

## Data Model

### ApplicationUser
Extends the default IdentityUser class:
- Role: string (nullable)

### ShipConfiguration
- Id: int (Primary Key)
- UserId: string (Foreign Key to ApplicationUser)
- ShipName: string
- WeaponsLevel: int
- ShieldLevel: int

### RepairTicket
- Id: int (Primary Key)
- UserId: string (Foreign Key to ApplicationUser)
- Description: string
- Status: string
- CreatedAt: DateTime
- ShipConfigurationId: int (Foreign Key to ShipConfiguration)

## Main Pages and Forms

### Home Page (Index)
- Displays welcome message
- Links to Ship Configurations and Repair Tickets pages

### Ship Configurations
1. Index Page (`/ShipConfigurations/Index`)
   - Displays list of user's ship configurations
   - Form to create a new ship configuration

2. Create/Edit Ship Configuration
   - Fields: ShipName, WeaponsLevel, ShieldLevel

### Repair Tickets
1. Index Page (`/RepairTickets/Index`)
   - Displays list of user's repair tickets

2. Create Repair Ticket Page (`/RepairTickets/Create`)
   - Fields: ShipConfiguration (dropdown), Description

## Authentication and Authorization
- Uses ASP.NET Core Identity for user authentication
- Authorizes access to Ship Configurations and Repair Tickets pages

## Key Components

### ApplicationDbContext
- Located in `Data/ApplicationDbContext.cs`
- Extends IdentityDbContext<ApplicationUser>
- Defines DbSet properties for ShipConfigurations and RepairTickets

### Program.cs
- Configures services including database context, identity, and cookie authentication

### Razor Pages
- Each main feature (Ship Configurations, Repair Tickets) has its own folder in the Pages directory
- Each folder contains Index, Create, Edit, and Delete pages as needed

## Data Access
- Uses Entity Framework Core with SQLite
- Database migrations are used to manage schema changes

## Styling
- Uses Bootstrap for responsive design

## Project Structure
```
DotKnotty/
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── ShipConfiguration.cs
│   └── RepairTicket.cs
│
├── Pages/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _LoginPartial.cshtml
│   │
│   ├── ShipConfigurations/
│   │   ├── Index.cshtml
│   │   ├── Index.cshtml.cs
│   │   ├── Create.cshtml
│   │   ├── Create.cshtml.cs
│   │   ├── Edit.cshtml
│   │   ├── Edit.cshtml.cs
│   │   ├── Delete.cshtml
│   │   └── Delete.cshtml.cs
│   │
│   ├── RepairTickets/
│   │   ├── Index.cshtml
│   │   ├── Index.cshtml.cs
│   │   ├── Create.cshtml
│   │   ├── Create.cshtml.cs
│   │   ├── Edit.cshtml
│   │   ├── Edit.cshtml.cs
│   │   ├── Delete.cshtml
│   │   └── Delete.cshtml.cs
│   │
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Error.cshtml
│   ├── Index.cshtml
│   └── Privacy.cshtml
│
├── wwwroot/
│   ├── css/
│   │   └── site.css
│   ├── js/
│   │   └── site.js
│   └── lib/
│       └── [Third-party libraries like Bootstrap, jQuery]
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── DotKnotty.csproj
```

This document provides an overview of the current implementation of DotKnotty. Developers should refer to individual code files for more detailed implementation specifics.