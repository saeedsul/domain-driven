
# Domain Driven API

A structured Domain-Driven Design (DDD) project with separate layers for API, Services, Persistence, UX, and Common utilities.
Includes Docker support, Azure pipelines, and Infrastructure as Code (IaC) via Terraform.

## Folder Structure
```
DomainDriven.sln
.dockerignore
.gitignore
azure-pipeline
azure-pipelines
Dockerfile
launchSettings
pipeline
/
|-- Api/             # ASP.NET Core Web API project
|-- Common/          # Shared utilities, models, helpers
|-- IAC/             # Infrastructure as Code (Terraform scripts)
|-- Persistence/     # Database access and repositories
|-- Services/        # Application services and business logic
|-- TestProject/     # Unit and Integration tests
|-- UX/              # Frontend application (optional, could be Razor Pages, Blazor, etc.)
```

---

## Base URL
```
@baseUrl = http://localhost:5118/api
```

---

## Endpoints

### Get All Activities
```http
GET {{baseUrl}}/activity/get-all-activities
```

### Get Activity by ID
```http
GET {{baseUrl}}/activity/11111111-1111-1111-1111-111111111111
```

### Create New Activity
```http
POST {{baseUrl}}/activity/create-activity
Accept: application/json
Content-Type: application/json

Body:
{
  "name": "test",
  "fromAddress": "sender5@example.com",
  "toEmailAddress": "user5@example.com",
  "fromName": "Campaign5"
}
```

### Update Existing Activity
```http
PUT {{baseUrl}}/activity/bd036630-3108-402d-b59a-d4e71522ac17
Content-Type: application/json

Body:
{
  "name": "test user",
  "fromAddress": "test@test2.com",
  "toEmailAddress": "test@test.com",
  "fromName": "test",
  "openedDate": "2025-04-23T21:30:00.000Z",
  "bouncedDate": "2025-04-22T20:00:00.000Z"
}
```

### Delete Activity
```http
DELETE {{baseUrl}}/activity/14799e05-51d4-4d6f-8e42-74b3cd45fa4a
```

---

## Docker
Build and run the API using Docker:

```bash
docker build -t domain-driven-api .
docker run -p 5118:5118 domain-driven-api
```

---

## Infrastructure as Code (IAC)
- The `IAC/` folder contains Terraform scripts for provisioning cloud infrastructure (e.g., App, Database).
- cd IAC
- terraform init
- terraform plan
- terraform apply --auto-approve
- this will build api project and great the api contianer and the sql db container
- open a browser and go to http://localhost:80 

---

## Docker
- A `Dockerfile` is available to containerize and run the API.
- Build and run the Docker container:
  ```bash
  docker build -t movies-api .
  docker run -p 80:80 movies-api
  ```

---
 

## For HTTP File Execution
More about HTTP files: [https://aka.ms/vs/httpfile](https://aka.ms/vs/httpfile)

---

## License
This project is licensed under the MIT License.
