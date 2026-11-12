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
-- ============================================================
-- HR Recruitment Database (cvstream)
-- Structure only, matching the ER diagram. No data, no credentials.
-- ============================================================

CREATE DATABASE IF NOT EXISTS cvstream
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE cvstream;

-- ------------------------------------------------------------
-- admin (HR staff who log in to the portal)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS admin (
    adminId   INT AUTO_INCREMENT PRIMARY KEY,
    email     VARCHAR(100) NOT NULL,
    password  VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- education_levels
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS education_levels (
    education_level_id  INT AUTO_INCREMENT PRIMARY KEY,
    level_name          VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- skills
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS skills (
    skill_id    INT AUTO_INCREMENT PRIMARY KEY,
    skill_name  VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- jobs
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS jobs (
    job_id               INT AUTO_INCREMENT PRIMARY KEY,
    job_title            VARCHAR(100) NOT NULL,
    department           VARCHAR(100),
    education_level_id   INT,
    education_keywords   VARCHAR(255),
    is_active            TINYINT DEFAULT 1,
    experience_keywords  VARCHAR(255),
    CONSTRAINT fk_jobs_education_level
        FOREIGN KEY (education_level_id) REFERENCES education_levels(education_level_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- job_skills
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS job_skills (
    jobSkills_id      INT AUTO_INCREMENT PRIMARY KEY,
    job_id            INT NOT NULL,
    skill_id          INT NOT NULL,
    importance_order  INT,
    CONSTRAINT fk_job_skills_job   FOREIGN KEY (job_id)   REFERENCES jobs(job_id),
    CONSTRAINT fk_job_skills_skill FOREIGN KEY (skill_id) REFERENCES skills(skill_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- applicants
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS applicants (
    applicant_id  INT AUTO_INCREMENT PRIMARY KEY,
    email         VARCHAR(255),
    phone         VARCHAR(20),
    education     VARCHAR(255),
    skills        VARCHAR(255)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- applicant_skills
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS applicant_skills (
    id            INT AUTO_INCREMENT PRIMARY KEY,
    applicant_id  INT NOT NULL,
    skill_id      INT NOT NULL,
    CONSTRAINT fk_applicant_skills_applicant FOREIGN KEY (applicant_id) REFERENCES applicants(applicant_id),
    CONSTRAINT fk_applicant_skills_skill     FOREIGN KEY (skill_id)     REFERENCES skills(skill_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- applications
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS applications (
    application_id       INT AUTO_INCREMENT PRIMARY KEY,
    applicant_id         INT NOT NULL,
    job_id               INT NOT NULL,
    qualification_score  INT,
    application_date     DATETIME,
    application_status   VARCHAR(50),
    cv_file_path         VARCHAR(255),
    CONSTRAINT fk_applications_applicant FOREIGN KEY (applicant_id) REFERENCES applicants(applicant_id),
    CONSTRAINT fk_applications_job       FOREIGN KEY (job_id)       REFERENCES jobs(job_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- education
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS education (
    education_id   INT AUTO_INCREMENT PRIMARY KEY,
    applicant_id   INT NOT NULL,
    qualification  VARCHAR(100),
    CONSTRAINT fk_education_applicant FOREIGN KEY (applicant_id) REFERENCES applicants(applicant_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- experience
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS experience (
    id            INT AUTO_INCREMENT PRIMARY KEY,
    applicant_id  INT NOT NULL,
    roles         VARCHAR(500),
    field         VARCHAR(150),
    CONSTRAINT fk_experience_applicant FOREIGN KEY (applicant_id) REFERENCES applicants(applicant_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
--
-- ------------------------------------------------------------
-- INSERT INTO admin (email, password) VALUES ('admin@example.com', 'PASTE_HASHED_PASSWORD_HERE');
## Author

Ruvimbo Mandava
