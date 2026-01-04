# Eg Group Assessment - User Interface

This repository contains the frontend application for the Eg Group Assessment project. It is built using React and TypeScript, providing a modern, type-safe, and responsive user interface for the authentication and dashboard system.

## Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) (v18 or higher recommended)
- [npm](https://www.npmjs.com/) (comes with Node.js)
- The backend API should be running (default: `https://localhost:44341`)

### Installation

1.  **Clone the repository**
    ```bash
    git clone [repository-url]
    cd eg-group-assessment-ui
    ```

2.  **Install dependencies**
    ```bash
    npm install
    ```

3.  **Configure Environment Variables**
    Copy the `.env.example` file to `.env` and update the `REACT_APP_API_BASE_URL` if necessary:
    ```bash
    cp .env.example .env
    ```

4.  **Run the application**
    ```bash
    npm start
    ```
    The application will open in your default browser at `http://localhost:3000`.

## Design Overview

This application follows a standard React project structure with a focus on type safety and maintainability.

- **Technology:** React, TypeScript, Axios, and React Router.
- **Architecture:** A dedicated service layer handles all API communication, keeping the UI components clean and focused on presentation.
- **Auth Implementation:** JWT-based authentication with role-based routing (Admin/User).
- **Styling:** Custom CSS with a focus on responsiveness and modern aesthetics.


---
*Developed as part of the EG Group Technical Assessment.*
