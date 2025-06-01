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

## 🧪 How to Verify

1. Generate token via `/api/token`
2. Make authenticated call to `/api/order/details` with the token in the request header.
3. Add a product to the cart and proceed to checkout — ensure `GiftMessage` is a required field.

## 📦 Docker

No breaking changes to the base image. Dockerfile updated to reflect custom plugin additions. Supports nopCommerce 4.80 and .NET 9.

## 🔗 Useful Links

- [nopCommerce Developer Docs](https://docs.nopcommerce.com/developer/index.html)
- [nopCommerce Marketplace](https://www.nopcommerce.com/marketplace)
- [Training Courses](https://www.nopcommerce.com/training)

---

Feel free to extend this plugin or use it as a reference for your own nopCommerce integrations.
