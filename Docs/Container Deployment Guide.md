
# Containerized Deployment Guide

This guide outlines the steps to build and run the BambooCard NopCommerce application using Docker.

## 🐳 Prerequisites

- Docker Desktop installed (Windows/macOS) or Docker CLI (Linux)
- .NET SDK 9.0 (as required by NopCommerce 4.80)
- Ensure `Dockerfile` and `docker-compose.yml` are located at the root of the solution

---

## 🧱 Dockerfile Overview

The repository already includes a `Dockerfile` that:
- Builds the application using SDK image
- Publishes it to a lightweight runtime image
- Sets `Nop.Web.dll` as the entry point

You can find the file at:
```
/Dockerfile
```

---

## ⚙️ Build and Run with Docker CLI

```bash
docker build -t bamboocard-app .
docker run -d -p 8080:80 --name bamboocard-container bamboocard-app
```

Then open [http://localhost:8080](http://localhost:8080) in your browser.

---

## 🧩 Using Docker Compose

If your setup includes additional dependencies like SQL Server, use the provided `docker-compose.yml`.

### Example Command:

```bash
docker-compose up --build
```

This spins up the app and SQL Server with linked configuration.

---

## 🗃 Database Initialization

- If using a local SQL Server, manually restore the `BambooCardDB.bacpac` file found under:
  ```
  /Database/BambooCardDB.bacpac
  ```
- Default admin credentials:
  - **Email:** `admin@bamboocard.com`
  - **Password:** `Bamboo@card19`

---

## ☁️ Deployment on Cloud (Optional)

### AWS EC2 / Azure VM

1. Install Docker on your instance
2. Clone this repository
3. Run `docker build` and `docker run` as above

For CI/CD, container images can be pushed to:
- [DockerHub](https://hub.docker.com)
- [AWS ECR](https://aws.amazon.com/ecr/)
- [Azure Container Registry](https://learn.microsoft.com/en-us/azure/container-registry/)

---

## 🔄 Rebuild and Cleanup

```bash
docker stop bamboocard-container
docker rm bamboocard-container
docker rmi bamboocard-app
```

---

Happy Containerizing! 🐋
