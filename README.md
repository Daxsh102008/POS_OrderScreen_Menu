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
- **Manager passcode:** `PSD` — prompted by the Manager Panel and Order History buttons

All passwords in this repository have been reset to that value, and they are stored
**hashed**, not as the words above. The database also contains sample menu items,
tables and orders.

---

## Project layout

```
POS_OrderScreen_Menu/
├─ *.cs / *.Designer.cs     one pair per screen
├─ App.config               connection string (PosDb)
├─ PasswordHasher.cs        PBKDF2 hashing and verification
├─ POS_DB.mdf               sample database
├─ *.resx                   form resources, incl. item images
└─ Properties/
```

---

## Security

- **Passwords are hashed with PBKDF2-HMAC-SHA256**, 100,000 iterations, with a fresh
  random 16-byte salt per account. The stored form is
  `PBKDF2$<iterations>$<salt>$<hash>` and verification is constant-time, so two users
  with the same password never produce the same stored value. See `PasswordHasher.cs`.
- **The manager passcode is no longer in the source.** It lives in an `AppSettings`
  table, stored hashed, and is checked via `DatabaseFunction.VerifyManagerPasscode`.
  Changing it is one row:

  ```powershell
  Add-Type -Path .\PasswordHasher.cs
  $new = [RESTAU.PasswordHasher]::Hash('your-new-passcode')
  # UPDATE AppSettings SET SettingValue = '<$new>' WHERE SettingKey = 'ManagerPasscode'
  ```

- **A value that is not a well-formed hash is rejected**, never compared as plaintext,
  so a leftover legacy row cannot be logged into by coincidence.
- Schema changes made for this: `Users.password` widened from `nchar(50)` to
  `nvarchar(200)` to hold a hash, and the `AppSettings` table added.

## Known limitations / next steps

- The manager passcode is a **single shared secret** rather than a per-user permission.
  A role column on `Users` would be the stronger model.
- **The demo manager passcode `PSD` is only three characters**, so it is brute-forceable
  regardless of how well it is hashed. Change it before using this anywhere real.
- `DatabaseFunction.cs` is a single static helper holding most queries; a repository
  layer would separate data access from the UI.
- No unit tests yet — the logic in `Payment.cs` is the obvious first target.
- `Menu.resx` embeds item images as base64. They were exported and resized from
  1024x1024 down to 320x320 (the buttons render at 192x180), which cut the file
  from 89 MB to 6 MB.

---

## Acknowledgements

Built with two teammates for LIPC1266. Published with their permission.
