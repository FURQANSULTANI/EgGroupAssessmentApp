# EG Group Assessment Application

This repository contains a **full-stack web application** developed as part of the **EG Group Technical Assessment**.  
The project demonstrates clean architecture, secure authentication, role-based authorization, and a simple, responsive user interface.

---

## Project Overview

The application is split into two main parts:

- **Backend:** ASP.NET Core Web API with Entity Framework Core and SQL Server
- **Frontend:** React (TypeScript) application for user authentication and dashboard access

Both parts are maintained within a **single repository** for easier review and setup.

---

## Repository Structure

EgGroupAssessmentApp
│
├── backend
│ ├── EgGroupAssessmentApp.API
│ ├── EgGroupAssessmentApp.Core
│ ├── EgGroupAssessmentApp.Infrastructure
│ ├── EgGroupAssessmentApp.Services
│ └── README.md
│
├── frontend
│ └── eg-group-assessment-ui
│ └── README.md
│
└── README.md

## Technology Stack

### Backend
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Clean Layered Architecture
- Swagger / OpenAPI

### Frontend
- React
- TypeScript
- JWT-based authentication
- Role-based UI rendering
- Responsive design

---

## Key Features

- User authentication using JWT
- Role-based authorization (Admin / User)
- Secure API endpoints
- CRUD operations for user management
- Clean separation of concerns
- Global error handling
- Responsive UI

---

## Setup Instructions

> Detailed setup steps are provided in the individual README files.

---
*Developed as part of the EG Group Technical Assessment.*