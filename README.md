# ClassroomPlus
https://classroomplus.azurewebsites.net/ (If the link is slow, please wait because I'm using free plans on Azure)
An open-source full stack project by Milan Pantelć designed for communication between teachers and students in special classrooms. Please read the README for more information.

# Technical Specification: 

# Backend Architecture (.NET CORE):
The backend project follows the standard N-Layer architecture. It is divided into the Presentation Layer, Service Layer, Repository Layer, and DataAccessLayer, with dependencies managed through dependency injection. DTOs (Data Transfer Objects) are used for data transfer between the presentation, service, and repository layers, while entities are passed between DataAccess and Repository layers. The code-first approach was employed for code and database consistency. The Repository Pattern was utilized to achieve a high level of decoupling within the monolithic application, ensuring that potential changes to the entire Data Access layer only require modifications to the repository layer. Utilized the Unit of Work pattern to group multiple database requests in service layers into a single transaction. Authentication is implemented using JWT tokens, and real-time communication is facilitated through SignalR. Finally, the application has been deployed on Azure. I used global middleware for exception handling.

# Frontend Architecture (ANGULAR):
For the frontend, I used Angular. I made an effort to break down the UI into as many components as possible, with the potential for reuse throughout the codebase. I registered separate services for HTTP calls for each entity to separate the responsibility for managing HTTP requests from the components. I independently designed the UI.

# Deployment (AZURE): 
For deployment, I used Microsoft Azure because it's the most intuitive for me and offers excellent free options. I created a resource group and placed two instances of App Service within it, one for the frontend and one for the backend. I configured CORS, pricing plans, and accessed FTPS on several occasions to make changes to certain files. I also used advanced tools to access the backend instance's bash for specific tasks.

# Tech Stack:
  Backend: .NET CORE 7, C#, SignalR (Realtime), Entity Framework Core, MySql (It was initially using MSSQL, but due to more options for free MySQL hosting, I migrated the project to MySQL.)
  Frontend: Angular

  
  
