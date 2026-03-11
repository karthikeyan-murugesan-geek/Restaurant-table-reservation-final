# Restaurant Table Reservation System

## Project Overview
This project is a simplified **Restaurant Table Reservation System** built using **ASP.NET Core Web API** with a **microservices architecture**.  

The system contains two services:
- **Customer Service** – Handles user registration, login, and authentication using JWT.
- **Reservation Service** – Manages restaurant table reservations.

Both services use **Entity Framework Core with an in-memory database**.

---

## Microservices

### Customer Service
Handles customer accounts and authentication.

Features:
- User registration
- User login
- JWT token generation
- Role management (Customer / Manager)

User Fields:
- Id
- Username
- PasswordHash
- Role

---

### Reservation Service
Handles reservation management.

Features:
- Create reservation
- View reservations
- Update reservation
- Delete reservation
- Search reservations by date and time slot
- Validate guest count with table capacity
- Prevent duplicate reservations for the same table and time slot

Access Rules:
- Authenticated users can access reservation endpoints
- Customers can view only their reservations
- Managers can view all reservations and update reservation status

---

## Authentication & Authorization
- Uses **JWT authentication**
- CustomerService generates JWT tokens after successful login
- ReservationService requires valid JWT tokens
- Role-based access control is implemented

---

## Technologies Used
- ASP.NET Core Web API
- C#
- Entity Framework Core
- In-Memory Database
- JWT Authentication

---

## How to Run the Project

1. Clone the repository


2. Open the solution in **Visual Studio**

3. Run both services:
- CustomerService
- ReservationService

4. Use **Postman or Swagger** to test API endpoints.

---

## API Testing
You can test APIs using:
- Swagger UI
- Postman

First register a user and login to receive a **JWT token**, then use the token to access reservation endpoints.

---

## Author
Karthikeyan Murugesan