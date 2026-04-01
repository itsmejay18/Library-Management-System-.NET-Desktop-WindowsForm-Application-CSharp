# Library Management System

A desktop library management application built with C# Windows Forms and .NET Framework 4.7.2.

This project is designed for school or institutional library use. It helps administrators, staff, and student borrowers manage books, members, borrowing transactions, returns, overdue records, authors, publishers, and basic reporting from one desktop application.

## What This System Is

This system is a role-based library management platform with:

- `Admin`, `Staff`, and `Student` user access
- `Supabase` for authentication and role-based profile lookup
- `MySQL` for core library records and transaction storage
- `Windows Forms` dashboards for daily library operations

The current codebase uses a hybrid setup:

- `Supabase Auth + profiles table` for sign up, login, and role validation
- `MySQL librarydb` for books, members, borrow/return history, dues, reports, authors, publishers, and interlibrary loan requests

## User Roles

### Admin

Admins have the broadest access in the system. Based on the current source code, they can:

- View dashboard summary cards for total books, borrowed books, overdue items, and total members
- Add, edit, search, and delete books
- Add, edit, search, and delete members
- Process book borrowing and returns
- Manage authors and publishers
- Open report tabs for books, members, transactions, overdue books, authors, publishers, and borrowers
- Log out and relaunch the application session

### Staff

Staff users have a more operational workflow focused on daily circulation and lookup tasks. They can:

- View the dashboard
- Search books
- Search students or members
- Process borrowing and returning of books
- View borrowing and fine-related transaction logs
- Access the author and publisher module from the staff dashboard

### Student

Student users act as borrowers in the system. From the current implementation, they can:

- Sign up and log in through Supabase authentication
- Search and view book details
- Borrow available books using their own linked member account
- Return borrowed books
- Access an interlibrary request screen that appears to be under active development
- Access their own borrower dashboard

Important note:

- The student borrow flow expects the logged-in email in Supabase to also exist in `members_tbl.email` inside MySQL. If the email is not found there, borrowing will fail for that account.

## Core Features

- Role-based login with explicit role selection
- Supabase sign up and authentication flow
- Book catalog management
- Member management
- Borrow and return processing
- Automatic due date generation
- Fine calculation for overdue returns
- Author and publisher management
- Report tabs for major library records
- Interlibrary request UI scaffolding
- Dashboard counters for key library statistics

## Borrow and Return Workflow

The source code currently performs these actions during transactions:

- Borrowing inserts a record into `borrowers_tbl`
- A corresponding dues entry is created in `dues_tbl`
- A transaction record is inserted into `transactions_tbl`
- The selected book is marked as `Borrowed`
- The due date is set to 7 days from the borrow date

When returning:

- The app looks up the active borrow record
- If the due date has passed, it calculates a fine at `5` per overdue day
- The dues record is updated
- The borrow record receives a return date
- The book status is changed back to `Available`

## Reports Available

The admin reporting module currently includes tabs for:

- Books report
- Members report
- Transactions report
- Overdue books report
- Authors report
- Publishers report
- Borrowers report

The staff log screen also shows circulation history with member, book, due date, return date, fine, and status information.

## Technology Stack

- `C#`
- `.NET Framework 4.7.2`
- `Windows Forms`
- `MySQL`
- `Supabase Auth / REST API`
- `MySql.Data`
- `Newtonsoft.Json`

Referenced packages also include `ZXing.Net` and `AForge`, but they are not currently wired into visible source features in this repository.

## Project Structure

```text
.
|-- Library Management System.sln
|-- README.md
|-- librarydb.sql
`-- Library Management System/
    |-- App.config
    |-- Program.cs
    |-- Class/
    |-- UI/
    |-- Borrower UI/
    `-- Usercontrol/
```

Main folders:

- `Library Management System/UI` - Login, signup, and dashboard forms
- `Library Management System/Borrower UI` - Student borrower screens and request forms
- `Library Management System/Usercontrol/AdminUserControl` - Admin modules for books, members, reports, authors, publishers, and dashboard widgets
- `Library Management System/Usercontrol/StaffUserControl` - Staff modules for search, issue/return, and transaction log viewing
- `Library Management System/Class` - Helper classes, session state, and model classes
- `librarydb.sql` - MySQL schema and sample data dump

## Database Overview

The included `librarydb.sql` file defines the main library tables, including:

- `books_tbl`
- `authors_tbl`
- `publishers_tbl`
- `members_tbl`
- `borrowers_tbl`
- `transactions_tbl`
- `dues_tbl`
- `interlibrary_loans`
- `users_tbl`
- `session_log`

Important note:

- The repository still includes a seeded `users_tbl`, but the active login screen in the app does not authenticate against this table. The current login implementation uses `Supabase`.

## Setup Instructions

### 1. Clone the repository

```bash
git clone <your-repository-url>
```

### 2. Open the solution

Open:

- `Library Management System.sln`

with Visual Studio.

### 3. Restore NuGet packages

If Visual Studio does not restore packages automatically, restore them before building the solution.

### 4. Create the MySQL database

Create a database named:

```sql
librarydb
```

Then import:

- `librarydb.sql`

This SQL dump already contains schema definitions and sample records.

### 5. Update the MySQL connection string

The current connection string is stored in:

- `Library Management System/App.config`

Replace it with your local MySQL credentials before running the project.

### 6. Configure Supabase

The project currently contains hard-coded Supabase values in several source files. Before using the app in your environment, review and update:

- Supabase project URL
- Supabase anon key
- `profiles` table structure
- authentication rules

The login and signup flow expects a Supabase `profiles` table with fields similar to:

- `id`
- `full_name`
- `email`
- `role`

### 7. Run the application

Set the WinForms project as the startup project and run it from Visual Studio.

The app starts at:

- `FormLogin`

## Suggested Contributor Notes

If you plan to contribute or continue building this project, these are the most important implementation notes from the current codebase:

- Authentication is handled by Supabase, not by `users_tbl`
- Most operational data is stored in MySQL
- Many connection strings are hard-coded directly in forms and user controls, not only in `App.config`
- Student borrowing depends on a matching email in the MySQL members table
- Some student dashboard areas are only partially implemented
- Some package references appear prepared for future features rather than currently active ones

## Known Gaps or Cleanup Items

Before publishing this repository publicly or using it in production, it is strongly recommended to:

- Remove hard-coded database passwords from `App.config` and source files
- Remove or rotate exposed Supabase keys
- Review sample personal data included in `librarydb.sql`
- Centralize configuration so credentials are not repeated across forms
- Clarify unfinished UI areas such as student reports and interlibrary request navigation
- Decide whether `users_tbl` is still needed, since Supabase now handles login

## Why This Project Is Useful

This project is a solid academic or portfolio system because it demonstrates:

- Desktop application development with WinForms
- Role-based user experience
- MySQL database design and CRUD workflows
- Authentication integration with an external backend service
- Borrowing, return, overdue, and fine logic
- Reporting and dashboard features for real operational data

## Contributing

Contributions are welcome.

If you want to improve this system, good places to start are:

- Refactoring configuration and secrets handling
- Improving validation and error messages
- Completing partially implemented student features
- Cleaning up duplicate database connection logic
- Adding tests or a documented QA checklist
- Improving report filtering and export support

## License

No license file is currently included in this repository.

If you plan to publish this project on GitHub for open contribution, consider adding a license such as `MIT`.
