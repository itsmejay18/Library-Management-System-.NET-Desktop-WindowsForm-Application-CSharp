Project Overview: Library Management System
SQL File:
- Naka self-contained na ang .sql file.
- Ang buhaton lang kay create schema nga parehas sa file name, then import — ready to use na.

Authentication:
- Naka Supabase ang authentication.
- Note: Dili permanent ang subscription, so kailangan nato i-resume or renew if needed.

Workflow / Guidelines
Purpose:
- Para kung gusto mo mag-practice or mag-develop bisan wala nay klase, pwede ra dire deretso. No hassle.

Reminders:
- Always push your changes sa GitHub para updated ang app nato.
- Pwede ta mag-collab real-time, pero inform sa GC kung kinsa ang magtrabaho para

Overview:
C# Windows Forms (WinForms/WPF) desktop application uses a hybrid database approach:
- Local SQL Server / MySQL: For primary and offline data storage.
- Supabase (PostgreSQL + Backend-as-a-Service): For cloud backup, remote sync, or authentication.

Key Features:
- User Authentication (optional via Supabase Auth)
- Local and Cloud (Supabase) database interaction
- CRUD Operations: Create, Read, Update, Delete
- Offline support with local SQL DB
- Sync button/feature to upload or retrieve data from Supabase
- Easy deployment for school or institution use

Architecture:

+------------------------+       +-------------------------+
|   C# WinForms/WPF App  | <-->  |  Local SQL Server/MySQL |
+------------------------+       +-------------------------+
            |
            |   Supabase SDK (.NET)
            V
+------------------------+
|     Supabase (Cloud)   |
|  - PostgreSQL Database |
|  - Auth (optional)     |
|  - REST/Realtime APIs  |
+------------------------+



