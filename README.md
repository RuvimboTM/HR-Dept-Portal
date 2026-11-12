# HR Department Portal

An ASP.NET web portal for the HR department, built as part of the **"Utilising Robotic Process Automation to Optimise the Job Recruitment Process"** project. HR staff log in to view and manage job postings and applicants, whose data is collected by a UiPath automation bot.

## Part of a larger project

This repository is the web portal. The other parts of the project are:

- **UiPath automation (CV processing bot):** https://github.com/RuvimboTM/HR-Optimisation-UiPath
- **Power BI dashboard:** preview screenshots are in the UiPath repository's `PowerBI/` folder.

## Features

- Secure HR staff login
- View job postings and applicant details
- View applicant education, experience and skills
- Reads from the same MySQL database that the UiPath bot populates
- Add, delete, update job criteria


## Built with

- ASP.NET Web Forms (.NET Framework 4.7.2)
- C#
- MySQL (via `MySql.Data`)
- Visual Studio

## Getting started

### Prerequisites

- Visual Studio 2022 (or later) with the **ASP.NET and web development** workload
- .NET Framework 4.7.2 developer pack
- Access to a MySQL database with the project schema (`cvstream`)

### Setup

1. **Clone the repository**
   ```
   git clone https://github.com/RuvimboTM/HR-Dept-Portal.git
   ```

2. **Open the solution** (`HR Dept.slnx`) in Visual Studio.

3. **Restore NuGet packages.** Visual Studio normally does this automatically. If not, right-click the solution and choose **Restore NuGet Packages**.

4. **Create your database connection file.** For security, the real connection string is **not** stored in this repository.
   - In the `HR Dept` project folder, copy `connectionStrings.config.example` and rename the copy to `connectionStrings.config`.
   - Open `connectionStrings.config` and replace the placeholders with your own details:
     ```xml
     <connectionStrings>
       <add name="MySqlConnection"
            connectionString="Server=YOUR_SERVER;Database=YOUR_DB;Uid=YOUR_USER;Pwd=YOUR_PASSWORD;"
            providerName="MySql.Data.MySqlClient" />
     </connectionStrings>
     ```
   - `connectionStrings.config` is listed in `.gitignore`, so it will never be committed.

5. **Run the project.** Press **F5** in Visual Studio. The login page should load.

## Configuration

`web.config` loads the connection string from the separate file:

```xml
<connectionStrings configSource="connectionStrings.config" />
```

If the site shows a configuration error, check that:

- `connectionStrings.config` is in the same folder as `web.config`
- the file contains **only** the `<connectionStrings>` block, with no `<configuration>` wrapper
- the placeholder values have been replaced with real ones

If you see *"Unable to connect to any of the specified MySQL hosts"*, check the server name, and make sure you are on a network that can reach the database server (for example, the campus network or VPN).


## Project structure

```
HR Dept/
├── HR Dept/                          # Web project
│   ├── web.config                    # App configuration (no secrets)
│   ├── connectionStrings.config.example   # Template for database settings
│   └── ...
├── HR Dept.slnx                      # Visual Studio solution
└── README.md
```


## Author

Ruvimbo Mandava
