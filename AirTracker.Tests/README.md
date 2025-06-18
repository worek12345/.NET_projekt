# AirTracker

**AirTracker** to webowa aplikacja do monitorowania jako�ci powietrza, wykorzystuj�ca zewn�trzne API OpenAQ oraz baz� danych SQL Server z zaimplementowanym systemem uwierzytelniania i autoryzacji (ASP.NET Core Identity).

## Wykorzystane biblioteki

* **Microsoft.AspNetCore.Identity.EntityFrameworkCore** v8.0.0
* **Microsoft.AspNetCore.Identity.UI** v8.0.0
* **Microsoft.EntityFrameworkCore.Design** v8.0.0
* **Microsoft.EntityFrameworkCore.SqlServer** v8.0.0
* **Microsoft.EntityFrameworkCore.Tools** v8.0.0
* **Microsoft.VisualStudio.Web.CodeGeneration.Design** v8.0.0
* **coverlet.collector** v6.0.0 (raportowanie pokrycia test�w)
* **Microsoft.EntityFrameworkCore.InMemory** v8.0.5 (baza w pami�ci do test�w)
* **Microsoft.NET.Test.Sdk** v17.8.0 (uruchamianie test�w)
* **xunit** v2.5.3 (framework testowy)
* **xunit.runner.visualstudio** v2.5.3 (integracja test�w w Visual Studio)

## Wymagania wst�pne

* .NET 8 SDK ([pobierz](https://dotnet.microsoft.com/download/dotnet/8.0))
* SQL Server (lub LocalDB w �rodowisku developerskim)
* (opcjonalnie) narz�dzie `dotnet-ef`:

  ```bash
  dotnet tool install --global dotnet-ef
  ```
* (opcjonalnie) IDE: Visual Studio 2022+, Rider lub VS Code z rozszerzeniem C#

## Konfiguracja projektu

1. Otw�rz plik **AirTracker/appsettings.json** i dostosuj ustawienia:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=tcp:fajnyserver.database.windows.net,1433;Initial Catalog=airtrackerdb;Persist Security Info=False;User ID=sqladmin;Password=silnehaslo123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   },
   "OpenAQ": {
     "ApiKey": "<d5dcf83a43f170476aa6327168dac24d752e41c8b5656b6fd9f1a51ac9958d19>"
   }
   ```
2. Alternatywnie mo�esz u�y� zmiennych �rodowiskowych:

   * `ConnectionStrings__DefaultConnection` � connection string do bazy danych
   * `OpenAQ__ApiKey` � klucz API OpenAQ

## Instalacja i uruchomienie

1. Sklonuj repozytorium lub rozpakuj archiwum:

   ```bash
   git clone <repozytorium>
   cd AirTracker
   ```
2. Przywr�� pakiety NuGet:

   ```bash
   dotnet restore
   ```
3. Zainstaluj narz�dzie `dotnet-ef` (je�li jeszcze nie jest zainstalowane):

   ```bash
   dotnet tool install --global dotnet-ef
   ```
4. Zastosuj migracje i utw�rz baz� danych:

   ```bash
   dotnet ef database update
   ```
5. Uruchom aplikacj�:

   ```bash
   dotnet run --project AirTracker
   ```

   Podczas pierwszego uruchomienia aplikacja zaimportuje domy�lny zestaw miast i wy�wietli w konsoli komunikat:

   > `>>> KONIEC importu. Sprawd� baz�.`
6. Otw�rz przegl�dark� i przejd� pod adres podany w konsoli (domy�lnie `https://localhost:5001`).

## Uruchamianie test�w

1. Przejd� do katalogu z testami:

   ```bash
   cd AirTracker.Tests
   ```
2. Uruchom testy:

   ```bash
   dotnet test
   ```

## Debugowanie i rozwijanie

* Generowanie nowych migracji:

  ```bash
  dotnet ef migrations add <NazwaMigracji>
  ```
* Reset bazy (usu� istniej�c� baz�, ponownie zastosuj migracje):

  ```bash
  dotnet ef database drop --force
  dotnet ef database update
  ```
* Klucz API oraz connection string mo�na te� dostarczy� jako argumenty linii polece� lub za pomoc� Secret Manager w ASP.NET Core.

---

*README wygenerowane automatycznie na podstawie analizy projektu.*
