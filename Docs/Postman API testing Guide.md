﻿# 🧪 Postman API Testing Guide

This guide explains how to use the provided Postman collection to test the custom NopCommerce APIs implemented as part of the BambooCard plugin assessment.

---

## 📁 Collection Path

```
PostmanCollection/BC_Assesment.postman_collection.json
```

## 📌 Collection Overview

This Postman collection contains three primary API calls:

### 1. 🔐 `POST /api/customer/login`

Authenticates the customer and returns a JWT token.

---

### 2. 📦 `POST /api/order/details`

Uses the token to retrieve a customer's order history by email.

* Required header: `BC-Token` (automatically populated from the login response)
* Request body includes the target email

---

### 3. 🔓 `GET /api/customer/logout`

Logs out the customer session.

---

## ⚙️ Collection Variables

| Variable   | Purpose                                       |
| ---------- | --------------------------------------------- |
| `baseUrl`  | API base URL (e.g., `http://localhost:52930`) |
| `BC-Token` | JWT token used in header                      |

Update these in the **Variables** tab after import.

---

## 🧪 How to Test the APIs

1. **Import the Collection:**

   * Open Postman → Import → Select `BC_Assesment.postman_collection.json`

2. **Set **\`\`**:**

   * Go to the "Variables" tab → Change `baseUrl` to your environment URL.

3. **Login:**

   * Use `admin@bamboocard.com` and `Bamboo@card19` as credentials.
   * On success, the JWT token is stored in `BC-Token`.

4. **Order Lookup:**

   * Send `POST /api/order/details`
   * Email is required in the body. Auth is handled via `BC-Token`.

5. **Logout (Optional):**

   * Call the `GET /api/customer/logout` to end the session.

---

## ✅ Prerequisites

* NopCommerce 4.80 up and running
* BambooCard plugin installed and configured
* Database restored from `Database/BambooCardDB.bacpac`
* Admin credentials available

---

For more context, visit the main project repo:
🔗 [BambooCard GitHub Repository](https://github.com/Md-Ahteshamul-Islam/BambooCard/tree/main)
