# MedCare Project

## Table of Contents

- [Introduction](#introduction)
- [Project Structure](#project-structure)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Installation](#installation)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Introduction

**MedCare** is a comprehensive health management system designed to assist users in managing their medications, booking doctor appointments, receiving notifications, and predicting potential diagnoses based on symptoms. The platform consists of a web API, mobile client, and web administration panel, ensuring seamless integration across various devices.

## Project Structure

The MedCare project is organized into four main components:

1. **Web API**:

   - Built with ASP.NET and FastAPI.
   - Implements Microservices architecture for scalability and efficiency.
   - Provides endpoints for managing medications, appointments, notifications, and diagnosis predictions.

2. **Mobile Client**:

   - Developed using .NET MAUI.
   - Allows users to manage their health on-the-go, including medication reminders and appointment bookings.

3. **Web Administration Panel**:

   - Built with React.
   - Enables healthcare professionals to manage patient information, issue documents, and provide feedback.

4. **Diagnosis Predictor**:
   - Integrated into the mobile client.
   - Users can input symptoms and receive potential diagnoses based on AI/ML models.

## Features

- **Medication Management**:

  - Add, edit, and delete medications.
  - Schedule reminders for taking medications.
  - Receive notifications when it’s time to take medications.

- **Appointment Booking**:

  - Schedule appointments with doctors.
  - View and manage upcoming and past appointments.
  - Doctors can issue documents and add comments post-appointment.

- **Diagnosis Predictor**:

  - Input symptoms and receive a prediction of possible diagnoses.
  - Access a history of past diagnoses and related medical information.

- **Web Administration**:
  - Manage patient records.
  - View, edit, and delete appointments and related documents.
  - Interact with patient information securely and efficiently.

## Tech Stack

- **Web API**:

  - **ASP.NET**: For robust, scalable, and secure APIs using Microservices architecture.
  - **FastAPI**: For fast and easy-to-use RESTful APIs.

- **Mobile Client**:

  - **.NET MAUI**: For cross-platform mobile application development.

- **Web Administration Panel**:

  - **React**: For a dynamic, responsive, and user-friendly admin interface.

- **Database**:

  - MSSQL databases for storing user data, medications, appointments, and other records.

- **Notifications**:

  - Push notifications implemented using third-party services like Firebase Cloud Messaging (FCM).

- **AI/ML**:
  - Integrated diagnosis predictor using machine learning models.

## Installation

### Prerequisites

- **.NET SDK** (version 8 or higher)
- **Node.js** (version 22.16 or higher)
- **Python** (version 3.12 or higher)
- **Database** (MSSQL)

### Clone the Repository

```bash
git clone https://github.com/MarioCSharp/Health-System
```

### Web API Setup

1. Navigate to the ASP.NET API directory and restore dependencies:

   ```bash
   cd HealthSystemApi
   dotnet restore
   ```

2. Configure the database connection in `appsettings.json`.

3. Run the API:

   ```bash
   dotnet run
   ```

4. Navigate to the FastAPI directory and set up a virtual environment:

   ```bash
   cd MachineLearningApi
   ```

5. Run the FastAPI server:

   ```bash
   uvicorn main:app --reload
   ```

### Mobile Client Setup

1. Navigate to the .NET MAUI directory:

   ```bash
   cd MobileClient
   dotnet restore
   ```

2. Run the mobile application:

   ```bash
   dotnet build
   dotnet run
   ```

### Web Administration Setup

1. Navigate to the React directory:

   ```bash
   cd WebAdministration
   npm install
   ```

2. Run the React application:

   ```bash
   npm run dev
   ```

### Database Setup

1. Create and configure the database according to the settings in `appsettings.json`.
2. Run any necessary migrations to set up the schema.

## Usage

1. **Mobile Client**: Users can log in, manage their medications, book appointments, use the diagnosis predictor and other fuctionalities.
2. **Web Administration Panel**: Healthcare professionals can log in to manage patient data and appointments.
3. **API**: Developers can use the provided endpoints to interact programmatically with the MedCare system.

## API Documentation

API documentation is available via Swagger for the ASP.NET API and via automatically generated docs in FastAPI.

- **ASP.NET API**: Different port for every microservice
- **FastAPI**: Access `https://localhost:8080/docs` for documentation.

### Reporting Issues

If you find any bugs or have feature requests, please open an issue on our [GitHub Issue Tracker](https://github.com/MarioCSharp/Health-System/issues).

### Pull Requests

If you wish to submit a pull request, please ensure that your changes are well-tested and that you adhere to our coding standards.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any questions or inquiries, please contact:

- **Project Maintainer**: [Mario Petkov](mailto:https://www.linkedin.com/in/mario-petkov-a5582a227/)
- **GitHub**: [https://github.com/MarioCSharp/Health-System](https://github.com/MarioCSharp/Health-System)

---

Thank you for using MedCare! We hope this project helps you manage your health more effectively.
