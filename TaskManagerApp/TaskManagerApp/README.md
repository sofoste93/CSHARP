# TaskManagerApp

A simple task management web application built with ASP.NET Core MVC. This project serves as a practical example of building a web application with CRUD (Create, Read, Update, Delete) functionality. The user interface is in French.

## Features

*   **List Tasks**: View all tasks with their title, creation date, and status.
*   **Create Task**: Add a new task with a title and description.
*   **View Details**: See the detailed information for a specific task.
*   **Edit Task**: Modify the title, description, and completion status of an existing task.
*   **Delete Task**: Remove a task from the list.
*   **Notifications**: Display success/error messages to the user after performing an action.
*   **Validation**: Both client-side and server-side validation for form inputs.

## Technology Stack

*   **Backend**: C#, ASP.NET Core MVC
*   **Database**: Entity Framework Core (designed to work with providers like SQL Server, SQLite, or an in-memory database).
*   **Frontend**:
    *   HTML5 & CSS3
    *   Bootstrap 5 for styling and responsive UI components.
    *   jQuery for client-side scripting.
    *   jQuery Validation & jQuery Unobtrusive Validation for seamless client-side form validation.
*   **Templating**: Razor Pages

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

*   [.NET SDK](https://dotnet.microsoft.com/download) (version 9.0 or later, as specified in the project files).
*   An IDE like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/).
*   A database server like SQL Server (including LocalDB), or you can configure it to use SQLite or an in-memory database.

### Installation & Setup

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/sofoste93/CSHARP.git
    cd TaskManagerApp
    ```

2.  **Restore dependencies:**
    Open a terminal in the project root directory (`TaskManagerApp/`) and run:
    ```bash
    dotnet restore
    ```

3.  **Database Setup:**
    The project is likely configured to use Entity Framework Core migrations.
    *   Ensure your `appsettings.json` has the correct database connection string.
    *   Apply the migrations to create the database schema:
    ```bash
    dotnet ef database update
    ```

4.  **Run the application:**
    ```bash
    dotnet run
    ```

5.  **Access the application:**
    Open your web browser and navigate to the URL provided in the console output (e.g., `https://localhost:5266`).

## Project Structure

The project follows the standard ASP.NET Core MVC structure:

*   `\Controllers`: Contains the MVC controllers that handle user requests and business logic.
    *   `TaskItemsController.cs`: Manages the CRUD operations for tasks.
*   `\Models`: Contains the data models.
    *   `TaskItem.cs`: Represents a single task entity.
*   `\Views`: Contains the Razor files for the UI.
    *   `\TaskItems`: Views for creating, deleting, viewing details, editing, and listing tasks.
*   `\wwwroot`: Contains static client-side assets like CSS, JavaScript, and third-party libraries (Bootstrap, jQuery).
*   `\Data`: (Likely location) Contains the `DbContext` and migration files for Entity Framework Core.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.