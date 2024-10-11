```markdown
# Infobip Projekt - Rezervacije Web Aplikacija

Ovaj repozitorij sadrži web aplikaciju za upravljanje rezervacijama, razvijenu u sklopu projekta za Infobip.

## Tehnologije
- **ASP.NET Core** - Backend aplikacije
- **Entity Framework Core** - Rad s bazom podataka
- **Fluent UI** - Frontend komponenta za korisničko sučelje
- **React** - Frontend framework za razvoj korisničkog sučelja
- **SQLite** - Korištena baza podataka

## Funkcionalnosti
- Kreiranje, uređivanje i brisanje rezervacija
- Pregled svih rezervacija
- Implementacija autentifikacije i autorizacije
- Filtriranje i pretraživanje rezervacija

## Postavke projekta

### Prerequisites
- .NET SDK 6.0 ili novija verzija
- Node.js i npm

### Instalacija

1. Kloniraj repozitorij:
   ```bash
   git clone https://github.com/Krapic/Infobip-Projekt-RezervacijeWebApp.git
   ```
2. Navigiraj u backend direktorij i pokreni aplikaciju:
   ```bash
   cd backend
   dotnet restore
   dotnet run
   ```
3. Navigiraj u frontend direktorij i pokreni aplikaciju:
   ```bash
   cd frontend
   npm install
   npm start
   ```

### Migracije baze podataka
Za primjenu migracija baze podataka, koristi sljedeće naredbe:

1. Generiranje migracije:
   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. Ažuriranje baze podataka:
   ```bash
   dotnet ef database update
   ```

## Konfiguracija
Aplikacija koristi SQLite bazu podataka po defaultu, no može se konfigurirati i za korištenje druge baze (npr. SQL Server). Konfiguracija se nalazi u datoteci `appsettings.json`.

## Doprinosi
Slobodno pošaljite PR za bilo kakve prijedloge ili poboljšanja.

## Licenca
Ovaj projekt je licenciran pod MIT licencom.
```
