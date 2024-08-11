# DotKnotty Project Requirements Document (Lab Environment Version)

## 1. Summary

DotKnotty is a mock web application designed to simulate a space pirate ship repair service called "GalacticDock". The application allows space pirate captains to save their ship configurations, create repair tickets, and track the status of their repairs. This project serves two primary purposes:
1. To provide a realistic, functional web application that demonstrates common business processes in a simplified environment.
2. To serve as a platform for demonstrating various web application vulnerabilities in a .NET environment.

The application is intended for use in individual lab environments and is not designed for production use.

## 2. Business Requirements

### 2.1 Ship Configuration Management
- **User Story**: As a space pirate captain, I want to save my ship's configuration so that I can easily reference it when requesting repairs or upgrades.
  - Captains should be able to input their ship's name, weapons level, and shield level.
  - Captains should be able to save multiple configurations for the same ship or different ships.
  - Captains should be able to view a list of their saved configurations.
  - Captains should be able to load a previously saved configuration to view or modify it.

### 2.2 Repair Ticket System
- **User Story**: As a space pirate captain, I want to create a repair ticket for my ship so that I can request necessary repairs or upgrades.
  - Captains should be able to create a new repair ticket by providing their ship's name, a description of the needed repairs, and optionally selecting a saved ship configuration.
  - The system should automatically assign a unique ID to each new repair ticket.

- **User Story**: As a GalacticDock technician, I want to view and update repair tickets so that I can manage ongoing repairs.
  - Technicians should be able to view all repair tickets in the system.
  - Technicians should be able to update the status of a repair ticket (e.g., "Pending", "In Progress", "Completed").
  - Technicians should be able to add comments or notes to a repair ticket.

### 2.3 User Authentication
- **User Story**: As a user of the GalacticDock system, I want to have a simple account so that I can access my personal information and tickets.
  - The system should support basic user registration for both captains and technicians.
  - Users should be able to log in and log out of the system.
  - The system should differentiate between captain and technician roles, providing appropriate access and functionality for each.

### 2.4 Basic Reporting
- **User Story**: As a GalacticDock manager, I want to view simple reports on repair activities.
  - The system should provide a basic dashboard showing key metrics (e.g., number of open tickets, total tickets).
  - Managers should be able to view a list of all repair activities.

## 3. Technical Requirements

### 3.1 Platform and Framework
- The application must be built using ASP.NET Core (latest stable version).
- The application should follow the Razor Pages architectural pattern for simplicity.

### 3.2 Database
- The application should use SQLite as the database to store user information, ship configurations, and repair tickets.
- Entity Framework Core should be used for database operations.

### 3.3 User Interface
- The user interface should be responsive and mobile-friendly.
- The application should use Bootstrap for styling and layout.

### 3.4 Authentication
- The application should implement a simple, custom authentication system using the application's SQLite database for user accounts.
- Passwords should be securely hashed before storage.

### 3.5 Docker Containerization
- The entire application, including the SQLite database, should be able to run within a single Docker container.
- A Dockerfile should be provided for easy build and deployment in lab environments.

### 3.6 Security (for normal operation, not including intentional vulnerabilities)
- Basic input validation should be implemented to prevent common web vulnerabilities.
- HTTPS should be supported, but the application should also be able to run over HTTP for lab environments where HTTPS might not be available.
- Simple error handling should be implemented to aid in troubleshooting.

### 3.7 Compatibility
- The application should be compatible with the latest versions of major web browsers (Chrome, Firefox, Safari, Edge).

### 3.8 Simplicity and Portability
- The application should be designed for easy setup and teardown in lab environments.
- All necessary components should be self-contained within the application or its Docker container.
- The application should not rely on external services or databases that might not be available in isolated lab environments.

## 4. Planned Vulnerabilities for Demonstration

The following vulnerabilities will be intentionally implemented in the application for demonstration purposes. These vulnerabilities should not be present in a real-world, production application and are included here solely for educational purposes.

### 4.1 Insecure Deserialization

**Description**: The application will include a feature that uses unsafe deserialization of user-controllable data, potentially allowing arbitrary code execution.

**Implementation Area**: This vulnerability will be implemented in the Ship Configuration management feature, specifically in the process of loading saved ship configurations.

**Purpose**: To demonstrate how improper deserialization of user input can lead to severe security issues, including potential remote code execution.

### 4.2 Broken Access Control / Insecure Direct Object References (IDOR)

**Description**: The application will have endpoints that allow users to access or modify resources they should not have permission to, by manipulating object identifiers.

**Implementation Area**: This vulnerability will be present in the Repair Ticket System, allowing users to potentially view or modify tickets that don't belong to them.

**Purpose**: To showcase how improper authorization checks can lead to unauthorized access to sensitive data or functionality.

### 4.3 Mass Assignment

**Description**: The application will include forms or API endpoints that accept user input for model binding without proper restrictions, allowing users to set fields they shouldn't have access to.

**Implementation Area**: This vulnerability will be implemented in the process of creating or updating repair tickets, potentially allowing users to set fields like priority or assigned technician that should be restricted.

**Purpose**: To demonstrate how overly permissive data binding can lead to privilege escalation or data integrity issues.

## Note on Vulnerability Implementation

These vulnerabilities are to be carefully implemented in a controlled manner. They should be clearly marked in the code and documented to prevent accidental use in any production environment. The application should include prominent warnings about the presence of these vulnerabilities.

A separate document will be created with detailed walkthroughs on how to identify and exploit these vulnerabilities, as well as how to properly mitigate them in real-world applications.