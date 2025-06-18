# AirTracker

**AirTracker** to webowa aplikacja do monitorowania jakoœci powietrza, wykorzystuj¹ca zewnêtrzne API OpenAQ oraz bazê danych SQL Server z zaimplementowanym systemem uwierzytelniania i autoryzacji (ASP.NET Core Identity).

## Wykorzystane biblioteki

* **Microsoft.AspNetCore.Identity.EntityFrameworkCore** v8.0.0
* **Microsoft.AspNetCore.Identity.UI** v8.0.0
* **Microsoft.EntityFrameworkCore.Design** v8.0.0
* **Microsoft.EntityFrameworkCore.SqlServer** v8.0.0
* **Microsoft.EntityFrameworkCore.Tools** v8.0.0
* **Microsoft.VisualStudio.Web.CodeGeneration.Design** v8.0.0
* **coverlet.collector** v6.0.0 (raportowanie pokrycia testów)
* **Microsoft.EntityFrameworkCore.InMemory** v8.0.5 (baza w pamiêci do testów)
* **Microsoft.NET.Test.Sdk** v17.8.0 (uruchamianie testów)
* **xunit** v2.5.3 (framework testowy)
* **xunit.runner.visualstudio** v2.5.3 (integracja testów w Visual Studio)

## Wymagania wstêpne

* .NET 8 SDK ([pobierz](https://dotnet.microsoft.com/download/dotnet/8.0))
* SQL Server (lub LocalDB w œrodowisku developerskim)
* (opcjonalnie) narzêdzie `dotnet-ef`:

  ```bash
  dotnet tool install --global dotnet-ef
  ```
* (opcjonalnie) IDE: Visual Studio 2022+, Rider lub VS Code z rozszerzeniem C#

## Konfiguracja projektu

1. Otwórz plik **AirTracker/appsettings.json** i dostosuj ustawienia:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=tcp:fajnyserver.database.windows.net,1433;Initial Catalog=airtrackerdb;Persist Security Info=False;User ID=sqladmin;Password=silnehaslo123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   },
   "OpenAQ": {
     "ApiKey": "<d5dcf83a43f170476aa6327168dac24d752e41c8b5656b6fd9f1a51ac9958d19>"
   }
   ```
2. Alternatywnie mo¿esz u¿yæ zmiennych œrodowiskowych:

   * `ConnectionStrings__DefaultConnection` — connection string do bazy danych
   * `OpenAQ__ApiKey` — klucz API OpenAQ

## Instalacja i uruchomienie

1. Sklonuj repozytorium lub rozpakuj archiwum:

   ```bash
   git clone <repozytorium>
   cd AirTracker
   ```
2. Przywróæ pakiety NuGet:

   ```bash
   dotnet restore
   ```
3. Zainstaluj narzêdzie `dotnet-ef` (jeœli jeszcze nie jest zainstalowane):

   ```bash
   dotnet tool install --global dotnet-ef
   ```
4. Zastosuj migracje i utwórz bazê danych:

   ```bash
   dotnet ef database update
   ```
5. Uruchom aplikacjê:

   ```bash
   dotnet run --project AirTracker
   ```

   Podczas pierwszego uruchomienia aplikacja zaimportuje domyœlny zestaw miast i wyœwietli w konsoli komunikat:

   > `>>> KONIEC importu. SprawdŸ bazê.`
6. Otwórz przegl¹darkê i przejdŸ pod adres podany w konsoli (domyœlnie `https://localhost:5001`).

## Uruchamianie testów

1. PrzejdŸ do katalogu z testami:

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
* Reset bazy (usuñ istniej¹c¹ bazê, ponownie zastosuj migracje):

  ```bash
  dotnet ef database drop --force
  dotnet ef database update
  ```
* Klucz API oraz connection string mo¿na te¿ dostarczyæ jako argumenty linii poleceñ lub za pomoc¹ Secret Manager w ASP.NET Core.

---

*README wygenerowane automatycznie na podstawie analizy projektu.*
