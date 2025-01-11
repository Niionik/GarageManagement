# Dokumentacja Projektu GarageManagement

## 1. Wprowadzenie

Projekt GarageManagement to aplikacja do zarządzania garażami i samochodami, umożliwiająca użytkownikom dodawanie, edytowanie i usuwanie danych o pojazdach oraz zarządzanie kontami użytkowników. Aplikacja wspiera różne role użytkowników, takie jak właściciele i administratorzy, oferując różne poziomy dostępu do funkcji.

## 2. Wymagania systemowe

- **System operacyjny**: Windows, macOS, Linux
- **.NET SDK**: 6.0 lub nowszy
- **Baza danych**: SQL Server
- **Przeglądarka**: Chrome, Firefox, Edge

## 3. Instalacja

1. **Klonowanie repozytorium**:
   ```bash
   git clone https://github.com/your-repo/garage-management.git
   cd garage-management
   ```

2. **Konfiguracja bazy danych**:
   - Upewnij się, że SQL Server jest uruchomiony.
   - Zaktualizuj `appsettings.json` z odpowiednim connection stringiem.

3. **Migracje bazy danych**:
   ```bash
   dotnet ef database update
   ```

4. **Uruchomienie aplikacji**:
   ```bash
   dotnet run
   ```

## 4. Użytkownicy testowi

- **Administrator**:
  - Email: admin@example.com
  - Hasło: AdminPassword123!
- **Zwykły użytkownik**:
  - Email: owner@example.com
  - Hasło: Password123!

## 5. Opis funkcjonalności

### Formularze

- **Rejestracja**: Umożliwia tworzenie nowych kont użytkowników. Formularz wymaga podania adresu e-mail, imienia, nazwiska oraz hasła.
- **Logowanie**: Umożliwia użytkownikom dostęp do aplikacji po podaniu poprawnych danych logowania.
- **Dodawanie samochodu**: Formularz do dodawania nowych pojazdów z walidacją danych, takich jak marka, model, rok produkcji, przebieg i status.
- **Edycja samochodu**: Umożliwia użytkownikom aktualizację danych istniejących pojazdów.
- **Dodawanie garażu**: Formularz do tworzenia nowych garaży, wymagający podania nazwy i lokalizacji.
- **Edycja garażu**: Umożliwia użytkownikom aktualizację danych istniejących garaży.
- **Dodawanie naprawy**: Formularz do rejestrowania nowych napraw pojazdów, w tym daty, opisu i kosztu.
- **Edycja naprawy**: Umożliwia użytkownikom aktualizację danych istniejących napraw.
- **Edycja profilu użytkownika**: Umożliwia użytkownikom aktualizację swoich danych osobowych, takich jak imię, nazwisko, adres e-mail i hasło.
- **Zmiana hasła**: Formularz umożliwiający użytkownikom zmianę hasła po zalogowaniu.

### Encje

- **User**: Zarządza danymi użytkowników, w tym ich rolami i uprawnieniami.
- **Car**: Przechowuje informacje o pojazdach, takie jak marka, model, rok produkcji, przebieg, status, oraz szczegóły dotyczące opon i ostatnich serwisów.
- **Garage**: Przechowuje informacje o garażach, w tym nazwę, lokalizację i właściciela.
- **Maintenance**: Rejestruje historię napraw pojazdów, w tym datę, opis i koszt naprawy.

### Autoryzacja

- **Zwykli użytkownicy**: Mają dostęp do podstawowych funkcji, takich jak przeglądanie i zarządzanie własnymi pojazdami oraz garażami.
- **Administratorzy**: Mają dostęp do zaawansowanych funkcji zarządzania, takich jak zarządzanie użytkownikami i ich rolami. Dodatkowo, administratorzy mogą dublować dane, co pozwala na szybkie tworzenie kopii istniejących rekordów, takich jak garaże i samochody.

### WebAPI

- **CRUD dla encji Car**:
  - **Create**: Dodawanie nowego pojazdu.
  - **Read**: Pobieranie informacji o pojazdach.
  - **Update**: Aktualizacja danych pojazdu.
  - **Delete**: Usuwanie pojazdu.

## 6. Uruchamianie aplikacji

Aplikację można uruchomić, wykonując polecenie `dotnet run` w katalogu głównym projektu. Po uruchomieniu aplikacja będzie dostępna pod adresem `http://localhost:7123`.

## 7. Uwagi końcowe

- Upewnij się, że wszystkie zależności są zainstalowane przed uruchomieniem aplikacji.
- Regularnie aktualizuj dokumentację w miarę wprowadzania zmian w projekcie.

## 8. Autorzy

- Patrycja Opałacz 
- Konrad Kopacz

## 9. Zakładki i ich przeznaczenie

### Strona główna

- **Opis**: Strona główna aplikacji, zawiera podstawowe informacje o projekcie i jego funkcjonalnościach.

### Zarządzanie Garażami

- **Opis**: Umożliwia użytkownikom przeglądanie, dodawanie, edytowanie i usuwanie garaży. Każdy garaż jest powiązany z właścicielem.

### Zarządzanie Samochodami

- **Opis**: Umożliwia użytkownikom przeglądanie, dodawanie, edytowanie i usuwanie samochodów. Samochody mogą być przypisane do garaży.

### Historia Napraw

- **Opis**: Umożliwia użytkownikom przeglądanie historii napraw dla każdego pojazdu, w tym szczegóły dotyczące daty, opisu i kosztu naprawy.

### Profil Użytkownika

- **Opis**: Umożliwia użytkownikom przeglądanie i edytowanie swoich danych profilowych, takich jak imię, nazwisko i adres e-mail.

### Panel Administratora

- **Opis**: Dostępny tylko dla administratorów, umożliwia zarządzanie użytkownikami, ich rolami oraz przeglądanie raportów. Administratorzy mogą również dublować dane, co ułatwia zarządzanie dużą ilością informacji.
