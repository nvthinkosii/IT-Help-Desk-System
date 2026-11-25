# **IT Help Desk Ticketing System**



A Windows desktop application developed using C# and MySQL to manage IT support requests, track ticket progress and support different user roles.



### **Project Overview**



The IT Help Desk Ticketing System is a desktop-based application designed to simplify the management of IT support tickets within an organisation.



Users can submit support requests, while technicians and administrators can manage, assign and resolve tickets through a centralised dashboard.



The application includes authentication, role based access and database integration.



### **Features**



* User authentication and login.
* Initial administrator account setup.
* User registration and account management.
* Role-based access for Administrators, Technicians and Employees.
* Create and submit support tickets.
* View and search support tickets.
* Filter tickets by status.
* Assign tickets to technicians.
* Resolve tickets with resolution notes.
* Dashboard statistics showing total, open, in-progress and resolved tickets.
* Ticket reporting.
* Logout functionality.



### **User Roles**



|**Role:**|**Capabilities:**|
|-|-|
|Administrator|Manage users, view tickets, assign and resolve tickets, access reports|
|Technician|View tickets, assign tickets to themselves and resolve assigned tickets|
|Employee|Submit tickets and view their own tickets|



### **Technologies Used**



* C#
* .NET 10
* Windows Forms (WinForms)
* MySQL
* MySqlConnector
* Visual Studio
* Git and GitHub



### **Database**

The application uses a MySQL database named helpdesk\_db.



The database contains two main tables:

* users – stores user account information and roles.
* tickets – stores support ticket details, assignments and resolution information.



Passwords are hashed using PBKDF2 with SHA-256 before being stored in the database.



### **Project Structure**



ITHelpDeskSystem/

│

├── Data/

├── Forms/

├── Helpers/

├── Models/

├── Services/

│

├── Program.cs

├── ITHelpDeskSystem.csproj

├── database.sql

├── README.md

├── .gitignore

│

└── screenshots/



### **Installation and Setup**

###### **Prerequisites**



* Windows operating system
* Visual Studio with .NET desktop development support
* .NET 10 SDK
* MySQL Server
* MySQL Workbench (recommended)



**1. Clone the Repository**

git clone https://github.com/YOUR-USERNAME/IT-Help-Desk-Ticketing-System.git



**2. Set Up the Database:**

1. Open MySQL Workbench.
2. Connect to your MySQL Server.
3. Open and execute the database.sql script.
4. Confirm that the helpdesk\_db database and its tables have been created.



**3. Configure the Database Connection**

The application reads its database connection string from a Windows environment variable.



Create a user environment variable named:

HELPDESK\_DB\_CONNECTION



Set its value using your own MySQL credentials:

Server=localhost;Port=3306;Database=helpdesk\_db;User ID=root;Password=YOUR\_PASSWORD;



Replace YOUR\_PASSWORD with your own MySQL password.



Do not commit your actual database credentials to GitHub.



Restart Visual Studio after setting the environment variable if necessary.



**4. Run the Application**



* Open ITHelpDeskSystem.csproj in Visual Studio.
* Allow the project dependencies to restore.
* Build the solution.
* Run the application.



On the first launch, the application will prompt you to create the initial administrator account if no users exist.



### **Application Screenshots**

\### Login Screen

!\[Login Screen](screenshots/login-screen.png)



\### Admin Dashboard

!\[Admin Dashboard](screenshots/admin-dashboard.png)



\### Technician Dashboard

!\[Technician Dashboard](screenshots/technician-dashboard.png)



\### Create Ticket Form

!\[Create Ticket Form](screenshots/create-ticket-form.png)



\### Register User Form

!\[Register User Form](screenshots/register-user-form.png)



\### Ticket Report

!\[Ticket Report](screenshots/ticket-report.png)



### **Learning Outcomes**



This project provided practical experience in:

* C# application development.
* Object-oriented programming.
* Windows Forms interface development.
* MySQL database design and integration.
* CRUD operations.
* User authentication and password hashing.
* Role-based application permissions.
* Search and filtering functionality.
* Software testing and debugging.
* Git and GitHub version control.



### **Future Improvements**

Potential future enhancements include:

* Ticket priority indicators and visual status badges.
* Email notifications.
* More advanced reporting and analytics.
* Ticket history and audit logging.
* A centralised API-based architecture for stronger server-side access control.



### **Author**

Developed as a personal software development portfolio project to demonstrate practical skills in C#, database management and application development by ***Lindokuhle Nkosinathi Nkosi***.

