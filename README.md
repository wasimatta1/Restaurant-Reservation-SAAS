# Restaurant Reservation System

## Overview
This project is a **Restaurant Reservation System** designed to handle reservations, orders, employees, and menu items for restaurants. It includes functionality for creating reservations, managing customers, handling orders, and associating menu items with orders.

The system uses **Entity Framework Core** for database interaction, **SQL Server** as the backend database, and **ASP.NET Core** for the application. It supports stored procedures, database views, and custom functions for advanced querying and operations.

### Database Schema

The database schema consists of the following tables:
- **Customers**: Stores customer details such as first name, last name, email, and phone number.
- **Reservations**: Represents reservations made by customers, including party size, reservation date, and the associated table and restaurant.
- **Orders**: Holds order information linked to a specific reservation and employee.
- **OrderItems**: Contains the items included in an order and their quantities.
- **Employees**: Stores employee data, including first name, last name, position, and the restaurant they work for.
- **Restaurants**: Stores restaurant details such as name, address, phone number, and opening hours.
- **Tables**: Represents tables available at a restaurant with their seating capacity.
- **MenuItems**: Contains information about items on a restaurant's menu.


### Features

- **Reservation Management**: Create and manage reservations for customers with a specified party size.
- **Order Management**: Handle orders placed for reservations, including items ordered and quantities.
- **Employee Management**: Manage employee details and assign them to restaurants.
- **Stored Procedures**: Execute stored procedures for advanced operations like retrieving customers based on party size.
- **Database Views**: Use database views to simplify querying of complex data such as reservations with associated customer and restaurant information.

### Setup Instructions

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/restaurant-reservation-system.git
   cd restaurant-reservation-system
