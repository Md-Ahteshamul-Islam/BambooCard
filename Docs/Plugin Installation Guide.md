﻿# 🧩 Bamboo Card Plugin Installation Guide for NopCommerce

This document guides you step-by-step to install and configure the **BambooCard.Plugin.Misc.AssessmentTasks** plugin in a fresh or existing NopCommerce 4.80 application.

---

## 🔧 Plugin Overview

**Name:** Assessment Tasks (Custom Features)
**Group:** Bamboo Card
**Version:** 1.0.1
**System Name:** Misc.AssessmentTasks
**Author:** Bamboo Card Engineering

This plugin includes features for:

* ✅ Custom Discount Rules
* ✅ Checkout Gift Message
* ✅ JWT-secured Order Retrieval API
* ✅ Containerization Support

---

## 📂 Step 1: Copy Plugin to NopCommerce

1. Locate the plugin project directory:

   ```
   Plugins/BambooCard.Plugin.Misc.AssessmentTasks
   ```
2. Copy this folder into your main NopCommerce solution’s **`/Plugins`** directory.
3. Ensure the `.csproj` file is included in your solution.

---

## ⚙️ Step 2: Build the Plugin

1. Open the `.sln` in Visual Studio.
2. Rebuild the solution.
   This will compile the plugin and generate views, settings, and routes.

---

## 🚀 Step 3: Run NopCommerce & Install Plugin

1. Launch the application (`Nop.Web`)

2. Go to `Admin > Configuration > Local Plugins`

3. Use the filters:

   * **Group:** `Bamboo Card`
   * Click **Search**

4. You will see the plugin listed:
   ![Plugin View](docs/assets/plugin-list.png)

5. Click **Install**

---

## 🧠 What Happens on Install?

Upon installation, the plugin will automatically:

* ✅ Create required **settings** (custom discounts, JWT config)
* ✅ Register **string resources** for admin panel labels/hints
* ✅ Register a **discount rule** with fallback percentage and criteria
* ✅ Add a **checkout attribute** named `GiftMessage`
* ✅ Add routes and controllers for secured API access

---

## ⚙️ Step 4: Configuration Page

Navigate to:
`Admin > Bamboo Card > Configure`

Here you’ll see two sections:

1. **Discount Settings**
2. **JWT / API Settings**

![Settings Screenshot](docs/assets/plugin-settings.png)

---

## 🔑 Admin Access

If you’ve restored the `.bacpac` file from `/Database/BambooCardDB.bacpac`, use:

```
Email: admin@bamboocard.com
Password: Bamboo@card19
```

---

## 🧼 Uninstalling the Plugin

1. Go to `Configuration > Local plugins`
2. Filter by **Group: Bamboo Card**
3. Click **Uninstall** next to the plugin

---

## 📌 Related Repositories

🔗 [GitHub Repo – Bamboo Card Assessment](https://github.com/Md-Ahteshamul-Islam/BambooCard/tree/main)

---

Need more help? Refer to the main README or ask your team lead.

---

© 2025 Bamboo Card Engineering
