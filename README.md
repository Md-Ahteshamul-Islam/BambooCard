﻿# nopCommerce: Custom API & Checkout Extensions

This fork of the official [nopCommerce](https://www.nopcommerce.com/?utm_source=github&utm_medium=content&utm_campaign=homepage) repository introduces **custom features for order lookup, customer token validation, and checkout attribute management** as part of an assessment plugin project.

## 🔍 Background & Purpose

As part of a structured plugin development assessment, this project implements four tasks aimed at extending nopCommerce 4.80 using .NET 9. The focus is on:

- Real-world plugin architecture.
- Secure token-based authorization.
- Extensible API endpoints.
- Seamless integration into nopCommerce core services and conventions.

These additions demonstrate practical plugin development, secure communication using JWT, and enhanced order search functionality with proper localization and configuration practices.

## ✅ Summary of Completed Tasks

### 1. **Order Lookup API**
- **Route:** `POST /api/order/details`
- **Input:** Customer email
- **Output:** A list of past orders (order ID, total amount, order date)
- **Why:** Enables external systems to fetch order history via email.
- **How:** Uses a new factory `OrderAPIModelFactory` to generate `OrderLookupResponseModel`.

### 2. **Token-Based Authorization**
- JWT token generation
- Cookie-based delivery of the token.
- Secure token decoding in API controller using secret key from settings.
- Fallback to generate and persist secret key if missing.

### 3. **Customer Auto-Login via Token**
- Extracts token from request headers.
- Decodes and identifies the customer ID.
- Auto-logs in the customer via `workContext`.

### 4. **Checkout Attribute - Gift Message**
- Automatically adds a **required** checkout attribute `GiftMessage`.
- Injected via plugin `Install()` and `Update()` lifecycle methods.
- Includes localization via string resource and fallback checks to avoid duplication.

## ⚙️ Implementation Highlights

- Used `IJwtDecoder` extensions and `JwtParts` for secure token decoding.
- Registered services via Dependency Injection.
- Plugin respects `NopStartup` and `IPlugin` conventions.
- Docker compatibility retained — updated Dockerfile accordingly.
- Clean and minimal plugin configuration (no UI config page required).

## 📏 Conventions Followed

- **Commit message format:** `feat(scope): short summary`
- **Naming conventions:** PascalCase for C#, kebab-case for URLs, consistent key usage for payloads.
- **Localization:** All display strings use `IStringLocalizer` with resource keys. XML export provided.
- **Security:** Token-based authentication is implemented using `HS256` with configurable secret key.

## 🗄️ Preloaded Database

A `.bacpac` file is included in the `/Database` folder for quick database setup in SQL Server.

### 🔹 File Path: /Database/BambooCardDB.bacpac

### 🛠️ How to Use:
- Use **SQL Server Management Studio (SSMS)** or **Azure Data Studio**.
- Import the `.bacpac` as a new database (e.g., `nop480`).
- Update your connection string in `appsettings.json` or Docker `environment` variables accordingly.

### 🔐 Preloaded Admin Credentials:
| Field | Value |
|-------|-------|
| **Email** | `admin@bamboocard.com` |
| **Password** | `Bamboo@card19` |

> ✅ These credentials allow immediate login to the nopCommerce admin area for testing and validation.


## 🧪 How to Verify

1. Generate token via `/api/token`
2. Make authenticated call to `/api/order/details` with the token in the request header.
3. Add a product to the cart and proceed to checkout — ensure `GiftMessage` is a required field.

## 📬 Postman Collection

A Postman collection is provided to help test the custom API endpoints quickly and consistently.

### 📁 File Location: /PostmanCollection/BC_Assesment.postman_collection.json

### 📌 Contains Requests For:
- 🔐 `POST /api/token` – Generate JWT token for a valid customer
- 📦 `POST /api/order/details` – Lookup orders by email (requires JWT in `Authorization` header)

### ✅ How to Use:
1. Open Postman.
2. Click **Import** → choose the `BC_Assesment.postman_collection.json` file.
3. Set the base URL to match your local environment (e.g., `http://localhost:8080`).
4. Run the requests in order.

> Make sure to restore the `BambooCardDB.bacpac` to access test data with valid customer records.


## 📦 Docker

No breaking changes to the base image. Dockerfile updated to reflect custom plugin additions. Supports nopCommerce 4.80 and .NET 9.

## 🔗 Useful Links

- [nopCommerce Developer Docs](https://docs.nopcommerce.com/developer/index.html)
- [nopCommerce Marketplace](https://www.nopcommerce.com/marketplace)
- [Training Courses](https://www.nopcommerce.com/training)

---

Feel free to extend this plugin or use it as a reference for your own nopCommerce integrations.
