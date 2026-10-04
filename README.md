# POS Order Screen & Menu

A Windows Forms point-of-sale desktop application for a restaurant — order screen,
menu browsing, payment handling, table management and a manager panel, backed by
SQL Server (LocalDB).

Built as a team project for **LIPC1266 Professional Software Development** at
De Montfort University (June 2026). I owned the **payment module** — order totals,
receipts and the bulk of the database layer.

---

## Features

| Screen | What it does |
|---|---|
| `Log_In` / `Registration` | Staff account sign-in and account creation |
| `Dashboard` | Landing screen after login |
| `Menu` | Category-filtered menu with item images (burgers, pizzas, sides, drinks) |
| `Payment` | Order totals and receipt generation — **my module** |
| `OrderHistory` | Past orders |
| `ManagerPanel` | Manager-only view |
| `UsersManagement` | Add / remove staff accounts |
| `PasscodeVerify` | Gate for manager-only actions |

---

## Tech stack

- **C#** — .NET Framework **4.7.2**, Windows Forms (`WinExe`)
- **SQL Server LocalDB** — database attached via `AttachDbFilename`, `Integrated Security` (no passwords in config)
- **Visual Studio** solution: `POS_OrderScreen_Menu.slnx`

Connection strings are **not** hard-coded in source — they live in `App.config`
and are read through `ConfigurationManager.ConnectionStrings["PosDb"]`.

---

## Running it

1. You need **Visual Studio** with the **.NET desktop development** workload, and
   **SQL Server Express LocalDB** (installed with the workload).
2. Open `POS_OrderScreen_Menu.slnx`.
3. Press **F5**.

`POS_DB.mdf` is copied to the output folder automatically on build. The app attaches
it at runtime using `|DataDirectory|\POS_DB.mdf`, so no path editing is needed.

### Demo login

The database ships with staff accounts and menu data so the app is usable immediately:

- **Username:** any username in the `Users` table
- **Password:** `demo1234`

All passwords in this repository have been reset to that value. The database also
contains sample menu items, tables and orders.

---

## Project layout

```
POS_OrderScreen_Menu/
├─ *.cs / *.Designer.cs     one pair per screen
├─ App.config               connection string (PosDb)
├─ POS_DB.mdf               sample database
├─ *.resx                   form resources, incl. item images
└─ Properties/
```

---

## Known limitations / next steps

- **Passwords are stored in plaintext.** The next meaningful change is hashing them
  (bcrypt or PBKDF2) with a per-user salt.
- The manager gate in `Dashboard.cs` compares against a **hard-coded passcode**. It should
  be a per-user role check in the database instead. (Noted rather than hidden — it is
  visible in this repository.)
- `DatabaseFunction.cs` is a single static helper holding most queries; a repository
  layer would separate data access from the UI.
- No unit tests yet — the logic in `Payment.cs` is the obvious first target.
- `Menu.resx` embeds item images as base64. They were exported and resized from
  1024x1024 down to 320x320 (the buttons render at 192x180), which cut the file
  from 89 MB to 6 MB.

---

## Acknowledgements

Built with two teammates for LIPC1266. Published with their permission.
