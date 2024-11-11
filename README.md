
# Restaurant Reservation System

## Overview
This project is a **Restaurant Reservation System** designed to manage reservations, orders, employees, and menu items for restaurants. The system includes functionality for creating and managing reservations, handling customer details, processing orders, associating menu items with orders, and managing employee roles.

It leverages **Entity Framework Core** for database interaction, **SQL Server** as the backend database, and **ASP.NET Core** for the application. The system also supports **stored procedures**, **database views**, and **custom functions** for advanced querying and operations.

---

## Features
- **Reservation Management**: Create and manage reservations for customers with party size, table association, and reservation date.
- **Order Management**: Manage orders placed for reservations, including items ordered and quantities.
- **Employee Management**: Manage employee details, including roles (e.g., managers), and assign employees to restaurants.
- **Menu Item Management**: Track items available on the restaurant menu.
- **Stored Procedures**: Execute stored procedures to query and update database information.
- **Database Views**: Use database views to simplify complex data retrieval such as customer reservations and associated restaurant details.
- **JWT Authorization**: Secure the APIs using JSON Web Tokens (JWT) for authentication and authorization.
- **API Documentation**: Auto-generate API documentation with Swagger.

---

## Database Schema

The database schema consists of the following tables:
- **Customers**: Stores customer details such as first name, last name, email, and phone number.
- **Reservations**: Represents reservations made by customers, including party size, reservation date, and associated table and restaurant.
- **Orders**: Holds order information linked to specific reservations and employees.
- **OrderItems**: Contains the items included in an order and their quantities.
- **Employees**: Stores employee data, including first name, last name, position, and the restaurant they work for.
- **Restaurants**: Stores restaurant details such as name, address, phone number, and opening hours.
- **Tables**: Represents tables available at a restaurant with seating capacity.
- **MenuItems**: Contains information about items on a restaurant's menu.

---

## Setup Instructions

### 1. Clone the Repository
Clone the project repository to your local machine:

```bash
git clone https://github.com/yourusername/restaurant-reservation-system.git
cd restaurant-reservation-system
```

### 2. Create the Database
- Open **SQL Server Management Studio (SSMS)**.
- Create a new database named `RestaurantReservationCore`.
- Use the migration commands below to create the schema for the database.

### 3. Project Setup

#### 3.1 Create a Console Application
- Create a **Console Application** project named `RestaurantReservation`.
  
#### 3.2 Create a Library Project
- Create a **Library Project** named `RestaurantReservation.Db` and add a reference to it from the `RestaurantReservation` project.

#### 3.3 Create DbContext and Models
- Add the `RestaurantReservationDbContext` class to the `RestaurantReservation.Db` project.
- Create data models for the entities (Customers, Reservations, Employees, etc.) in the `RestaurantReservation.Db` project, including necessary relationships, keys, and constraints.
- Write migrations to create the tables and seed the database with at least 5 records per table.

### 4. Create Web API Project

#### 4.1 Create a Web API Project
Create a new **ASP.NET Core Web API** project named `RestaurantReservation.API`.

#### 4.2 Add Dependency References
Add a reference to the `RestaurantReservation.Db` project in the `RestaurantReservation.API` project to interact with the database.

#### 4.3 Implement CRUD Operations
- Create API controllers for each entity (e.g., Customers, Reservations, Employees).
- Implement standard **CRUD operations** (Create, Read, Update, Delete) for each entity.

#### 4.4 Implement Reservation-Specific Endpoints
Create additional endpoints specific to reservations:

- `GET /api/employees/managers`: List all employees who hold the position of "Manager."
- `GET /api/reservations/customer/{customerId}`: Retrieve all reservations by a customer ID.
- `GET /api/reservations/{reservationId}/orders`: List orders and menu items for a reservation.
- `GET /api/reservations/{reservationId}/menu-items`: List ordered menu items for a reservation.
- `GET /api/employees/{employeeId}/average-order-amount`: Calculate average order amount for an employee.

#### 4.5 Implement Authorization
Secure your API endpoints using **JWT authentication** or another preferred authorization mechanism.

#### 4.6 Input Validation and Error Handling
Implement **input validation** for incoming requests and provide **user-friendly error messages** for common issues.

#### 4.7 API Documentation with Swagger
Integrate **Swagger** to automatically generate API documentation. Ensure that the documentation includes details such as:
- API routes
- Query parameters
- Expected responses
- Error codes

---

## Database Operations

### 1. Views
- Use **Entity Framework Core** to query a database view that lists all the reservations, along with customer and restaurant details.
- Write a query to retrieve employees along with their respective restaurant details using a view.

### 2. Database Functions
- Create a database function to calculate the total revenue generated by a specific restaurant.
- Implement the function call in the `RestaurantReservation.Db` project, making it accessible from the application.

### 3. Stored Procedures
- Design a **stored procedure** to find all customers who have made reservations with a party size greater than a certain value.
- Write a method in the `RestaurantReservation.Db` project to execute the stored procedure.

### 4. Repository Pattern
- Create a repository class for each entity, e.g., `CustomerRepository.cs`, `ReservationRepository.cs`.
- Move related methods (e.g., `ListManagers`, `GetReservationsByCustomer`, etc.) to the appropriate repositories, adhering to the repository pattern.
- Ensure repositories are located in the `Repositories` folder inside the `RestaurantReservation.Db` project.

---

## Testing and Bonus

### 1. Manual Testing
Test all API endpoints manually using **Postman** or **Swagger UI** to ensure they function correctly.

### 2. Bonus: Postman Test Suite
- Write a comprehensive **Postman test suite** that includes tests for:
  - Successful API operations
  - Error handling for invalid inputs and other edge cases
- Share the Postman collection file with your team or upload it to the repository.

---

## Git Workflow and Version Control

1. **Follow GIT Best Practices**:
   - Commit early and often.
   - Use meaningful commit messages.
   - Push changes to GitHub regularly.

2. **Branching**:
   - Use feature branches for new tasks.
   - Merge changes to the main branch after review.

3. **Version Control**:
   - Host the project on GitHub and ensure that all code changes are committed and pushed regularly.

---

## Conclusion
This project provides a comprehensive solution for restaurant reservation management, incorporating best practices such as using the **Repository pattern**, **Entity Framework Core migrations**, **API documentation** with **Swagger**, and secure **JWT authentication**. Follow the instructions step by step to ensure all components are set up correctly.
