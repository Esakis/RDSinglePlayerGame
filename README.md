# Red Dragon - Gra RPG

Klon tekstowej gry RPG Red Dragon. Full-stack: .NET 9 backend, Angular 16 frontend, MSSQL baza danych.

## Struktura Projektu

- **ClickTrackerAPI** - .NET 9 Web API backend (API gry)
- **ClickTrackerUI** - Angular 16 frontend (interfejs gry)

## Wymagania

- .NET 9 SDK
- Node.js i npm
- Angular CLI (`npm install -g @angular/cli`)
- SQL Server LocalDB (dołączony z Visual Studio)

## Baza Danych

Aplikacja używa SQL Server LocalDB:
```
Server=(localdb)\\mssqllocaldb;Database=RedDragonDB;Trusted_Connection=true;TrustServerCertificate=true;
```

Baza tworzy się automatycznie przy pierwszym uruchomieniu backendu wraz z danymi startowymi (potwory, przedmioty).

## Uruchamianie

### Najłatwiej - plik BAT

```
start-app.bat
```

### Ręcznie

**Backend:**
```
cd ClickTrackerAPI
dotnet run
```
API: `http://localhost:5069`

**Frontend:**
```
cd ClickTrackerUI
npm install
ng serve
```
Gra: `http://localhost:4200`

## Funkcje

- Rejestracja i logowanie postaci
- Rasa: Człowiek (zrównoważone statystyki)
- System walki z potworami (8 typów: od Szczura do Czerwonego Smoka)
- System doświadczenia i poziomów
- Przydzielanie punktów statystyk (Siła, Zręczność, Inteligencja, Wytrzymałość, Szczęście)
- Sklep z bronią, zbrojami i miksturami (15 przedmiotów)
- Ekwipunek - zakładanie broni/zbroi, używanie mikstur
- Odpoczynek (regeneracja HP i Many)
- Ranking bohaterów

## API Endpoints

### Auth
- `POST /api/auth/register` - Rejestracja nowej postaci
- `POST /api/auth/login` - Logowanie

### Character
- `GET /api/character/{id}` - Pobierz statystyki postaci
- `POST /api/character/{id}/allocate-stat` - Przydziel punkty statystyk
- `POST /api/character/{id}/rest` - Odpoczywaj
- `GET /api/character/{id}/inventory` - Ekwipunek
- `POST /api/character/{id}/equip/{itemId}` - Załóż przedmiot
- `POST /api/character/{id}/use-potion/{itemId}` - Użyj mikstury
- `GET /api/character/ranking` - Ranking

### Battle
- `GET /api/battle/monsters` - Lista potworów
- `POST /api/battle/{charId}/fight/{monsterId}` - Walcz z potworem
- `GET /api/battle/{charId}/log` - Historia walk

### Shop
- `GET /api/shop/items` - Przedmioty w sklepie
- `POST /api/shop/{charId}/buy/{itemId}` - Kup przedmiot
- `POST /api/shop/{charId}/sell/{charItemId}` - Sprzedaj przedmiot
