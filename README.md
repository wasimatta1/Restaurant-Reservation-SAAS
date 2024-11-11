# Restaurant Reservation System API

## Overview

The **Restaurant Reservation System API** provides a comprehensive set of RESTful API endpoints designed to manage restaurant reservations, orders, employees, menu items, and other related entities. Developed using **ASP.NET Core Web API** and **Entity Framework Core**, the system leverages a **SQL Server** database for persistence. This API enables restaurants to handle reservations, manage employees, track customer orders, and perform complex queries using database views, stored procedures, and custom functions.

## Features

### Reservation Management
- **Create, Update, Delete, and Retrieve Reservations**: Full support for managing reservations, including party size, reservation time, and associated restaurant and table details.
- **Retrieve Reservations by Customer**: Endpoint to retrieve all reservations made by a particular customer, allowing for customer-centric data management.
- **List Orders and Menu Items for a Reservation**: Retrieve detailed orders placed during a reservation, including the associated menu items.
- **Get Ordered Menu Items**: Fetch distinct menu items ordered in a specific reservation, enabling precise tracking of customer preferences.

### Employee Management
- **List Managers**: Retrieve all employees holding the position of **Manager**, facilitating the management of restaurant staff.
- **Calculate Average Order Amount for Employees**: Computes the average order value for a given employee, useful for performance analysis and employee metrics.

### Restaurant and Table Management
- **Restaurant CRUD Operations**: Fully functional Create, Read, Update, and Delete (CRUD) operations for managing restaurant details (name, location, contact information, etc.).
- **Table Management**: Manage tables within a restaurant, including CRUD operations for table availability, seating capacity, and associations with reservations.

### Database Views & Stored Procedures
- **Database Views**: Simplify complex queries, such as fetching reservations along with customer and restaurant information, using pre-defined views.
- **Stored Procedures**: Provides support for executing stored procedures to retrieve data efficiently. Notably, includes a stored procedure for finding customers who have made reservations with a party size above a specified threshold.

### API Endpoints

#### **Reservation Endpoints**
- **`GET /api/reservations`**: Retrieve all reservations with pagination.
- **`GET /api/reservations/{id}`**: Get the details of a specific reservation by ID.
- **`POST /api/reservations`**: Create a new reservation.
- **`PUT /api/reservations`**: Update an existing reservation.
- **`DELETE /api/reservations/{id}`**: Delete a reservation by ID.
- **`GET /api/reservations/customer/{customerId}`**: Retrieve all reservations made by a specific customer.
- **`GET /api/reservations/{reservationId}/orders`**: Get the orders associated with a specific reservation.
- **`GET /api/reservations/{reservationId}/menu-items`**: Get the distinct menu items ordered during a reservation.

#### **Employee Endpoints**
- **`GET /api/employees/managers`**: Retrieve a list of all employees with the position of **Manager**.
- **`GET /api/employees/{employeeId}/average-order-amount`**: Calculate the average order amount associated with a specific employee.

#### **Restaurant Endpoints**
- **`GET /api/restaurants`**: Retrieve all restaurants.
- **`GET /api/restaurants/{id}`**: Get details of a specific restaurant.
- **`POST /api/restaurants`**: Create a new restaurant record.
- **`PUT /api/restaurants`**: Update the details of an existing restaurant.
- **`DELETE /api/restaurants/{id}`**: Delete a restaurant by its ID.

#### **Table Endpoints**
- **`GET /api/tables`**: List all tables available in the restaurant, optionally filtered by criteria such as seating capacity.
- **`GET /api/tables/{id}`**: Get details of a specific table by its ID.
- **`POST /api/tables`**: Create a new table within the restaurant.
- **`PUT /api/tables`**: Update the details of a table.
- **`DELETE /api/tables/{id}`**: Delete a table by its ID.

## Authorization & Security

The API employs **JWT (JSON Web Tokens)** for secure authorization. Each request to restricted endpoints requires a valid JWT token, ensuring that sensitive data such as restaurant management and employee information is only accessible to authorized users. The security layer is implemented at the controller level, ensuring that only users with appropriate roles (e.g., admins, restaurant owners) can access or modify data.

### Authorization Flow
- Upon successful login, a JWT token is issued.
- The token is included in the `Authorization` header for subsequent requests.
- The API validates the token and grants access to endpoints based on the user's role.

## Validation and Error Handling

- **Input Validation**: Every request is validated to ensure that the data provided is accurate and complete. Invalid input is handled gracefully with detailed error messages.
- **Error Responses**: The API responds with **standard HTTP status codes** (e.g., `400 Bad Request`, `404 Not Found`, `500 Internal Server Error`) and includes an informative message body for each error scenario.

## API Documentation

The API utilizes **Swagger** for auto-generating API documentation. The Swagger UI is integrated directly into the application, providing an interactive interface where developers can explore all available endpoints, view parameter definitions, expected responses, and test API calls directly from the documentation interface.

### Key Benefits of Swagger Integration:
- Automatically generated API documentation from code annotations.
- Interactive testing of API endpoints without the need for external tools like Postman.
- Clear explanations of request parameters, response models, and error handling.

## Database Architecture

The underlying database architecture supports the following entities:

- **Customers**: Stores customer information (first name, last name, email, phone number).
- **Reservations**: Contains reservation details such as the customer, reservation date, party size, and associated table.
- **Orders**: Tracks orders placed by customers, linked to reservations and employees.
- **MenuItems**: Contains details of items available on the restaurant’s menu.
- **Employees**: Stores employee details including their role (e.g., Manager) and associated restaurant.
- **Restaurants**: Holds information about the restaurant, such as name, address, and operational hours.
- **Tables**: Represents tables within a restaurant, including seating capacity and availability.

The database schema has been designed with **Entity Framework Core**, leveraging **migrations** to keep track of database changes. Migrations are included in the project to create the necessary tables and relationships, and the database is seeded with sample data to ensure the system is ready for testing and development.

## Database Views & Stored Procedures

- **Database Views**: The system utilizes views to simplify complex queries, such as retrieving all reservation data along with customer and restaurant information. This reduces the need for complex joins in the code and improves query performance.
- **Stored Procedures**: Several stored procedures are used for advanced queries, such as fetching customers who have made reservations with a party size above a certain threshold.

## Testing & Quality Assurance

All endpoints have been manually tested using tools like **Postman** and **Swagger UI** to verify their functionality. Additionally, a comprehensive **Postman Test Suite** is included in the project to automate the testing process and ensure all endpoints work correctly.

The test suite includes tests for:
- **Successful operations** (e.g., creating a reservation, retrieving restaurant data).
- **Error scenarios** (e.g., invalid input, unauthorized access).
- **Edge cases** (e.g., empty reservations, exceeding page size limits).

## Conclusion

The **Restaurant Reservation System API** provides a powerful, scalable solution for managing restaurant reservations, employees, orders, and menu items. The system’s use of modern web technologies such as **ASP.NET Core**, **Entity Framework Core**, and **SQL Server**, along with features like JWT authentication, error handling, and database views, ensures a robust and efficient backend for restaurant management applications.

The API is designed to be easy to extend and integrate with other systems, making it ideal for developers looking to build or extend restaurant reservation solutions.
